using ActivityLog.Business.ProductPrice;
using ActivityLog.Business.SoldOut;
using ActivityLog.Model;
using ActivityLog.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ActivityLog.Features.Consumer
{
    public class ActivityConsumer : IHostedService
    {
        private readonly string QueueName = "activity_log";
        private readonly IModel _model;
        private readonly IConnection _connection;
        private readonly ISoldOutBusiness _soldOutBusiness;
        private readonly IProductPriceBusiness _productPriceBusiness;

        // declare consumer type 
        const string TYPE_SOLD_OUT = "sold_out";
        const string TYPE_PRODUCT_PRICE = "product_price";

        public ActivityConsumer(IRabbitMqService rabbitMqService, ISoldOutBusiness soldOutBusiness, IProductPriceBusiness productPriceBusiness)
        {
            _connection = rabbitMqService.CreateChannel();
            _model = _connection.CreateModel();

            _soldOutBusiness = soldOutBusiness;
            _productPriceBusiness = productPriceBusiness;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _model.QueueDeclare(queue: QueueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            _model.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new AsyncEventingBasicConsumer(_model);
            consumer.Received += async (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($" [x] Received {message}");

                try
                {
                    JObject messageObj = JObject.Parse(message);
                    if (messageObj["type"] == null)
                    {
                        throw new Exception("type is required");
                    }

                    var type = messageObj["type"].ToString();

                    if (messageObj["data"] == null)
                    {
                        throw new Exception("data is required");
                    }

                    var payload = messageObj["data"].ToString();

                    switch (type)
                    {
                        case TYPE_SOLD_OUT:
                            var soldOut = JsonConvert.DeserializeObject<SoldOutModel>(payload);
                            await _soldOutBusiness.New(soldOut);
                            break;
                        case TYPE_PRODUCT_PRICE:
                            var productPrice = JsonConvert.DeserializeObject<ProductPriceModel>(payload);
                            await _productPriceBusiness.New(productPrice);
                            break;
                        default:
                            Console.WriteLine(type + " is not implemented");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("payload: " + message + ", error: " + e);
                }
                finally
                {
                    _model.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
            };

            _model.BasicConsume(
                queue: QueueName,
                autoAck: false,
                consumer: consumer);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
