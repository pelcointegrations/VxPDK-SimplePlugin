using PluginNs.Models;
using PluginNs.Services.PluginHost;
using PluginNs.Utilities;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using Unity;

namespace PluginNs.ViewModels
{
    class BaseViewModel : BindableBase, INavigationAware, IDisposable
    {
        [Dependency]
        public IPluginHostSvc PluginHost { get; set; }

        [Dependency]
        public IRegionManager RegionMgr { get; set; }

        [Dependency]
        public IEventAggregator Aggregator { get; set; }

        public PersistentModel Settings { get; }

        public BaseViewModel()
        {
            Settings = Utils.I.GetCacheItem<PersistentModel>(nameof(PersistentModel));
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
