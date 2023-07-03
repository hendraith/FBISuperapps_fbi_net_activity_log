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
        private readonly string SoldOutQueue = "activity_log";
        private readonly IModel _model;
        private readonly IConnection _connection;
        private readonly ISoldOutBusiness _soldOutBusiness;

        // declare consumer type 
        const string TYPE_SOLD_OUT = "sold_out";

        public ActivityConsumer(IRabbitMqService rabbitMqService, ISoldOutBusiness soldOutBusiness)
        {
            _soldOutBusiness = soldOutBusiness;
            _connection = rabbitMqService.CreateChannel();
            _model = _connection.CreateModel();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _model.QueueDeclare(queue: SoldOutQueue,
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
                            var data = JsonConvert.DeserializeObject<SoldOutModel>(payload);
                            await _soldOutBusiness.New(data);
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
                queue: SoldOutQueue,
                autoAck: false,
                consumer: consumer);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
