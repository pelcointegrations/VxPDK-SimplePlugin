using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PluginNs.Models
{
    [Serializable]
    public class PersistentModel : INotifyPropertyChanged
    {
        private const int MinPluginWidth = 200;
        private const int MaxPluginWidth = 500;
        public event PropertyChangedEventHandler PropertyChanged;
        private int _pluginWidth;

        public PersistentModel()
        {
            PluginWidth = 300;
        }

        public int PluginWidth
        {
            get { return _pluginWidth; }
            set
            {
                int val = value;
                val = Math.Min(MaxPluginWidth, val);
                val = Math.Max(MinPluginWidth, val);
                SetProperty(ref _pluginWidth, val);
            }
        }

        private void SetProperty<T>(ref T member, T value, [CallerMemberName] string name = "")
        {
            member = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
