
using System.Collections.Generic;
using ActivityMonitor.Data;

namespace ActivityMonitor.Interfaces
{
    public interface IActivityMonitor
    {
        int RefreshInterval { get; set; }

        int SendDataInterval { get; set; }

        int GetActiveWindow();

        void ActivityChanged(int lastHwnd, int newHwnd, long time);

        void SendData(List<Activity> activities);

        void Start();

        void Stop();

        bool Started();

        List<Activity> GetRegisteredActivities();

    }
}
