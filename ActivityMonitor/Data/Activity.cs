
using System;
using System.Runtime.Serialization;

namespace ActivityMonitor.Data
{
    [Serializable]
    [DataContract(Name = "activity")]
    public class Activity
    {
        [DataMember(Name = "windowText")]
        public string WindowText { get; set; }

        [DataMember(Name = "processName")]
        public string ProcessName { get; set; }

        [DataMember(Name = "moduleFilename")]
        public string ModuleFilename { get; set; }

        [DataMember(Name = "life")]
        public double Life { get; set; }

        [DataMember(Name = "date")]
        public DateTime Date { get; set; }

        public bool Equals(Activity activity)
        {
            return WindowText == activity.WindowText &&
                    ModuleFilename == activity.ModuleFilename &&
                    ProcessName == activity.ProcessName &&
                    Date.Ticks == activity.Date.Ticks;
        }
    }
}
