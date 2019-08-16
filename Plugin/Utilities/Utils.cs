using Newtonsoft.Json;
using NLog;
using Pelco.Phoenix.PluginHostInterfaces;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Threading;

namespace PluginNs.Utilities
{
    class Utils
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        private static Utils _instance = null;

        public static Utils I
        {
            get
            {
                if (_instance == null)
                    _instance = new Utils();
                return _instance;
            }
        }
        public Dispatcher MainDispatcher { get; set; }

        public IHost OCCHost { get; set; }

        private Dictionary<string, object> _cache;

        private Utils()
        {
            _cache = new Dictionary<string, object>();
        }

        public Assembly CurrentAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }

        public bool IsConfigSet()
        {
            bool success = !string.IsNullOrWhiteSpace(Const.PluginId);
            success = success && !string.IsNullOrWhiteSpace(Const.PluginKey);
            success = success && !string.IsNullOrWhiteSpace(Const.SdkLicense);
            success = success && !string.IsNullOrWhiteSpace(Const.MockBaseUri);
            success = success && !string.IsNullOrWhiteSpace(Const.MockAuthToken);
            return success;
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
                serialized = JsonConvert.SerializeObject(obj);
            }
            catch (Exception e)
            {
                Log.Warn(e, "Threw while serializing");
            }
            return serialized;
        }

        public T Deserialize<T>(string serialized) where T : class
        {
            T deserialized = default(T);
            try
            {
                deserialized = JsonConvert.DeserializeObject<T>(serialized);
            }
            catch (Exception e)
            {
                Log.Warn(e, "Threw while deserializing");
            }
            return deserialized;
        }
    }
}
