using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginNs.Services.Host
{
    interface IPluginHost
    {
        void RequestClose();
        void StoreCredentials(string credentials);
        void SetPluginWidthDockRight(int pixels);
        void DockRight();
        void DockFront();
        void SetVideoPosition(DateTime position);
        string GetAuthToken();
        string GetBaseUri();
    }
}
