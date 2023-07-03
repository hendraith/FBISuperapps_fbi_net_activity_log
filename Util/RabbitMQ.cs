using ActivityLog.Config;
using RabbitMQ.Client;

namespace ActivityLog.Util
{
    public interface IRabbitMqService
    {
        IConnection CreateChannel();
    }

    public class RabbitMQService : IRabbitMqService
    {
        private readonly AppConfig _appConfig;

        public RabbitMQService(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }

        public IConnection CreateChannel()
        {
            ConnectionFactory connection = new ConnectionFactory()
            {
                UserName = _appConfig.rabbit_url.user_name,
                Password = _appConfig.rabbit_url.password,
                HostName = _appConfig.rabbit_url.host,
                Port = 5672,
            };

            connection.DispatchConsumersAsync = true;
            var channel = connection.CreateConnection();

            return channel;
        }
    }
}
