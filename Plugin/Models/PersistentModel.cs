using Newtonsoft.Json;
using Prism.Mvvm;
using System;

namespace PluginNs.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PersistentModel : BindableBase
    {
        private const int MinPluginWidth = 200;
        private const int MaxPluginWidth = 500;
        private int _pluginWidth;

        public PersistentModel()
        {
            PluginWidth = 300;
        }

        [JsonProperty]
        public int PluginWidth
        {
            get => _pluginWidth;
            set
            {
                int val = value;
                val = Math.Min(MaxPluginWidth, val);
                val = Math.Max(MinPluginWidth, val);
                SetProperty(ref _pluginWidth, val);
            }
        }
    }
}
