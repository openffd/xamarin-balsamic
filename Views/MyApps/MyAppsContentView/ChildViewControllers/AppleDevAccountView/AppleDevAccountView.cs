using AppKit;
using Foundation;
using System;

namespace Balsamic.Views.MyApps.MyAppsContentView
{
    public partial class AppleDevAccountView : NSView
    {
        #region Constructors

        public AppleDevAccountView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public AppleDevAccountView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        void Initialize() {}

        #endregion
    }
}
