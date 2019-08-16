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
    class Bootstrapper : UnityBootstrapper
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public bool HasRun => Container != null;

        private bool _isPlugin;

        public Bootstrapper(bool isPlugin = false)
        {
            _isPlugin = isPlugin;
        }

        public new void Run()
        {
            if (!HasRun)
                base.Run();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterSingleton<IVxSdkSvc, VxSdkSvc>();

            if (_isPlugin)
                Container.RegisterSingleton<IPluginHostSvc, PluginHostSvc>();
            else
                Container.RegisterSingleton<IPluginHostSvc, PluginHostSvcMock>();

            Container.RegisterTypeForNavigation<MainView>(nameof(MainView));
            Container.RegisterTypeForNavigation<DefaultView>(nameof(DefaultView));
            Container.RegisterTypeForNavigation<SettingsView>(nameof(SettingsView));
        }

        protected override IRegionBehaviorFactory ConfigureDefaultRegionBehaviors()
        {
            var factory = base.ConfigureDefaultRegionBehaviors();
            factory.AddIfMissing(nameof(DisposeRemovedViews), typeof(DisposeRemovedViews));
            return factory;
        }

        protected override DependencyObject CreateShell()
        {
            DependencyObject shell = null;
            if (_isPlugin)
                LoadResources();
            else
                shell = Container.Resolve<MainWindow>();
            return shell;
        }

        // Gets called if CreateShell returns a non null object
        protected override void InitializeShell()
        {
            base.InitializeShell();
            App.Current.MainWindow = (Window)Shell;
            App.Current.MainWindow.Show();
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
    }
}
