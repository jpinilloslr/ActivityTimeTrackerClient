using System.Collections.Generic;
using ActivityMonitor.Data;

namespace ActivityMonitor.Interfaces
{
    interface IActivityStack
    {

        void Add(Activity activity);

        void Clear();

        void Remove(List<Activity> activities);

        List<Activity> GetActivities();

        void Load();

        void Save();

        bool HasPendingData();
    }
}
