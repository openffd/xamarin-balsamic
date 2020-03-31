using Foundation;
using AppKit;

namespace Balsamic
{
    public class LaunchWindowController : NSWindowController, IWindowController
    {
        public LaunchWindowController(System.IntPtr handle) : base(handle) {}

        [Export("initWithCoder:")]
        public LaunchWindowController(NSCoder coder) : base(coder) {}

        public LaunchWindowController() : base()
        {
            base.Window = new LaunchWindow(
                new CoreGraphics.CGRect(0, 0, 800, 400),
                NSWindowStyle.Borderless | NSWindowStyle.Closable | NSWindowStyle.Miniaturizable | NSWindowStyle.Titled,
                NSBackingStore.Buffered,
                false
            );
            Window.AwakeFromNib();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public new LaunchWindow Window
        {
            get { return (LaunchWindow)base.Window; }
        }

        public void ShowWindow()
        {
            Window.Center();
            Window.MakeKeyAndOrderFront(this);
        }
    }
}
