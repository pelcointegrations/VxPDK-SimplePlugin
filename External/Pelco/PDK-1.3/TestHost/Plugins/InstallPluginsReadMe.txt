Installing a Plugin

To install a plugin, the dll or exe containing the class which inherits from IPlugin must be copied to a directory beneath the “Plugins” directory for the Ops Center Client.  The location pointing to the installation directory for the Ops Center Client will be stored in a registry key. 
For now this registry key points here. (ProgramData\Pelco\OpsCenter\Plugins).  Be sure to get the appropriate path from the registry key in case it changes in the future.

For ANYCPU and 64bit installers the key can be found here…
HKLM\Software\WOW6432\Pelco\OpsCenter
	Key=InstallDir   Value=The Installation Directory Type=String

For 32bit installers look here...
	HKLM\Software\Pelco\OpsCenter
	Key=InstallDir   Value=The Installation Directory Type=String

Thus the plugins can be found under <<InstallDir>>\Plugins\. It is recommended that each company create their own subfolder and folder for each plugin.
	e.g. <<InstallDir>>\Plugins\CompanyName\PluginName\PluginAssembly.dll(exe)
