using Pelco.Phoenix.PluginHostInterfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Diagnostics;


namespace ExampleDllPlugin
{
    class Plugin : PluginBase, IOCCPlugin1, IOCCPluginJoystick
    {
        #region Constructor
        private PluginModel _pluginModel = null;

        public Plugin(IHost host)
        {
            _pluginModel = new PluginModel(host);
        }
        #endregion

        #region IPluginMethods
        public override FrameworkElement CreateControl()
        {
            // System.Diagnostics.Debugger.Break();
            _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IPlugin.CreateControl()");
            return _pluginModel.PluginCreateControl();
        }

        public override string Description
        {
            get 
            {
                _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IPlugin.Description");
                return "Example Dll Plugin"; 
            }
        }

        public override string GetPluginKey()
        {
            _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IPlugin.GetPluginKey()");
            return "ExampleDll-PluginKey";
        }

        public override bool IsOverlay
        {
            get {
                _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IPlugin.IsOverlay");
                return false; 
            }
        }

        public override string Name
        {
            get {
                _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IPlugin.Name");
                return "ExampleDll"; 
            }
        }

        public override string PluginID
        {
            get {
                _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IPlugin.ID");
                return "1"; 
            }
        }

        public override void Shutdown()
        {
            _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IPlugin.Shutdown()");
            _pluginModel.PluginShutdown();
        }

        public override string Version
        {
            get {
                _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IPlugin.Version");
                return "1.2"; 
            }
        }

        #endregion

        #region IOCCPlugin1Methods

        #region PreferenceNotifications
        public void OnThumbnailPreferenceNotification(bool thumbnailPrefNotify)
        {
            _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IOCCPlugin1.OnThumbnailPreferenceNotification()");
        }

        public void OnCameraOnScreeen(string cameraId, bool onScreen)
        {
            _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IOCCPlugin1.OnCameraOnScreen()");
        }
        #endregion

        #region StartupAndShutdown
        public bool RequiresCredentials
        {
            get
            {
                _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IOCCPlugin1.RequiresCredentials");
                return true;
            }
        }

        public void Login(string credentials)
        {
            _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IOCCPlugin1.Login()");
        }

        public void Logout()
        {
            _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IOCCPlugin1.Logout()");
        }
        #endregion

        #region SavingAndRestoringState
        public string GetPluginState()
        {
            _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IOCCPlugin1.GetPluginState()");
            return "Example Plugin State";
        }

        public void SetPluginState(string state)
        {
            _pluginModel.TraceEvent(TraceEventType.Information, 0, "Host calling IOCCPlugin1.SetPluginState()");
        }
        #endregion

        #endregion

        #region IOCCPluginJoystick
        public void OnJoystickNotification(int x, int y, int z, int u)
        {
            string traceMsg = "Host calling IOCCPluginJoystick.OnJoystickNotification(" + x + ", " + y + ", " + z + ", " + u + ")";
            _pluginModel.TraceEvent(TraceEventType.Verbose, 0, traceMsg);
        }
        #endregion

    }
}
