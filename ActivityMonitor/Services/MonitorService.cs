using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActivityMonitor.Data;
using ActivityMonitor.Infrastructure;
using ActivityMonitor.Interfaces;

namespace ActivityMonitor.Services
{
    public class MonitorService
    {
        private static MonitorService _monitorService;
        private readonly IActivityMonitor _activityMonitor;
        private readonly Config _config;

        public static MonitorService GetInstance()
        {
            return _monitorService ?? (_monitorService = new MonitorService());
        }

        private MonitorService()
        {
            _activityMonitor = new DefaultActivityMonitor(
                new DefaultActivityService(),
                new DefaultActivityStack(new FilePendingDataSaver()),
                new DefaultDataSender());

            _config = new Config();
            _config.Load();
        }

        public IActivityMonitor GetActivityMonitor()
        {
            return _activityMonitor;
        }

        public Config GetConfig()
        {
            return _config;
        }
    }
}
