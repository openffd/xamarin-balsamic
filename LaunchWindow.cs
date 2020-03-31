using AppKit;
using Foundation;

namespace Balsamic
{
    public class LaunchWindow : NSWindow
    {
        public LaunchWindow(CoreGraphics.CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation) :
            base(contentRect, aStyle, bufferingType, deferCreation)
        {
            IsOpaque = true;
            MovableByWindowBackground = false;
            ReleasedWhenClosed = false;
            Title = NSBundle.MainBundle.Name();
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }
    }
}
