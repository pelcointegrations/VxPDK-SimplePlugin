// Copyright (c) 2014 Pelco
// Copyright (c) 2013 Ivan Krivyakov
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
using System;
using System.Windows;
using System.Drawing;
using System.Windows.Controls;
using System.Collections.Generic;
using Pelco.Phoenix.PluginHostInterfaces;

namespace ExampleDllPlugin
{
    /// <summary>
    /// Interaction logic for MainUserControl.xaml
    /// </summary>
    public partial class MainUserControl : UserControl
    {
        public MainUserControl()
        {
            InitializeComponent();
        }

        #region IHost
        private void reportFatalErrorBtn_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;
            pm.HostReportFatalError("Testing ReportFatalError", "FullExceptionText goes here");
        }

        private void requestCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;
            pm.HostRequestClose();
        }

        private void requestFullScreeBtn_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;
            pm.HostRequestFullScreen();
        }

        private void returnFromFullScreenBtn_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;
            pm.HostReturnFromFullScreen();
        }
        #endregion

        #region IOCCHost1
        private void initiateStreamsBtn_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;

            List<string> cameraList = new List<string>();
            cameraList.Add("2231415a-456f-905a-166e"); // Pass a real value.
            cameraList.Add("3451415a-456f-905a-45dg"); // Pass a real value.
            pm.Host1InitiateStreams(cameraList);
        }

        private bool _wantThumbnailPreference = false;
        private void registerForThumbnailPreferenceBtn_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;

            _wantThumbnailPreference = !_wantThumbnailPreference;
            pm.Host1RegisterForThumbnailPreference(_wantThumbnailPreference);
        }

        private bool _wantOnScreenNotifications = false;
        private void registerForOnScreenNotificationsBtn_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;

            _wantOnScreenNotifications = !_wantOnScreenNotifications;
            pm.Host1RegisterForOnScreenNotifications(_wantOnScreenNotifications);
        }

        private void StoreCredentialsBtn_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;

            // Encrypt the credentials before sending.
            pm.Host1StoreCredentials("un:JohnSmith, pw:Secret");
        }
        #endregion

        #region IOCCHostJoystick
        private bool _wantJoystick = false;
        private void registerForJoystickNotificationsBtn_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;

            _wantJoystick = !_wantJoystick;
            pm.HostJoystickRegisterForJoystickNotifications(_wantJoystick);
        }
        #endregion

    }
}
