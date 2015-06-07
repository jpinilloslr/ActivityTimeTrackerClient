using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ActivityMonitor.Data;
using ActivityMonitor.Interfaces;
using Timer = System.Threading.Timer;

namespace ActivityMonitor.Infrastructure
{
    class DefaultActivityMonitor: IActivityMonitor
    {        

        private int _hwnd;
        private Timer _refreshTimer;
        private Timer _sendDataTimer;
        private readonly IActivityService _activityService;
        private readonly IActivityStack _activityStack;
        private readonly IDataSender _dataSender;
        private long _ticks;
        private bool _started;

        public int RefreshInterval { get; set; }
        public int SendDataInterval { get; set; }


        public DefaultActivityMonitor(IActivityService activityService, 
            IActivityStack activityStack, 
            IDataSender dataSender)
        {
            _activityStack = activityStack;
            _activityService = activityService;
            _dataSender = dataSender;
            RefreshInterval = 100;
            SendDataInterval = 20000;
            _started = false;
        }        

        public int GetActiveWindow()
        {
            var hwnd = User32Wrapper.GetForegroundWindow();

            if (hwnd != 0 && _hwnd != hwnd)
            {                
                ActivityChanged(_hwnd, hwnd, DateTime.Now.Ticks - _ticks);
                _ticks = DateTime.Now.Ticks;
                _hwnd = hwnd;
            }

            return hwnd;
        }

        public void ActivityChanged(int lastHwnd, int newHwnd, long time)
        {
            var activity = new Activity()
            {
                ModuleFilename = _activityService.GetExecutableFilename(lastHwnd),
                ProcessName = _activityService.GetProcessName(lastHwnd),
                WindowText = _activityService.GetWindowText(lastHwnd),
                Life = TimeSpan.FromTicks(time).TotalMilliseconds,
                Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day)
            };

            _activityStack.Add(activity);
            LogManager.GetInstance().Info(String.Format("Activity registered in process {1}.", activity.Life, activity.ProcessName));
        }

        public void SendData(List<Activity> activities)
        {
            var copy = new List<Activity>(activities);
            if (copy.Any() && _dataSender.Send(copy))
                _activityStack.Remove(copy);
        }

        public void Start()
        {
            LogManager.GetInstance().Info("Activity monitor started.");
            _started = true;
            _activityStack.Load();
            _hwnd = User32Wrapper.GetForegroundWindow();
            _ticks = DateTime.Now.Ticks;

            TimerCallback refreshCallback = (obj => GetActiveWindow());
            _refreshTimer = new Timer(refreshCallback, null, 0, RefreshInterval);

            TimerCallback sendCallback = (obj => SendData(_activityStack.GetActivities()));
            _sendDataTimer = new Timer(sendCallback, null, 0, SendDataInterval);
        }

        public void Stop()
        {
            LogManager.GetInstance().Info("Activity monitor stopped.");
            _refreshTimer.Dispose();
            _sendDataTimer.Dispose();
            _started = false;

            if (_activityStack.HasPendingData())
            {
                _activityStack.Save();
            }
        }

        public bool Started()
        {
            return _started;
        }

        public List<Activity> GetRegisteredActivities()
        {
            return _activityStack.GetActivities();
        }
    }
}
