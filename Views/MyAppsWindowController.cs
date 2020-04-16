using AppKit;
using Foundation;
using System;

namespace Balsamic.Views
{
    public partial class MyAppsWindowController : NSWindowController, AppDelegate.IWindowController
    {
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

            Window.Appearance = NSAppearance.GetAppearance(NSAppearance.NameDarkAqua);
            Window.TitleVisibility = NSWindowTitleVisibility.Hidden;
        }

        public void ShowWindow()
        {
            Window.Center();
            Window.MakeKeyAndOrderFront(this);
        }

        internal new MyAppsWindow Window => (MyAppsWindow)base.Window;

        #endregion
    }
}
