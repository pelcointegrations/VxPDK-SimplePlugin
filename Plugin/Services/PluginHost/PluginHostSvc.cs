using Pelco.Phoenix.PluginHostInterfaces;
using PluginNs.Utilities;
using System;

namespace PluginNs.Services.PluginHost
{
    class PluginHostSvc : IPluginHostSvc
    {
        private IOCCHostGeneral _host1;
        private IOCCHostOverlay _overlay;
        private IOCCHostOverlayEx _overlayEx;
        private IOCCHostPlaybackController _playback;
        private IOCCHostSerenity _hostSerenity;
        private int _pluginWidth;

        public PluginHostSvc()
        {
            _host1 = Utils.I.OCCHost?.GetService<IOCCHostGeneral>();
            _overlay = Utils.I.OCCHost?.GetService<IOCCHostOverlay>();
            _overlayEx = Utils.I.OCCHost?.GetService<IOCCHostOverlayEx>();
            _playback = Utils.I.OCCHost?.GetService<IOCCHostPlaybackController>();
            _hostSerenity = Utils.I.OCCHost?.GetService<IOCCHostSerenity>();

            _playback?.RegisterForVideoPlaybackNotifications(true);

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
            _playback?.Seek(position);
        }

        public void RequestClose()
        {
            Utils.I.OCCHost?.RequestClose();
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
