using System.Collections.Generic;
using ActivityMonitor.Data;

namespace ActivityMonitor.Interfaces
{
    public interface IPendingDataSaver
    {
        void Save(List<Activity> activities);
        List<Activity> Load();
    }
}
