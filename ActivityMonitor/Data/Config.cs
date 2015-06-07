using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace ActivityMonitor.Data
{
    public class Config
    {
        private const string FileName = ".config";
        public string Server { get; set; }

        public void Save()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(stream, this);
                stream.Close();
            }
            catch { }  
        }

        public void Load()
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                var config = (Config)formatter.Deserialize(stream);
                stream.Close();

                Server = config.Server;
            }
            catch { }
        }
    }
}
