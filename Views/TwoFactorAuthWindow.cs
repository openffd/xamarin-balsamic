using Foundation;
using AppKit;

namespace Balsamic.Views
{
    public partial class TwoFactorAuthWindow : NSWindow
    {
        public TwoFactorAuthWindow(System.IntPtr handle) : base(handle) {}

        [Export("initWithCoder:")]
        public TwoFactorAuthWindow(NSCoder coder) : base(coder) {}

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public override bool CanBecomeKeyWindow => true;
    }
}
