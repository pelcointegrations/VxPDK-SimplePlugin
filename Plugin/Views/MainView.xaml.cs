using PluginNs.ViewModels;
using Prism.Regions;
using System.Windows.Controls;

namespace PluginNs.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    partial class MainView : UserControl
    {
        public MainView(MainViewModel viewModel, IRegionManager regionMgr)
        {
            InitializeComponent();
            DataContext = viewModel;
            RegionManager.SetRegionManager(this, regionMgr);
        }
    }
}
