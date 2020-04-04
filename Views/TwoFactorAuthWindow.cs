using System;

using Foundation;
using AppKit;

namespace Balsamic.Views
{
    public partial class TwoFactorAuthWindow : NSWindow
    {
        public TwoFactorAuthWindow(IntPtr handle) : base(handle)
        {
        }

        [Export("initWithCoder:")]
        public TwoFactorAuthWindow(NSCoder coder) : base(coder)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }
    }
}
