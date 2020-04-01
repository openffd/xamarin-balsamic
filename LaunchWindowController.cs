using Foundation;
using AppKit;

namespace Balsamic
{
    public class LaunchWindowController : NSWindowController, IWindowController
    {
        private readonly NSStoryboard storyboard = NSStoryboard.FromName("Welcome", null);
        
        public LaunchWindowController(System.IntPtr handle) : base(handle) {}

        [Export("initWithCoder:")]
        public LaunchWindowController(NSCoder coder) : base(coder) {}

        public LaunchWindowController() : base()
        {
            CoreGraphics.CGSize size = new CoreGraphics.CGSize(640, 400);
            base.Window = new LaunchWindow(
                CoreGraphics.CGRect.Empty,
                NSWindowStyle.Borderless | NSWindowStyle.Closable | NSWindowStyle.Miniaturizable,
                NSBackingStore.Buffered,
                false)
            {
                ContentViewController = (NSViewController)storyboard.InstantiateInitialController(),
                MaxSize = size
            };
            Window.AwakeFromNib();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public new LaunchWindow Window => (LaunchWindow)base.Window;

        public void ShowWindow()
        {
            Window.Center();
            Window.MakeKeyAndOrderFront(this);
        }
    }
}
