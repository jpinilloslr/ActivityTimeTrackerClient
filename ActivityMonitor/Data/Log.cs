using System;

namespace ActivityMonitor.Data
{

    public enum LogLevel
    {
        Info,
        Warning,
        Error,
        Debug
    }

    public class Log
    {
        public DateTime Time { get; set; }
        public String Text { get; set; }
        public LogLevel LogLevel { get; set; }

        public override string ToString()
        {
            return LogLevel.ToString() + ": " + Text;
        }
    }
}
