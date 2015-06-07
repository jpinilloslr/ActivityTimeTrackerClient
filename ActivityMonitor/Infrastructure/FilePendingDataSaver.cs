using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using ActivityMonitor.Data;
using ActivityMonitor.Interfaces;

namespace ActivityMonitor.Infrastructure
{
    public class FilePendingDataSaver : IPendingDataSaver
    {
        private const string FileName = ".pendingdata";

        public void Save(List<Activity> activities)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, activities);
                stream.Close();
                LogManager.GetInstance().Debug("Pending data saved in local storage.");
            }
            catch (Exception e)
            {
                LogManager.GetInstance().Debug(e.Message);
            }
         
        }

        public List<Activity> Load()
        {
            var result = new List<Activity>();

            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                result = (List<Activity>) formatter.Deserialize(stream);
                stream.Close();

                File.Delete(FileName);
                LogManager.GetInstance().Debug("Pending data loaded from local storage.");
            }
            catch(Exception e)
            {
                LogManager.GetInstance().Debug(e.Message);
            }
            

            return result;
        }
    }
}
