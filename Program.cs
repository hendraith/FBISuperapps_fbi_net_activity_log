using ActivityLog.Business.ProductPrice;
using ActivityLog.Business.SoldOut;
using ActivityLog.Config;
using ActivityLog.Features.Consumer;
using ActivityLog.Repository.ProductPrice;
using ActivityLog.Repository.SoldOut;
using ActivityLog.Util;
using FNBLibrary.Middlewares;
using FNBLibrary.Models;
using FNBLibrary.Services;
using KLGLib.common.helpers;
using KLGLib.common.log;
using KLGLib.config;
using Microsoft.OpenApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using MongoDB.Driver.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// get data from env
var appConfig = await ConfigHelper.loadConfigFromServerByEnv<AppConfig>("ACCESS_TOKEN");

//Rabbit MQ
var eventHelper = new EventHelper(appConfig.rabbit_url.getRabbitMQUri(), appConfig.rabbit_EventBus, appConfig.rabbit_eventQueueName, appConfig.AppName + "_" + appConfig.ModuleName);

builder.Services.AddHostedService((e) =>
{
    return eventHelper;
});

// Logging
IMyLog logging = new MyLog(eventHelper)
{
    appName = appConfig.AppName,
    moduleName = appConfig.ModuleName,
    logExchangeName = appConfig.rabbit_LoggingQueue,
    logQueueName = "",
};

// initiate mongo client service.
MongoClientSettings mongoSetting = MongoClientSettings.FromUrl(new MongoUrl(appConfig.MONGO_URL.getMongoUri()));
mongoSetting.AllowInsecureTls = true;
mongoSetting.LinqProvider = LinqProvider.V3;
mongoSetting.ClusterConfigurator = cb =>
{
    cb.Subscribe<CommandStartedEvent>(e =>
    {
        Console.WriteLine($"{e.CommandName} - {e.Command.ToJson()}");
    });
};

var mongoClient = new MongoClient(mongoSetting);

{
    var services = builder.Services;

    // KLGLib env
    services.AddSingleton(appConfig);

    // KLGLib
    services.AddSingleton(logging);
    services.AddSingleton<IEventHelper>(eventHelper);

    services.AddSingleton<IVerify>(x =>
        new Verify(appConfig.JWT_ISSUER, appConfig.JWT_SECRET));

    // MongoDB
    services.AddSingleton<IMongoClient>(mongoClient);

    // RabbitMQ
    services.AddSingleton<IRabbitMqService, RabbitMQService>();

    // Sold Out
    services.AddSingleton<ISoldOutRepository, SoldOutRepository>();
    services.AddSingleton<ISoldOutBusiness, SoldOutBusiness>();
    
    // Product Price
    services.AddSingleton<IProductPriceRepository, ProductPriceRepository>();
    services.AddSingleton<IProductPriceBusiness, ProductPriceBusiness>();

    // Activity Consumer 
    services.AddHostedService<ActivityConsumer>();
}

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Activity Log Service",
        Description = "A Service for log transaction data",
        Contact = new OpenApiContact
        {
            Name = "IT",
            Email = "itklgroup@kawanlamaretail.com"
        },
        License = new OpenApiLicense
        {
            Name = "License By IT CORP"
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

eventHelper.startReceiving();

app.UseMiddleware<RequestResponseLogging<JWTModel>>(logging, appConfig.JWT_SECRET, appConfig.API_KEY, new string[]{
        "/", "/index.html", "/healthz"
    });

app.UseAuthorization();

app.MapControllers();

app.Run();
