using PluginNs.Views;
using Prism.Regions;
using System.Windows;
using Unity;

namespace PluginNs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IUnityContainer container, IRegionManager regionMgr)
        {
            InitializeComponent();
            regionMgr.RegisterViewWithRegion(nameof(MainWindow), () => container.Resolve<MainView>());
        }
    }
}
