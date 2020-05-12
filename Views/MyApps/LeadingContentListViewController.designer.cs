// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Balsamic.Views.MyApps
{
	[Register ("LeadingContentListViewController")]
	partial class LeadingContentListViewController
	{
		[Outlet]
		AppKit.NSOutlineView OutlineView { get; set; }

		[Outlet]
		AppKit.NSTreeController TreeController { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TreeController != null) {
				TreeController.Dispose ();
				TreeController = null;
			}

			if (OutlineView != null) {
				OutlineView.Dispose ();
				OutlineView = null;
			}
		}
	}
}
