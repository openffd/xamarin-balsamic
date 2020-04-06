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
		AppKit.NSButton CancelButton { get; set; }

		[Outlet]
		Balsamic.Views.SingleDigitTextField CodePart1TextField { get; set; }

		[Outlet]
		Balsamic.Views.SingleDigitTextField CodePart2TextField { get; set; }

		[Outlet]
		Balsamic.Views.SingleDigitTextField CodePart3TextField { get; set; }

		[Outlet]
		Balsamic.Views.SingleDigitTextField CodePart4TextField { get; set; }

		[Outlet]
		Balsamic.Views.SingleDigitTextField CodePart5TextField { get; set; }

		[Outlet]
		Balsamic.Views.SingleDigitTextField CodePart6TextField { get; set; }

		[Outlet]
		AppKit.NSButton ContinueButton { get; set; }

		[Outlet]
		AppKit.NSTextField InstructionTextField { get; set; }

		[Outlet]
		AppKit.NSButton ResendCodeButton { get; set; }

		[Outlet]
		AppKit.NSBox TwoFactorAuthBox { get; set; }

		[Action ("Cancel:")]
		partial void Cancel (AppKit.NSButton sender);

		[Action ("Continue:")]
		partial void Continue (AppKit.NSButton sender);

		[Action ("ResendCode:")]
		partial void ResendCode (AppKit.NSButton sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}

			if (CodePart1TextField != null) {
				CodePart1TextField.Dispose ();
				CodePart1TextField = null;
			}

			if (CodePart2TextField != null) {
				CodePart2TextField.Dispose ();
				CodePart2TextField = null;
			}

			if (CodePart3TextField != null) {
				CodePart3TextField.Dispose ();
				CodePart3TextField = null;
			}

			if (CodePart4TextField != null) {
				CodePart4TextField.Dispose ();
				CodePart4TextField = null;
			}

			if (CodePart5TextField != null) {
				CodePart5TextField.Dispose ();
				CodePart5TextField = null;
			}

			if (CodePart6TextField != null) {
				CodePart6TextField.Dispose ();
				CodePart6TextField = null;
			}

			if (ContinueButton != null) {
				ContinueButton.Dispose ();
				ContinueButton = null;
			}

			if (InstructionTextField != null) {
				InstructionTextField.Dispose ();
				InstructionTextField = null;
			}

			if (ResendCodeButton != null) {
				ResendCodeButton.Dispose ();
				ResendCodeButton = null;
			}

			if (TwoFactorAuthBox != null) {
				TwoFactorAuthBox.Dispose ();
				TwoFactorAuthBox = null;
			}
		}
	}
}
