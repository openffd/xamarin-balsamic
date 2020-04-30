using AppKit;
using CoreGraphics;
using Foundation;
using System;

namespace Balsamic.Views
{
    sealed partial class MyAppsWindow : NSWindow
    {
        public MyAppsWindow(IntPtr handle) : base(handle) {}

        [Export("initWithCoder:")]
        public MyAppsWindow(NSCoder coder) : base(coder) {}

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            SetFrame(new CGRect(CGPoint.Empty, new CGSize(1350, 950)), true);
        }
    }
}
