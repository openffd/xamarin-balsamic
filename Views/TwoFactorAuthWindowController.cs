using AppKit;
using Foundation;

namespace Balsamic.Views
{
    public partial class TwoFactorAuthWindowController : NSWindowController, INSTextFieldDelegate, INSControlTextEditingDelegate
    {
        public TwoFactorAuthWindowController(System.IntPtr handle) : base(handle) {}

        [Export("initWithCoder:")]
        public TwoFactorAuthWindowController(NSCoder coder) : base(coder) {}

        public TwoFactorAuthWindowController() : base("TwoFactorAuthWindow") {}

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            CodePart1TextField.Delegate = this;
            CodePart2TextField.Delegate = this;
            CodePart3TextField.Delegate = this;
            CodePart4TextField.Delegate = this;
            CodePart5TextField.Delegate = this;
            CodePart6TextField.Delegate = this;
        }

        public new TwoFactorAuthWindow Window => (TwoFactorAuthWindow)base.Window;

        partial void Cancel(NSButton _)
        {
            Window.SheetParent.EndSheet(Window, NSModalResponse.Cancel);
        }

        partial void Continue(NSButton _)
        {
            System.Console.WriteLine("ContinueButton");
        }

        partial void ResendCode(NSButton _)
        {
            System.Console.WriteLine("ResendCodeButton");
        }

        [Export("controlTextDidChange:")]
        public void ControlTextDidChange(NSNotification notification)
        {
            var textField = (NSTextField)notification.Object;
            if (textField.StringValue.Length == 0)
            {
                Window.SelectKeyViewPrecedingView(textField);
                return;
            }

            Window.SelectKeyViewFollowingView(textField);  
        }
    }
}
