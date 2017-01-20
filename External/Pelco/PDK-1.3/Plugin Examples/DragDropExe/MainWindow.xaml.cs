﻿using System;
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

namespace DragDropPlugin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PluginModel _pluginModel = null;

        public MainWindow()
        {
            InitializeComponent();
            _pluginModel = new PluginModel(null);
            _pluginModel.MainUserControl = MainControl;  // SInce we are running stand alone, PluginCreateControl will not get called.
            MainControl.DataContext = _pluginModel;
            MainControl.Loaded += _pluginModel.MainUserControl_Loaded;
        }
    }
}
