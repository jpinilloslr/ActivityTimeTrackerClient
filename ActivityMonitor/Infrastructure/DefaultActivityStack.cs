using System.Collections.Generic;
using System.Linq;
using ActivityMonitor.Data;
using ActivityMonitor.Interfaces;

namespace ActivityMonitor.Infrastructure
{
    public class DefaultActivityStack : IActivityStack
    {
        private readonly IPendingDataSaver _pendingDataSaver;
        private List<Activity> _activities = new List<Activity>();

        public DefaultActivityStack(IPendingDataSaver pendingDataSaver)
        {
            _pendingDataSaver = pendingDataSaver;
        }

        public void Add(Activity activity)
        {
            _activities.Add(activity);    
        }

        public void Clear()
        {
            _activities.Clear();
        }

        public void Remove(List<Activity> activities)
        {
            foreach (var activity in activities)
            {
                _activities.Remove(activity);
            }
        }

        public List<Activity> GetActivities()
        {
            return _activities;
        }

        public void Load()
        {
            _activities = _pendingDataSaver.Load();            
        }

        public void Save()
        {
            _pendingDataSaver.Save(_activities);
        }

        public bool HasPendingData()
        {
            return _activities.Any();
        }
    }
}
