using AppKit;
using Foundation;

namespace Balsamic.Views
{
    public partial class TwoFactorAuthWindowController : NSWindowController
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

        partial void Exit(NSButton _)
        {
            Window.SheetParent.EndSheet(Window, NSModalResponse.Cancel);
        }
    }
}
