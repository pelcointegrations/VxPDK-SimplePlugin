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
using System.Diagnostics;

namespace OverlayExePlugin
{
    /// <summary>
    /// Interaction logic for MainUserControl.xaml
    /// </summary>
    public partial class MainUserControl : UserControl
    {
        public MainUserControl()
        {
            InitializeComponent();
            buttonGrid.Visibility = Visibility.Visible;
            anchorGrid.Visibility = Visibility.Collapsed;
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

        #region IOCCHostOverlay
        private bool _wantVideoCellNotifications = false;
        private void registerForVideoCellNotificationsBtn_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;

            _wantVideoCellNotifications = !_wantVideoCellNotifications;
            pm.HostOverlayRegisterForVideoCellNotifications(_wantVideoCellNotifications);
        }

        private void setOverlayAnchorBtn_Click(object sender, RoutedEventArgs e)
        {
            anchorGrid.Visibility = Visibility.Visible;
        }

        private void submitAnchorBtn_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;

            int anchoredTo = anchorCombo.SelectedIndex;  // Note: 0=inFront, 1 = left, 2 = top, 3 = right, 4 = bottom
            double percent = 0.0;
            int min = 0;
            int max = 0;

            if (anchoredTo != -1)
            {
                AnchorTypes atype = AnchorTypes.inFront;
                if (anchoredTo == 0)
                {
                    anchorGrid.Visibility = Visibility.Collapsed;
                    pm.HostOverlaySetOverlayAnchor(atype, 0.0, 0, 0);
                    return;
                }
                else
                {
                    switch (anchoredTo)
                    {
                        case 0:
                            atype = AnchorTypes.inFront;
                            break;
                        case 1:
                            atype = AnchorTypes.left;
                            break;
                        case 2:
                            atype = AnchorTypes.top;
                            break;
                        case 3:
                            atype = AnchorTypes.right;
                            break;
                        case 4:
                            atype = AnchorTypes.bottom;
                            break;
                    }
                    string tmp = percentageText.Text;
                    if (double.TryParse(tmp, out percent))
                    {
                        tmp = mininumText.Text;
                        if (int.TryParse(tmp, out min))
                        {
                            tmp = maximumText.Text;
                            if (int.TryParse(tmp, out max))
                            {
                                if ((percent >= 1 && percent <= 99) && (min > 0) && (max >= min))
                                {
                                    anchorGrid.Visibility = Visibility.Collapsed;
                                    pm.HostOverlaySetOverlayAnchor(atype, percent, min, max);
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            string invalidParameters;
            invalidParameters = string.Format("AnchoredTo={0} Must be 0-4,  percentage={1} Must be 1-99,  min={2} Must be > 0, max={3} Must be > min", anchoredTo, percent, min, max);
            pm.TraceEvent(TraceEventType.Information, 0, "submitAnchorBtn_Click: " + invalidParameters);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            anchorGrid.Visibility = Visibility.Collapsed;
        }
        #endregion

        #region IOCCHostSetVideoPosition
        private void SetVideoPositionBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;

            DateTime dt = DateTime.UtcNow;
            TimeSpan tenMinutes = new TimeSpan(0, 10, 0);
            dt = dt.Subtract(tenMinutes);
            pm.HostSVPSetVideoPosition(dt); // Jump back to 10 minutes before now.
        }
        #endregion

    }
}


