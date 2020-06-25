using AppKit;
using Foundation;
using System;
using static Balsamic.AppDelegate;
using static Balsamic.String.Notification;
using static Balsamic.String.Notification.ToggleCollapsed.UserInfoKey;

namespace Balsamic.Views
{
    internal sealed partial class MyAppsWindowController : NSWindowController, IWindowController
    {
        private NSNotificationCenter NotificationCenter { get; } = NSNotificationCenter.DefaultCenter;

        private MyAppsSplitViewController? SplitViewController => ContentViewController as MyAppsSplitViewController;

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

            SetupStatusDisplayTextField();

            _ = NotificationCenter.AddObserver(ToggleCollapsed.Name, notification =>
            {
                NSNumber? collapsed = (NSNumber)notification.UserInfo.ObjectForKey(IsCollapsed.NSString());
                NSNumber? segmentIndex = (NSNumber)notification.UserInfo.ObjectForKey(SegmentIndex.NSString());
                ToggleSidebarSegmentedControl.SetSelected(!collapsed.BoolValue, segmentIndex.NIntValue);
            });
        }

        private void SetupStatusDisplayTextField()
        {
            NSTextAttachment attachment = new NSTextAttachment
            {
                Image = NSImage.ImageNamed(NSImageName.ApplicationIcon),
                Bounds = CoreGraphics.CGRect.FromLTRB(0, -4, 14, 10),
            };
            NSAttributedString imageAttributedString = NSAttributedString.FromAttachment(attachment);

            NSMutableAttributedString statusDisplayAttributedString = new NSMutableAttributedString();
            statusDisplayAttributedString.Append(imageAttributedString);
            statusDisplayAttributedString.Append(new NSAttributedString("  Apple Cider 2020 for Mac"));

            StatusDisplayTextField.AttributedStringValue = statusDisplayAttributedString;
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
                    SplitViewController?.ToggleLeadingSidebar();
                    break;
                case 1:
                    break;
                case 2:
                    SplitViewController?.ToggleTrailingSidebar();
                    break;
                default:
                    break;
            }
        }
    }
}
