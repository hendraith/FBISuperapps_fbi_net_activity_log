using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ActivityLog.Features.Healthz.Controllers
{
    [Route("healthz")]
    [ApiController]
    public class HealthzController : ControllerBase
    {
        private readonly IMongoClient _mongoClient;

        public HealthzController(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        [HttpGet, Route("/healthz")]
        public async Task<HealthCheckResult> Index(CancellationToken cancellationToken = default)
        {
            MongoDatabaseSettings options = new MongoDatabaseSettings();
            options.ReadConcern = ReadConcern.Majority;

            var db = _mongoClient.GetDatabase("platform_aggregator", options);

            try
            {
                await db.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("failed to ping mongodb database: " + ex.Message);
                return HealthCheckResult.Unhealthy("MongoDB health check failure");
            }

            return HealthCheckResult.Healthy("healthy");
        }
    }
}
