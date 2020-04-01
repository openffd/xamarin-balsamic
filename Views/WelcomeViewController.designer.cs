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
	[Register ("WelcomeViewController")]
	partial class WelcomeViewController
	{
		[Outlet]
		AppKit.NSTextField VersionTextField { get; set; }

		[Outlet]
		AppKit.NSTextField WelcomeTextField { get; set; }

		[Action ("Exit:")]
		partial void Exit (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (WelcomeTextField != null) {
				WelcomeTextField.Dispose ();
				WelcomeTextField = null;
			}

			if (VersionTextField != null) {
				VersionTextField.Dispose ();
				VersionTextField = null;
			}
		}
	}
}
