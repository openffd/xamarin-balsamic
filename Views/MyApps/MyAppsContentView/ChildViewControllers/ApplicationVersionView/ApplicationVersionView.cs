using System;
using Foundation;
using AppKit;

namespace Balsamic.Views.MyApps.MyAppsContentView
{
    public partial class ApplicationVersionView : NSView
    {
        #region Constructors

        public ApplicationVersionView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public ApplicationVersionView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        void Initialize() {}

        #endregion
    }
}
