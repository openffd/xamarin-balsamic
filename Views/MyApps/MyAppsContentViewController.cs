using System;
using Foundation;
using AppKit;

namespace Balsamic.Views.MyApps
{
    sealed partial class MyAppsContentViewController : NSViewController
    {
        #region Constructors

        public MyAppsContentViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public MyAppsContentViewController(NSCoder coder) : base(coder)
        {

            Initialize();
        }

        public MyAppsContentViewController() : base("MyAppsContentView", NSBundle.MainBundle)
        {
            Initialize();
        }

        void Initialize() {}

        #endregion

        public new MyAppsContentView View => (MyAppsContentView)base.View;
    }
}
