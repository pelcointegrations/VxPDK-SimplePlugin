using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginNs.Services.Host
{
    class PluginHostMock : IPluginHost
    {
        public void DockFront()
        { }

        public void DockRight()
        { }

        public string GetAuthToken()
        {
            return "ZXlKMWMyVnlibUZ0WlNJNkltRmtiV2x1SWl3aWNHRnpjM2R2Y21RaU9pSmhaRzFwYmpFeU15SXNJbVJ2YldGcGJpSTZJa3hQUTBGTUlpd2laWGh3YVhKbGN5STZNVFE0TkRrek1qSXhOamsxTUN3aVlXZGxiblFpT2lKQlpHMXBibEJ2Y25SaGJDSXNJbU5zYVdWdWRFbHdJam9pTVRBdU1qSXdMakl6TXk0eE1qZ2lmUT09";
        }

        public string GetBaseUri()
        {
            return "https://10.220.233.219:443/";
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
