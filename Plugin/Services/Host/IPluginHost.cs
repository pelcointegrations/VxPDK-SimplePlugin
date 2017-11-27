using System;

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
