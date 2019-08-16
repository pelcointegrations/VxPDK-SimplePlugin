using NLog;
using PluginNs.Events;
using PluginNs.Utilities;
using PluginNs.Views;
using Prism.Commands;
using Prism.Events;
using System.Linq;

namespace PluginNs.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();
        public DelegateCommand SettingsCmd { get; private set; }
        public DelegateCommand CloseCmd { get; private set; }

        public MainViewModel(IEventAggregator aggregator)
        {
            SettingsCmd = new DelegateCommand(OnSettings);
            CloseCmd = new DelegateCommand(OnClose);
            aggregator.GetEvent<ShutdownStarted>().Subscribe(OnShutdownStarted, ThreadOption.UIThread, true);

            Utils.I.RunOnUi(OnSettings);
        }

        private void OnSettings()
        {
            var activeView = RegionMgr.Regions[Const.RegionMainView].ActiveViews.FirstOrDefault();
            string viewName = activeView is DefaultView ? nameof(SettingsView) : nameof(DefaultView);
            RegionMgr.RequestNavigate(Const.RegionMainView, viewName);
        }

        private void OnClose()
        {
            Aggregator.GetEvent<ShutdownStarted>().Publish();
        }

        protected override void Dispose(bool dispose)
        {
            base.Dispose(dispose);
        }

        private void OnShutdownStarted()
        {
            foreach (var region in RegionMgr.Regions)
            {
                foreach (var view in region.Views)
                    region.Remove(view);
            }
            Dispose();
            Aggregator.GetEvent<ShutdownCompleted>().Publish();
        }
    }
}