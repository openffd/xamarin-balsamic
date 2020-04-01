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
		AppKit.NSImageView AppIconImageView { get; set; }

		[Outlet]
		AppKit.NSTextField AppleIDTextField { get; set; }

		[Outlet]
		AppKit.NSButton ForgotPasswordButton { get; set; }

		[Outlet]
		AppKit.NSTextField PasswordTextField { get; set; }

		[Outlet]
		AppKit.NSButton SigninButton { get; set; }

		[Outlet]
		AppKit.NSTextField SigninHeaderTextField { get; set; }

		[Outlet]
		AppKit.NSTextField VersionTextField { get; set; }

		[Outlet]
		AppKit.NSTextField WelcomeTextField { get; set; }

		[Action ("Exit:")]
		partial void Exit (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AppIconImageView != null) {
				AppIconImageView.Dispose ();
				AppIconImageView = null;
			}

			if (VersionTextField != null) {
				VersionTextField.Dispose ();
				VersionTextField = null;
			}

			if (WelcomeTextField != null) {
				WelcomeTextField.Dispose ();
				WelcomeTextField = null;
			}

			if (SigninHeaderTextField != null) {
				SigninHeaderTextField.Dispose ();
				SigninHeaderTextField = null;
			}

			if (AppleIDTextField != null) {
				AppleIDTextField.Dispose ();
				AppleIDTextField = null;
			}

			if (PasswordTextField != null) {
				PasswordTextField.Dispose ();
				PasswordTextField = null;
			}

			if (SigninButton != null) {
				SigninButton.Dispose ();
				SigninButton = null;
			}

			if (ForgotPasswordButton != null) {
				ForgotPasswordButton.Dispose ();
				ForgotPasswordButton = null;
			}
		}
	}
}
