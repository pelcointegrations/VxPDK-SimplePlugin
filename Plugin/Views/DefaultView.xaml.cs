using PluginNs.ViewModels;
using System.Windows.Controls;

namespace PluginNs.Views
{
    /// <summary>
    /// Interaction logic for DefaultView.xaml
    /// </summary>
    partial class DefaultView : UserControl
    {
        public DefaultView(DefaultViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
