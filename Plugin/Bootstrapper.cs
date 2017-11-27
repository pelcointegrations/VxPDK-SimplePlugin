using Microsoft.Practices.Unity;
using PluginNs.Behaviors;
using PluginNs.Services.Host;
using PluginNs.Services.Logging;
using PluginNs.Services.Serenity;
using PluginNs.Utilities;
using PluginNs.Views;
using Prism.Logging;
using Prism.Regions;
using Prism.Unity;
using System;
using System.IO;
using System.Windows;

namespace PluginNs
{
    class Bootstrapper : UnityBootstrapper
    {
        private ILogger _logger;

        public Bootstrapper(bool isPlugin = false)
        {
            Utils.Instance.IsPlugin = isPlugin;
        }

        public bool HasRun
        {
            get
            {
                return Container != null;
            }
        }

        protected override ILoggerFacade CreateLogger()
        {
            TextWriter textWriter = null;
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                var fs = File.Open(Const.LogFilePath, FileMode.Append, FileAccess.Write, FileShare.Write);
                var streamWriter = new StreamWriter(fs);
                streamWriter.AutoFlush = true;
                textWriter = streamWriter;
            }
            else
            {
                textWriter = new TraceWriter();
            }
            _logger = new Logger(textWriter);
            return _logger;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            Container.RegisterInstance<ILogger>(_logger);
            Container.RegisterType<ISerenity, Serenity>(new ContainerControlledLifetimeManager());

            if (!System.Diagnostics.Debugger.IsAttached)
            {
                Container.RegisterType<IPluginHost, PluginHost>(new ContainerControlledLifetimeManager());
            }
            else
            {
                Container.RegisterType<IPluginHost, PluginHostMock>(new ContainerControlledLifetimeManager());
            }

            Container.RegisterType<object, MainView>(nameof(MainView));
            Container.RegisterType<object, DefaultView>(nameof(DefaultView));
            Container.RegisterType<object, SettingsView>(nameof(SettingsView));

            Utils.Instance.SetCacheItem(Const.ILogger, _logger);
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
            if (Utils.Instance.IsPlugin)
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
