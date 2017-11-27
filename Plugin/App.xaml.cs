using PluginNs.Models;
using PluginNs.Utilities;
using System.Windows;
using System.Windows.Threading;

namespace PluginNs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Utils.Instance.MainDispatcher = Dispatcher.CurrentDispatcher;
            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();
            Utils.Instance.SetCacheItem(Const.PersistentModel, new PersistentModel());
        }
    }
}
