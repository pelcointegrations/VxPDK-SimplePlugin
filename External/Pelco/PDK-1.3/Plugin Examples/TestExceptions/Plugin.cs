// Copyright (c) 2014 Pelco
// Copyright (c) 2013 Ivan Krivyakov
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
using System.Windows;
using Pelco.Phoenix.PluginHostInterfaces;

namespace TestExceptions
{
    public class Plugin : PluginBase
    {
        private IHost _host;
        private string _title = "TestExceptionsDLL-plugin.cs";

        public Plugin(IHost host)
        {
            _host = host;
        }

        public override FrameworkElement CreateControl()
        {
            // System.Diagnostics.Debugger.Break();
            return new MainUserControl();
        }

        public override string GetPluginKey()
        {
            return "TestExceptionKey";
        }

        public override string Name
        {
            get { return "TestExceptionsDLL"; }
        }

        public override string Description
        {
            get { return _title; }
        }

        public override string Version
        {
            get { return "1.0"; }
        }

        public override bool IsOverlay
        {
            get { return false; }
        }

        public override string PluginID
        {
            get { return "0"; }
        }

        public override void Shutdown()
        {
            _host.RequestClose();
        }

    }
}
