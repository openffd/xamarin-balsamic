// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Balsamic.Views.MyApps.LeadingContentListViewControllerOutlineView
{
	[Register ("ApplicationDetailTableCellView")]
	partial class ApplicationDetailTableCellView
	{
		[Outlet]
		internal AppKit.NSTextField AppNameLabelTextField { get; set; }

		[Outlet]
		internal AppKit.NSTextField BundleIdentifierLabelTextField { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AppNameLabelTextField != null) {
				AppNameLabelTextField.Dispose ();
				AppNameLabelTextField = null;
			}

			if (BundleIdentifierLabelTextField != null) {
				BundleIdentifierLabelTextField.Dispose ();
				BundleIdentifierLabelTextField = null;
			}
		}
	}
}
