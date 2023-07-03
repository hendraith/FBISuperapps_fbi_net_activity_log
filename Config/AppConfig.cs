using KLGLib.config.ConfigType;

namespace ActivityLog.Config
{
    public class AppConfig
    {
        #region apps
        public string AppName { get; set; }
        public string ModuleName { get; set; }
        #endregion

        #region rabbit
        public DBServer rabbit_url { get; set; } = default!;
        public string rabbit_LoggingQueue { get; set; } = default!;
        public string rabbit_EventBus { get; set; } = default!;
        public string rabbit_eventQueueName { get; set; } = default!;
        public string rabbit_EmailQueue { get; set; } = default!;
        #endregion

        public DBServer MONGO_URL { get; set; } = new DBServer { };
    }
}
