# PowerShellIseFindAllPlugin
Implementation of a "Find All" plugin for the PowerShell ISE

We frequently get questions asking if the PowerShell ISE supports a feature that it doesn’t. For example,
variable watch windows, function browsers, or “find all matches in the current document”.

Or as another example, many of you are very familiar with Visual Studio and naturally wish for
Visual Studio feature <x>. That <x> is usually different for each person :) The Visual Studio team is
many times larger than the PowerShell team, and they’ve had a 15-year head start.

When preparing for PowerShell V3, we realized that the appetite for new functionality in the ISE would far
outstrip our ability to create it, so we designed one of the most (until now) un-heralded features in
the PowerShell ISE: the ability to create custom tool windows.

Creating a PowerShell ISE Add-In / Plugin is very straight-forward. Jason gives a great overview in his
blog: http://blog.codeassassin.com/2012/01/16/create-a-powershell-v3-ise-add-on-tool/.

The design center of ISE plugins are that they are really just WPF UserControls. In addition, they implement the IAddOnToolHostObject interface.

When you call the .Add() method on either $psISE.CurrentPowerShellTab.VerticalAddOnTools or
$psISE.CurrentPowerShellTab.HorizontalAddOnTools, the ISE will create your UserControl, set the HostObject property to an
object that represents the ISE’s object model (almost identical to the $psISE object model variable you may be used to from
within the ISE), and then add your control to the appropriate pane.

After that, the control is all yours.

You have full access to WPF, XAML, C#, and anything else you can imagine or would expect from a WPF control. This repository
contains the implementation of a very simple "Find All" plugin for the ISE.

For more information, see: http://www.leeholmes.com/blog/2013/04/04/creating-add-ons-plugins-and-tools-for-the-powershell-ise/