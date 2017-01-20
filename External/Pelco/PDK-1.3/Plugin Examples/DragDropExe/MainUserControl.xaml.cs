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
using System.Windows.Input;
using System.Reflection;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Diagnostics;

namespace DragDropPlugin
{
    /// <summary>
    /// Interaction logic for MainUserControl.xaml
    /// </summary>
    public partial class MainUserControl : UserControl
    {
        IList<LaunchContent> leftContentList;
        IList<LaunchContent> rightContentList;

        public MainUserControl()
        {
            InitializeComponent();

            LaunchContent launchContent = new LaunchOverlayPlugin()
            {
                DataSourceId = "uuid:left_content:video",
                PluginId = "plugin1-id-here",
                PluginKey = "plugin1-key-here",
                PluginArgs = "arg1 arg2",
                StartTimeUtc = DateTime.UtcNow - TimeSpan.FromHours(1), // playback 1 hour prior to now.
                IsPlaying = false, 
                LoopDuration = TimeSpan.FromSeconds(10),
            };
            leftBlock.Text = "LaunchOverlayPlugin";
            leftContentList = new List<LaunchContent>();
            leftContentList.Add(launchContent);


            launchContent = new LaunchDataSource()
            {
                DataSourceId = "uuid:right_content:video",
                StartTimeUtc = null, // live playback
                IsPlaying = true,
                LoopDuration = null,
            };
            rightBlock.Text = "LaunchDataSource";
            rightContentList = new List<LaunchContent>();
            rightContentList.Add(launchContent);
        }

        #region FrameWidgets
        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;
        }

        private void Next_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;

            pm.HostRequestClose();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            PluginModel pm = this.DataContext as PluginModel;
            if (pm == null) return;
        }
        #endregion

        #region DragDrop
        private void ellipse_MouseMove(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            if (ellipse != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(ellipse,
                    (ellipse == leftEllipse) ? leftContentList : rightContentList,
                                     DragDropEffects.Copy);
            }
        }

        private void ellipse_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            // Nothing to handle at this time.
        }

        private string _previousLeft;
        private string _previousRight;
        private void ellipse_DragEnter(object sender, DragEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            if (ellipse != null)
            {
                // Save the current text so we can revert back when we leave.
                if (sender == leftEllipse)
                    _previousLeft = leftBlock.Text;
                else
                    _previousRight = rightBlock.Text;

                if (LaunchContentHelpers.IsValidLaunchContent(e))
                {
                    IList<LaunchContent> contentList = LaunchContentHelpers.GetValidLaunchContent(e);
                    if (contentList != null)
                    {
                        if (sender == leftEllipse)

                            leftBlock.Text = GetDescriptionOfContent(contentList);
                        else
                            rightBlock.Text = GetDescriptionOfContent(contentList);
                    }
                }
            }
        }

        private string GetDescriptionOfContent(IList<LaunchContent> contentList)
        {
            string text = "";
            if (contentList != null)
            {
                if (contentList.Count > 0)
                {
                    foreach (var content in contentList)
                    {
                        if (content is LaunchContentPlugin)
                        {
                            text += "LaunchContentPlugin\n";
                        }
                        // Note: If content is LaunchOverlayPlugin it will be castable to LaunchDataSource but not vice versa.
                        // If doing checks like this to determine the type, check Overlay first.
                        else if (content is LaunchOverlayPlugin)
                        {
                            text += "LaunchOverlayPlugin\n";
                        }
                        else if (content is LaunchDataSource)
                        {
                            text += "LaunchDataSource\n";
                        }
                        else
                        {
                            text += "Unrecognized Launch Content\n";
                        }
                    }
                }
                else
                {
                    text = "empty contentList";
                }
            }
            else
            {
                text = "null contentList";
            }
            return text;
        }

        private void ellipse_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;

            if (LaunchContentHelpers.IsValidLaunchContent(e))
            {
                e.Effects = DragDropEffects.Copy;
            }
        }

        private void ellipse_DragLeave(object sender, DragEventArgs e)
        {
            Ellipse ellipse = sender as Ellipse;
            if (ellipse != null)
            {
                if (LaunchContentHelpers.IsValidLaunchContent(e))
                {
                    if (sender == leftEllipse)
                        leftBlock.Text = _previousLeft;
                    else
                        rightBlock.Text = _previousRight;
                }
            }
        }

        private void ellipse_Drop(object sender, DragEventArgs e)
        {
            //Debugger.Break();

            Ellipse ellipse = sender as Ellipse;
            if (ellipse != null)
            {
                if (LaunchContentHelpers.IsValidLaunchContent(e))
                {
                    IList<LaunchContent> contentList = LaunchContentHelpers.GetValidLaunchContent(e);
                    if (contentList != null)
                    {
                        if (sender == leftEllipse)
                        {
                            leftBlock.Text = GetDescriptionOfContent(contentList);
                            leftContentList = contentList;
                        }
                        else
                        {
                            rightBlock.Text = GetDescriptionOfContent(contentList);
                            rightContentList = contentList;
                        }
                    }
                }
            }
        }
        #endregion

    }
}
