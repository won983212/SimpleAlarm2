using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SimpleAlarm2
{
    public abstract class BinaryProperties : ObservableObject
    {
        private static string _filePath;
        private Dictionary<string, object> _settingDatas = new Dictionary<string, object>();
        private volatile bool _nowReading = false;

        static BinaryProperties()
        {
            _filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "simple_alarm_cfg.dat");
        }

        public void Load()
        {
            if (File.Exists(_filePath))
            {
                _nowReading = true;
                using (BinaryReader reader = new BinaryReader(File.OpenRead(_filePath)))
                {
                    Deserialize(reader);
                }
                _nowReading = false;
            } 
            else
            {
                Save();
            }
        }

        public void Save()
        {
            if (!_nowReading)
            {
                using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(_filePath)))
                {
                    Serialize(writer);
                }
            }
        }

        protected object GetData(string key, object defaultValue)
        {
            if (_settingDatas.ContainsKey(key))
                return _settingDatas[key];
            _settingDatas.Add(key, defaultValue);
            return defaultValue;
        }

        protected void SetData(string key, object value)
        {
            if (_settingDatas.ContainsKey(key))
                _settingDatas[key] = value;
            else
                _settingDatas.Add(key, value);
            OnPropertyChanged(key);
        }

        protected abstract void Serialize(BinaryWriter writer);

        protected abstract void Deserialize(BinaryReader reader);

        public string Version
        {
            get => Assembly.GetEntryAssembly().GetName().Version.ToString();
        }
    }
}
