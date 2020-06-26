using System;
using Foundation;
using AppKit;

namespace Balsamic.Views.MyApps
{
    public sealed partial class MyAppsContentView : NSView
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

        private void Initialize() {}

        #endregion
    }
}
