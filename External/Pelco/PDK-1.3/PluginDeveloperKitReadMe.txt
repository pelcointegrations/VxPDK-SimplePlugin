To build and run the example plug-ins...

1) use VS2013 to open and build "Plugin Examples\Plugin Examples.sln"
2) Goto the properties --> Debug ---> Start External Program settings of your startup project and reference "..\TestHost\PluginHost.exe" as the executable to startup when debugging.
3) press start.

=============================
To write your own plugins...
=============================

Developing Plug-Ins

You can implement a plug-in as a class library (DLL) or an executable (EXE).  In the DLL scenario, the steps are as follows:
1) Create a new class library project
2) Reference the WPF assemblies PresentationCore, PresentationFramework, System.Xaml and WindowBase.
3) Add a reference to the PluginHostInterfaces assembly.  Make sure “copy local” is set to false.
4) Create a new WPF user control, such as MainUserControl.
5) Create a class named Plugin that derives from Pelco.Phoenix.PluginHostInterfaces.PluginBase.
6) Put the .dll file into the appointed plug-in discovery directory
7) Compile your plug-in and run the host.


Alternatively, a plug-in can be implemented as an executable.  In this case, the steps are:
1) Create a WPF application.
2) Create a WPF user control, for example, MainUserControl
3) Add MainUserControl to the application’s main window.
4) Add a reference to the PluginHostInterfaces assembly.  ** >>> Make sure “copy local” is set to false. <<< **
5) Create a class named Plugin that derives from Pelco.Phoenix.PluginHostInterfaces.PluginBase
6)Put the .exe file into the appointed plug-in discovery directory

Your plug-in class would look exactly like the preceding example, and your main window XAML should contain nothing but a reference to MainUserControl.

<Window x:Class=”MyPlugin.MainWindow”
   xmlns=”http://schemas.microsoft.com/winfx/2006/xaml/presentation”
   xmlns:x=”http://schemas.microsoft.com/winfx/2006/xaml”
   xmlns:local=”clr-namespace:MyProject”
   Title=”My Plugin” Height=”600” Width = “750”>
   <Grid>
      <local:MainUserControl />
   </Grid>
</Window>

A plug-in implemented like this can run as a standalone application or within the host.  This simplifies debugging plug-in code not related to host integration.

All Plug-Ins MUST implement IPlugin and they can call into the Ops Center with IHost.  The plug-in should check for all required host interfaces by calling host.GetService< Inteface > in it's plug-in constructor.   If a required interface is not available the plug-in should call host.ReportFatalError() inside the constructor.

Please Notice - Redistributables:
1) That the Plugin solution is set up to copy the <Plugin>.exe and <Plugin>.exe.config files to the "Plugins" directory under the PluginHost program.
2) That the Plugin solution DOES NOT copy the PluginInterfaces.dll file into the Plugins Directory.
     * The Plugin Host, like the Ops Center, already has its own copy of the PluginInterfaces.dll, so you DO NOT NEED to copy it over to the "Plugins" directory.
3) If you want to run an Example as a stand alone app it will need to be able to find a PluginInterfaces.dll file.

Please Notice - config file.
1) The <Plugin>.exe.config file is setup to write "Information" (ie verbose) level trace output to a text file that will be created where the .exe lives.
2) You do NOT want to ship with it that way.   Either omit the config file altogether or set its default to "Critical" so that it does not create a *HUGE* log file over time.
   * The config file is included by default as it is very useful for debugging.  Delete it before shipping.
   * Note: Adding the config file to an installation in the field will cause traces to be written again. It's a runtime (not compile time) behavior.