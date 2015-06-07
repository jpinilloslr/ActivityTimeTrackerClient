using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Threading;
using ActivityMonitor.Data;
using ActivityMonitor.Interfaces;

namespace ActivityMonitor.Infrastructure
{
    public class DefaultDataSender: IDataSender
    {
        private ManualResetEvent _allDone;        
        private bool _succeed;
        private bool _sending;

        public List<Activity> Activities { get; set; }
        public string ResourceEndPoint { get; set; }

        public DefaultDataSender()
        {
            ResourceEndPoint = "http://localhost/att/api/activity";
        }

        public bool IsSending()
        {
            return _sending;
        }

        public bool Send(List<Activity> activities)
        {
            if (!IsSending())
            {
                _sending = true;
                _succeed = false;
                _allDone = new ManualResetEvent(false);

                CompactData(activities);
                var request = (HttpWebRequest)WebRequest.Create(ResourceEndPoint);
                request.Method = "POST";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                request.UserAgent = "ATT";
                request.BeginGetRequestStream(GetRequestStreamCallback, request);
                _allDone.WaitOne();
                Console.WriteLine(_succeed.ToString());

                
                if(_succeed)
                    LogManager.GetInstance().Info("Data sended to server.");
                else
                    LogManager.GetInstance().Error("Data could not be sended to server.");
            }
            
            return _succeed;
        }

        private void GetRequestStreamCallback(IAsyncResult asynchronousResult)
        {
            try
            {
                var request = (HttpWebRequest) asynchronousResult.AsyncState;
                var postStream = request.EndGetRequestStream(asynchronousResult);

                var serializer = new DataContractJsonSerializer(typeof (List<Activity>));
                serializer.WriteObject(postStream, Activities);
                postStream.Close();

                var response = (HttpWebResponse)request.GetResponse();

                _sending = false;
                _succeed = (response.StatusCode == HttpStatusCode.Created);
                _allDone.Set();
            }
            catch(Exception e)
            {
                LogManager.GetInstance().Debug(e.Message);
                _sending = false;
                _allDone.Set();
            }            
        }       

        private void CompactData(IEnumerable<Activity> activities)
        {
            Activities = new List<Activity>();
            foreach (var activity in activities)
            {
                var list = Activities.Where(m => m.Equals(activity)).ToList();

                if (list.Any())
                {
                    list[0].Life += activity.Life;
                }
                else
                {
                    Activities.Add(activity);
                }            
            }
        }
    }
}
