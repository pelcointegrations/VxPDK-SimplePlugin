using NLog;
using PluginNs.Behaviors;
using PluginNs.Services.PluginHost;
using PluginNs.Services.VxSdk;
using PluginNs.Views;
using Prism.Ioc;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Windows;
using Unity;

namespace PluginNs
{
    class Bootstrapper : PrismBootstrapper
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public bool HasRun => Container != null;

        private bool _isPlugin;

        public Bootstrapper(bool isPlugin = false)
        {
            _isPlugin = isPlugin;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
        }

        public new void Run()
        {
            if (!HasRun)
                base.Run();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IVxSdkSvc, VxSdkSvc>();

            if (_isPlugin)
                containerRegistry.RegisterSingleton<IPluginHostSvc, PluginHostSvc>();
            else
                containerRegistry.RegisterSingleton<IPluginHostSvc, PluginHostSvcMock>();

            containerRegistry.RegisterForNavigation<MainView>(nameof(MainView));
            containerRegistry.RegisterForNavigation<DefaultView>(nameof(DefaultView));
            containerRegistry.RegisterForNavigation<SettingsView>(nameof(SettingsView));
        }

        protected override DependencyObject CreateShell()
        {
            DependencyObject shell = null;
            LoadResources();
            if (!_isPlugin)
                shell = Container.Resolve<MainWindow>();
            return shell;
        }

        // Gets called if CreateShell returns a non null object
        protected override void InitializeShell(DependencyObject shell)
        {
            base.InitializeShell(shell);
            App.Current.MainWindow = (Window)shell;
            App.Current.MainWindow.Show();
        }

        protected override void ConfigureDefaultRegionBehaviors(IRegionBehaviorFactory regionBehaviors)
        {
            base.ConfigureDefaultRegionBehaviors(regionBehaviors);
            regionBehaviors.AddIfMissing(nameof(DisposeRemovedViews), typeof(DisposeRemovedViews));
        }

        // This is required instead of using App.xaml because when loaded as a plug-in
        // App.xaml(.cs) is not ran and merged dictionaries are not loaded.
        private void LoadResources()
        {
            var resourceDicts = new string[]
            {
                "/Plugin;component/Resources/Constants.xaml",
                "/Plugin;component/Resources/Colors.xaml",
                "/Plugin;component/Resources/UXGGlobalStyles.xaml",
                "/Plugin;component/Resources/GlobalStyles.xaml",
                "/Plugin;component/Resources/PluginMasterStyles.xaml",
                "/Plugin;component/Resources/DateTimeUpDownStyles.xaml"
            };

            foreach (var resourceDict in resourceDicts)
            {
                var resource = new ResourceDictionary();
                resource.Source = new Uri(resourceDict, UriKind.Relative);
                App.Current.Resources.MergedDictionaries.Add(resource);
            }
        }

        private void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ee)
                Log.Error(ee, $"Threw an unhandled exception! - Message - {ee.Message}");
            else if (e.ExceptionObject is string es)
                Log.Error($"Threw an unhandled exception! - String - {es}");
            else if (e.ExceptionObject != null)
                Log.Error($"Threw an unhandled exception! - Type - {e.ExceptionObject.GetType()}");
            else if (sender != null)
                Log.Error($"Threw an unhandled exception - Sender - {sender.GetType()}");
            else
                Log.Error("Threw an unhandled exception! - No more info available.");
        }
    }
}
