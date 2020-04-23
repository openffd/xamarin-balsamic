using AppKit;
using static Balsamic.AppDelegate;
using static Balsamic.String.Notification;
using static Balsamic.String.Notification.ToggleCollapsed.UserInfoKey;
using Foundation;
using System;

namespace Balsamic.Views
{
    sealed partial class MyAppsWindowController : NSWindowController, IWindowController
    {
        NSNotificationCenter NotificationCenter { get; } = NSNotificationCenter.DefaultCenter;

        MyAppsSplitViewController SplitViewController => ContentViewController as MyAppsSplitViewController;

        #region Constructors

        public MyAppsWindowController(IntPtr handle) : base(handle) {}

        [Export("initWithCoder:")]
        public MyAppsWindowController(NSCoder coder) : base(coder) {}

        public MyAppsWindowController() : base("MyAppsWindow") {}

        #endregion

        #region Overrides

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            Window.Appearance = NSAppearance.GetAppearance(NSAppearance.NameVibrantDark);
            Window.TitleVisibility = NSWindowTitleVisibility.Hidden;

            NotificationCenter.AddObserver(ToggleCollapsed.Name, notification => {
                var collapsed = notification.UserInfo.ObjectForKey(IsCollapsed.NSString()) as NSNumber;
                var segmentIndex = notification.UserInfo.ObjectForKey(SegmentIndex.NSString()) as NSNumber;
                ToggleSidebarSegmentedControl.SetSelected(!collapsed.BoolValue, segmentIndex.NIntValue);
            });
        }

        public void ShowWindow()
        {
            Window.Center();
            Window.MakeKeyAndOrderFront(this);
        }

        internal new MyAppsWindow Window => (MyAppsWindow)base.Window;

        #endregion

        partial void ToggleSidebar(NSSegmentedControl sender)
        {
            switch (sender.SelectedSegment)
            {
                case 0:
                    SplitViewController.ToggleLeadingSidebar();
                    break;
                case 1:
                    break;
                case 2:
                    SplitViewController.ToggleTrailingSidebar();
                    break;
            }
        }
    }
}
