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
            
        }
    }
}
