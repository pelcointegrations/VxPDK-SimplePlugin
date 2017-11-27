using Microsoft.Practices.Unity;
using PluginNs.Models;
using PluginNs.Services.Host;
using PluginNs.Services.Logging;
using PluginNs.Utilities;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace PluginNs.ViewModels
{
    class BaseViewModel : BindableBase, INavigationAware, IDisposable
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        [Dependency]
        public IPluginHost PluginHost { get; set; }

        [Dependency]
        public ILogger Logger { get; set; }

        [Dependency]
        public IRegionManager RegionMgr { get; set; }

        [Dependency]
        public IEventAggregator Aggregator { get; set; }

        public PersistentModel Settings { get; set; }

        public BaseViewModel()
        {
            Settings = Utils.Instance.GetCacheItem<PersistentModel>(Const.PersistentModel);
        }

        public virtual bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public virtual void OnNavigatedFrom(NavigationContext navigationContext)
        { }

        public virtual void OnNavigatedTo(NavigationContext navigationContext)
        { }

        public virtual void OnViewRemoved()
        { }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool dispose)
        {
            GC.SuppressFinalize(this);
        }
    }
}
