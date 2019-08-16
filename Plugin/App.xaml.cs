using NLog;
using PluginNs.Models;
using PluginNs.Utilities;
using System;
using System.Windows;
using System.Windows.Threading;

namespace PluginNs
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!Utils.I.IsConfigSet())
            {
                var ex = new Exception("You must first setup the values in Utilities/Const.cs");
                Log.Error(ex);
                throw ex;
            }

            Utils.I.MainDispatcher = Dispatcher.CurrentDispatcher;

            var bootstrapper = new Bootstrapper();
            bootstrapper.Run();

            Utils.I.SetCacheItem(nameof(PersistentModel), new PersistentModel());
        }
    }
}
