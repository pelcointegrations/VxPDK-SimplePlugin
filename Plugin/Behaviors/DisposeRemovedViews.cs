using Prism.Regions;
using System;
using System.Collections.Specialized;
using System.Windows;

namespace PluginNs.Behaviors
{
    class DisposeRemovedViews : RegionBehavior
    {
        protected override void OnAttach()
        {
            Region.Views.CollectionChanged += Views_CollectionChanged;
        }

        private void Views_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Remove) return;

            foreach (var removedView in e.OldItems)
            {
                var frameworkElementView = removedView as FrameworkElement;
                IDisposable disposableViewModel = frameworkElementView?.DataContext as IDisposable;

                (removedView as IDisposable)?.Dispose();
                disposableViewModel?.Dispose();
            }
        }
    }
}
