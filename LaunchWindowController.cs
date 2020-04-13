using AppKit;
using CoreGraphics;
using Foundation;

namespace Balsamic
{
    sealed class LaunchWindowController : NSWindowController, AppDelegate.IWindowController
    {
        internal new LaunchWindow Window => (LaunchWindow)base.Window;
        
        public LaunchWindowController(System.IntPtr handle) : base(handle) {}

        [Export("initWithCoder:")]
        public LaunchWindowController(NSCoder coder) : base(coder) {}

        public LaunchWindowController() : base()
        {
            base.Window = new LaunchWindow(CGRect.Empty, NSWindowStyle.Borderless, NSBackingStore.Buffered, false)
            {
                ContentViewController = (NSViewController)Balsamic.Storyboard.Welcome.InstantiateInitialController()
            };
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public void ShowWindow()
        {
            Window.Center();
            Window.MakeKeyAndOrderFront(this);
        }
    }
}
