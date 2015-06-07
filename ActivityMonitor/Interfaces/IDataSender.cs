using System.Collections.Generic;
using ActivityMonitor.Data;

namespace ActivityMonitor.Interfaces
{
    public interface IDataSender
    {
        string ResourceEndPoint { get; set; }

        bool IsSending();

        bool Send(List<Activity> activities);
    }
}
