using AppKit;
using Foundation;

namespace Balsamic
{
    public class LaunchWindow : NSWindow
    {
        public LaunchWindow(CoreGraphics.CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation) :
            base(contentRect, aStyle, bufferingType, deferCreation)
        {
            Appearance = NSAppearance.GetAppearance(NSAppearance.NameVibrantDark);
            IsOpaque = true;
            MovableByWindowBackground = true;
            ReleasedWhenClosed = false;
            Title = NSBundle.MainBundle.Name();
            TitlebarAppearsTransparent = true;
            TitleVisibility = NSWindowTitleVisibility.HiddenWhenActive;
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }
    }
}
