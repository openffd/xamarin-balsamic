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
	[Register ("TwoFactorAuthWindowController")]
	partial class TwoFactorAuthWindowController
	{
		[Outlet]
		AppKit.NSButton CloseButton { get; set; }

		[Action ("Exit:")]
		partial void Exit (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CloseButton != null) {
				CloseButton.Dispose ();
				CloseButton = null;
			}
		}
	}
}
