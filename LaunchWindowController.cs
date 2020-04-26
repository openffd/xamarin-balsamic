using AppKit;
using static AppKit.NSBackingStore;
using static AppKit.NSWindowStyle;
using CoreGraphics;
using Foundation;
using System;

namespace Balsamic
{
    sealed class LaunchWindowController : NSWindowController, AppDelegate.IWindowController
    {
        internal new LaunchWindow Window => (LaunchWindow)base.Window;
        
        public LaunchWindowController(IntPtr handle) : base(handle) {}

        [Export("initWithCoder:")]
        public LaunchWindowController(NSCoder coder) : base(coder) {}

        public LaunchWindowController() : base()
        {
            base.Window = new LaunchWindow(CGRect.Empty, Borderless | Closable | Miniaturizable | Titled, Buffered, false)
            {
                BackgroundColor = NSColor.FromWhite((nfloat)0.125, (nfloat)0.98),
                ContentViewController = (NSViewController)Balsamic.Storyboard.Welcome.InstantiateInitialController(),
                
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
