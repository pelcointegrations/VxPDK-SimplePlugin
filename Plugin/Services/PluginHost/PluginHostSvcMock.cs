using PluginNs.Utilities;
using System;

namespace PluginNs.Services.PluginHost
{
    class PluginHostSvcMock : IPluginHostSvc
    {
        public void DockFront()
        { }

        public void DockRight()
        { }

        public string GetAuthToken()
        {
            return Const.MockAuthToken;
        }

        public string GetBaseUri()
        {
            return Const.MockBaseUri;
        }

        public void RequestClose()
        { }

        public void SetPluginWidthDockRight(int pixels)
        { }

        public void SetVideoPosition(DateTime position)
        { }

        public void StoreCredentials(string credentials)
        { }
    }
}
