using AppKit;
using CoreGraphics;
using Foundation;

namespace Balsamic
{
    public class LaunchWindowController : NSWindowController, AppDelegate.IWindowController
    {
        public new LaunchWindow Window => (LaunchWindow)base.Window;

        private readonly NSStoryboard storyboard = NSStoryboard.FromName("Welcome", null);
        
        public LaunchWindowController(System.IntPtr handle) : base(handle) {}

        [Export("initWithCoder:")]
        public LaunchWindowController(NSCoder coder) : base(coder) {}

        public LaunchWindowController() : base()
        {
            base.Window = new LaunchWindow(CGRect.Empty, NSWindowStyle.Borderless, NSBackingStore.Buffered, false)
            {
                ContentViewController = (NSViewController)storyboard.InstantiateInitialController()
            };
            Window.AwakeFromNib();
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
