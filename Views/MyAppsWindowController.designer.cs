// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Balsamic.Views
{
	[Register ("MyAppsWindowController")]
	partial class MyAppsWindowController
	{
		[Outlet]
		AppKit.NSToolbar Toolbar { get; set; }

		[Action ("ToggleSidebar:")]
		partial void ToggleSidebar (AppKit.NSSegmentedControl sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (Toolbar != null) {
				Toolbar.Dispose ();
				Toolbar = null;
			}
		}
	}
}
