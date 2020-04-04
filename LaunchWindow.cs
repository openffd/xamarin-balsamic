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

        public override NSView ContentView
        {
            get => base.ContentView;
            set
            {
                value.WantsLayer = true;
                value.Layer.Frame = value.Frame;
                value.Layer.CornerRadius = 20;
                value.Layer.MasksToBounds = true;
                base.ContentView = value;
            }
        }

        public override bool CanBecomeKeyWindow => true;
    }
}
