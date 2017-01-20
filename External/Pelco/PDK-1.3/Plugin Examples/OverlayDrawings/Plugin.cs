using Pelco.Phoenix.PluginHostInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OverlayDrawings
{
    class Plugin : PluginBase, IOCCPlugin1
    {
        private IHost _host;
        private string _state = "StateInfo";

        private IOCCHostOverlay HostOverlayService { get; set; }
        private IOCCHostOverlayDrawingService HostDrawingService { get; set; }

        public Plugin(IHost host)
        {
            _host = host;

            HostOverlayService = host.GetService<IOCCHostOverlay>();
            if (HostOverlayService == null)
            {
                MessageBox.Show("IOCCHostOverlay service not available", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            HostDrawingService = host.GetService<IOCCHostOverlayDrawingService>();
            if (HostDrawingService == null)
            {
                MessageBox.Show("IOCCHostOverlayDrawingService not available", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public override string Description
        {
            get { return "Overlay Drawing Example"; }
        }

        public override bool IsOverlay
        {
           get { return true; }
        }

        public override string Name
        {
           get { return "Overlay Drawing Plugin"; }
        }

        public override string PluginID
        {
            get { return Guid.NewGuid().ToString(); }
        }

        public override string Version
        {
            get { return "1.3"; }
        }

        public bool RequiresCredentials
        {
            get { return false; }
        }

        public override FrameworkElement CreateControl()
        {
            if (HostOverlayService != null)
            {
                HostOverlayService.SetOverlayAnchor(AnchorTypes.left, 25, 450, 450);
            }

            View.MainUserControl control = new View.MainUserControl(HostDrawingService);

            return control;
        }

        public override string GetPluginKey()
        {
            //return "ExampleExe-PluginKey";
            return "eSY/QPsQvRvJOnyl2rqWTTGZDPauAKcQrbHkqckvUtP8q/k1Jh/63NlPELIA5sGgdt+ipRtflhtQjJNO3dZ3EQ==";
        }

        public override void Shutdown()
        {
            _host.RequestClose();
        }

        #region IOCCPlugin1 Members

        public void OnThumbnailPreferenceNotification(bool show)
        {
        }

        public void OnCameraOnScreeen(string cameraId, bool onScreen)
        {
        }

        public string GetPluginState()
        {
            return _state;
        }

        public void SetPluginState(string pluginState)
        {
            _state = pluginState;
        }

        public void Login(string credentials)
        {
        }

        public void Logout()
        {
        }

        #endregion
    }
}
