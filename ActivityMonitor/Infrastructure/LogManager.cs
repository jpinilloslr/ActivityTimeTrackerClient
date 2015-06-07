using System;
using System.Collections.Generic;
using System.Linq;
using ActivityMonitor.Data;

namespace ActivityMonitor.Infrastructure
{
    public class LogManager 
    {
        private static LogManager _logManager;
        private readonly List<Log> _logs;


        private void AddEntry(LogLevel logLevel, string text)
        {
            _logs.Add(new Log()
            {
                LogLevel = logLevel,
                Text = text,
                Time = DateTime.Now
            });
        }

        public static LogManager GetInstance()
        {
            return _logManager ?? (_logManager = new LogManager());
        }

        private LogManager()
        {
            _logs = new List<Log>();
        }

        public List<Log> GetLogs()
        {
            return _logs.ToList();
        }

        public void Clear()
        {
            _logs.Clear();
        }

        public void Info(string text)
        {
            AddEntry(LogLevel.Info, text);
        }

        public void Warning(string text)
        {
            AddEntry(LogLevel.Warning, text);
        }

        public void Error(string text)
        {
            AddEntry(LogLevel.Error, text);
        }

        public void Debug(string text)
        {
            AddEntry(LogLevel.Debug, text);
        }
    }
}
