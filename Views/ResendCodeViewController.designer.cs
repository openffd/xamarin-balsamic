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
	[Register ("ResendCodeViewController")]
	partial class ResendCodeViewController
	{
		[Outlet]
		AppKit.NSBox MoreOptionsBox { get; set; }

		[Outlet]
		AppKit.NSTextField MoreOptionsDescriptionTextField { get; set; }

		[Outlet]
		AppKit.NSTextField MoreOptionsHeaderTextField { get; set; }

		[Outlet]
		AppKit.NSImageView MoreOptionsImageView { get; set; }

		[Outlet]
		AppKit.NSBox ResendCodeBox { get; set; }

		[Outlet]
		AppKit.NSTextField ResendCodeDescriptionTextField { get; set; }

		[Outlet]
		AppKit.NSTextField ResendCodeHeaderTextField { get; set; }

		[Outlet]
		AppKit.NSImageView ResendCodeImageView { get; set; }

		[Outlet]
		AppKit.NSBox UsePhoneNumberBox { get; set; }

		[Outlet]
		AppKit.NSTextField UsePhoneNumberDescriptionTextField { get; set; }

		[Outlet]
		AppKit.NSTextField UsePhoneNumberHeaderTextField { get; set; }

		[Outlet]
		AppKit.NSImageView UsePhoneNumberImageView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ResendCodeBox != null) {
				ResendCodeBox.Dispose ();
				ResendCodeBox = null;
			}

			if (ResendCodeImageView != null) {
				ResendCodeImageView.Dispose ();
				ResendCodeImageView = null;
			}

			if (ResendCodeHeaderTextField != null) {
				ResendCodeHeaderTextField.Dispose ();
				ResendCodeHeaderTextField = null;
			}

			if (ResendCodeDescriptionTextField != null) {
				ResendCodeDescriptionTextField.Dispose ();
				ResendCodeDescriptionTextField = null;
			}

			if (UsePhoneNumberBox != null) {
				UsePhoneNumberBox.Dispose ();
				UsePhoneNumberBox = null;
			}

			if (UsePhoneNumberImageView != null) {
				UsePhoneNumberImageView.Dispose ();
				UsePhoneNumberImageView = null;
			}

			if (UsePhoneNumberHeaderTextField != null) {
				UsePhoneNumberHeaderTextField.Dispose ();
				UsePhoneNumberHeaderTextField = null;
			}

			if (UsePhoneNumberDescriptionTextField != null) {
				UsePhoneNumberDescriptionTextField.Dispose ();
				UsePhoneNumberDescriptionTextField = null;
			}

			if (MoreOptionsBox != null) {
				MoreOptionsBox.Dispose ();
				MoreOptionsBox = null;
			}

			if (MoreOptionsImageView != null) {
				MoreOptionsImageView.Dispose ();
				MoreOptionsImageView = null;
			}

			if (MoreOptionsHeaderTextField != null) {
				MoreOptionsHeaderTextField.Dispose ();
				MoreOptionsHeaderTextField = null;
			}

			if (MoreOptionsDescriptionTextField != null) {
				MoreOptionsDescriptionTextField.Dispose ();
				MoreOptionsDescriptionTextField = null;
			}
		}
	}
}
