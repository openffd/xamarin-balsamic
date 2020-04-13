using AppKit;
using Foundation;
using System;

namespace Balsamic.Views
{
    public partial class MyAppsWindowController : NSWindowController
    {
        public MyAppsWindowController(IntPtr handle) : base(handle) {}

        [Export("initWithCoder:")]
        public MyAppsWindowController(NSCoder coder) : base(coder) {}

        public MyAppsWindowController() : base("MyAppsWindow") {}

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        internal new MyAppsWindow Window => (MyAppsWindow)base.Window;
    }
}
