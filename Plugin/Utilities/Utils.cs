using Pelco.Phoenix.PluginHostInterfaces;
using PluginNs.Services.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Threading;
using System.Xml.Serialization;

namespace PluginNs.Utilities
{
    class Utils
    {
        private static Utils _instance = null;

        public static Utils Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Utils();
                return _instance;
            }
        }

        /////////////////////////////////////////////////////////////

        public bool IsPlugin { get; set; }
        public IHost OCCHost { get; set; }
        public Dispatcher MainDispatcher { get; set; }

        private Dictionary<string, object> _cache;

        private Utils()
        {
            _cache = new Dictionary<string, object>();
        }

        public void RunOnUi(Action action)
        {
            if (MainDispatcher != null)
                MainDispatcher.BeginInvoke(action);
            else
                Dispatcher.CurrentDispatcher.BeginInvoke(action);
        }

        public void SetCacheItem<T>(string key, T obj)
        {
            _cache[key] = obj;
        }

        public T GetCacheItem<T>(string key) where T : class
        {
            T item = default(T);
            if (_cache.ContainsKey(key))
                item = _cache[key] as T;
            return item;
        }

        public string Serialize<T>(T obj) where T : class
        {
            string serialized = string.Empty;
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                using (var writer = new StringWriter())
                {
                    serializer.Serialize(writer, obj);
                    string data = writer.ToString();
                    serialized = Convert.ToBase64String(Encoding.UTF8.GetBytes(data));
                }
            }
            catch (Exception e)
            {
                GetCacheItem<ILogger>(Const.ILogger)?.Log("Threw while serializing", e);
            }
            return serialized;
        }

        public T Deserialize<T>(string serialized) where T : class
        {
            T deserialized = default(T);
            try
            {
                var serializer = new XmlSerializer(typeof(T));
                serialized = Encoding.UTF8.GetString(Convert.FromBase64String(serialized));
                using (var reader = new StringReader(serialized))
                {
                    deserialized = serializer.Deserialize(reader) as T;
                }
            }
            catch (Exception e)
            {
                GetCacheItem<ILogger>(Const.ILogger)?.Log("Threw while deserializing", e);
            }
            return deserialized;
        }

        public T DeserializeJson<T>(string serialized) where T : class
        {
            T deserialized = default(T);
            try
            {
                var js = new JavaScriptSerializer();
                deserialized = js.Deserialize<T>(serialized);
            }
            catch (Exception e)
            {
                GetCacheItem<ILogger>(Const.ILogger)?.Log("Threw while deserializing json", e);
            }
            return deserialized;
        }

        public string ToRfc3339(DateTime time)
        {
            string strTime = string.Empty;
            try
            {
                return time.ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'");
            }
            catch (Exception e)
            {
                GetCacheItem<ILogger>(Const.ILogger)?.Log("Threw while converting time to RFC3339", e);
            }
            return strTime;
        }

        public string CurrentVersion
        {
            get
            {
                var assembly = System.Reflection.Assembly.GetExecutingAssembly();
                var version = System.Reflection.AssemblyName.GetAssemblyName(assembly.Location).Version.ToSt‌​ring();
                return version;
            }
        }
    }
}
