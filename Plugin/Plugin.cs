using Microsoft.Practices.Unity;
using Pelco.Phoenix.PluginHostInterfaces;
using PluginNs.Events;
using PluginNs.Models;
using PluginNs.Services.Host;
using PluginNs.Utilities;
using PluginNs.Views;
using Prism.Events;
using System;
using System.Windows;
using System.Windows.Threading;

namespace PluginNs
{
    class Plugin : PluginBase, IOCCPlugin1, IOCCPluginOverlay
    {
        private Bootstrapper _bootstrapper = new Bootstrapper(true);
        private IEventAggregator _aggregator;
        private IPluginHost _pluginHost;

        public Plugin(IHost host)
        {
            Utils.Instance.OCCHost = host;
            Utils.Instance.MainDispatcher = Dispatcher.CurrentDispatcher;
        }

        private void RunBootstrapper()
        {
            //System.Diagnostics.Debugger.Launch();
            if (!_bootstrapper.HasRun)
            {
                _bootstrapper.Run();
                Utils.Instance.SetCacheItem(Const.PersistentModel, new PersistentModel());
                _pluginHost = _bootstrapper.Container.Resolve<IPluginHost>();
                _aggregator = _bootstrapper.Container.Resolve<IEventAggregator>();
                _aggregator.GetEvent<ShutdownCompleted>().Subscribe(_ => OnShutdownCompleted(), ThreadOption.UIThread, true);
                _pluginHost.DockRight();
            }
        }

        private void OnShutdownCompleted()
        {
            Utils.Instance.OCCHost.RequestClose();
        }

        #region Plugin
        public override string Description
        {
            get
            {
                return Properties.Resources.PluginDescription;
            }
        }

        public override bool IsOverlay
        {
            get
            {
                return true;
            }
        }

        public override string Name
        {
            get
            {
                return Properties.Resources.PluginName;
            }
        }

        public override string PluginID
        {
            get
            {
                return Const.PluginId;
            }
        }
        #endregion

        #region IOCCPlugin1
        public bool RequiresCredentials
        {
            get
            {
                return true;
            }
        }

        public override string Version
        {
            get
            {
                return Utils.Instance.CurrentVersion;
            }
        }

        public override FrameworkElement CreateControl()
        {
            RunBootstrapper();
            return _bootstrapper.Container.Resolve<MainView>();
        }

        public override string GetPluginKey()
        {
            return Const.PluginKey;
        }

        public string GetPluginState()
        {
            return string.Empty;
        }

        public void Login(string credentials)
        {
            RunBootstrapper();
            var settings = Utils.Instance.GetCacheItem<PersistentModel>(Const.PersistentModel);
            if (!string.IsNullOrWhiteSpace(credentials))
            {
                var tempSettings = Utils.Instance.Deserialize<PersistentModel>(credentials);
                if (tempSettings != null)
                {
                    settings = tempSettings;
                    Utils.Instance.SetCacheItem(Const.PersistentModel, settings);
                }
            }
            _pluginHost.SetPluginWidthDockRight(settings.PluginWidth);
        }

        public void Logout()
        { }

        public void OnCameraOnScreeen(string cameraId, bool onScreen)
        { }

        public void OnThumbnailPreferenceNotification(bool show)
        { }
        #endregion

        #region IOCCPluginOverlay
        public void OnVideoPlayPause(string dataSourceId, string number, bool live, bool playing, DateTime utcTime)
        { }

        public void OnVideoRemoved()
        { }

        public void OnVideoTimer(DateTime utcTime)
        { }

        public void SetPluginState(string pluginState)
        { }

        public override void Shutdown()
        {
            _aggregator.GetEvent<ShutdownStarted>().Publish(null);
        }
        #endregion
    }
}
