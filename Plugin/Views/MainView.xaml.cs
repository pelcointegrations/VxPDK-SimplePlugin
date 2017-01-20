using PluginNs.ViewModels;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
