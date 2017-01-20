using Pelco.Phoenix.PluginHostInterfaces;
using PluginNs.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginNs.Services.Host
{
    class PluginHost : IPluginHost
    {
        private IOCCHost1 _host1;
        private IOCCHostOverlay _overlay;
        private IOCCHostOverlayEx _overlayEx;
        private IOCCHostSetVideoPosition _position;
        private IOCCHostSerenity _hostSerenity;
        private IOCCHostOverlayDrawingService _draw;
        private int _pluginWidth;

        public PluginHost()
        {
            _host1 = Utils.Instance.OCCHost?.GetService<IOCCHost1>();
            _overlay = Utils.Instance.OCCHost?.GetService<IOCCHostOverlay>();
            _overlayEx = Utils.Instance.OCCHost?.GetService<IOCCHostOverlayEx>();
            _position = Utils.Instance.OCCHost?.GetService<IOCCHostSetVideoPosition>();
            _hostSerenity = Utils.Instance.OCCHost?.GetService<IOCCHostSerenity>();
            _draw = Utils.Instance.OCCHost?.GetService<IOCCHostOverlayDrawingService>();
            _pluginWidth = 300;
        }

        public string GetAuthToken()
        {
            return _hostSerenity?.GetAuthToken();
        }

        public string GetBaseUri()
        {
            return _hostSerenity?.GetBaseURI();
        }

        public void SetVideoPosition(DateTime position)
        {
            _position?.SetVideoPosition(position);
        }

        public void RequestClose()
        {
            Utils.Instance.OCCHost?.RequestClose();
        }

        public void StoreCredentials(string credentials)
        {
            _host1?.StoreCredentials(credentials);
        }

        public void SetPluginWidthDockRight(int pixels)
        {
            _pluginWidth = pixels;
            DockRight();
        }

        public void DockRight()
        {
            _overlay?.SetOverlayAnchor(AnchorTypes.right, 5, _pluginWidth, _pluginWidth);
        }

        public void DockFront()
        {
            _overlay?.SetOverlayAnchor(AnchorTypes.inFront, 90, 30, 30);
        }
    }
}
