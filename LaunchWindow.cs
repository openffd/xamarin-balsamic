﻿using AppKit;
using Foundation;

namespace Balsamic
{
    internal sealed class LaunchWindow : NSWindow
    {
        public LaunchWindow(CoreGraphics.CGRect contentRect, NSWindowStyle aStyle, NSBackingStore bufferingType, bool deferCreation) :
            base(contentRect, aStyle, bufferingType, deferCreation)
        {
            Appearance = NSAppearance.GetAppearance(NSAppearance.NameVibrantDark);
            IsOpaque = true;
            MovableByWindowBackground = true;
            ReleasedWhenClosed = false;
            Title = NSBundle.MainBundle.GetName();
            TitlebarAppearsTransparent = true;
            TitleVisibility = NSWindowTitleVisibility.Hidden;
            Toolbar = new NSToolbar() { ShowsBaselineSeparator = false };
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        internal new NSView ContentView => base.ContentView;

        public override bool CanBecomeKeyWindow => true;
    }
}
