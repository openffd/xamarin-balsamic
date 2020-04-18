using System;
using Foundation;
using AppKit;

namespace Balsamic.Views.MyApps
{
    sealed partial class MyAppsContentView : NSView
    {
        #region Constructors

        public MyAppsContentView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public MyAppsContentView(NSCoder coder) : base(coder)
        {
            Initialize();

        }

        void Initialize() {}

        #endregion
    }
}
