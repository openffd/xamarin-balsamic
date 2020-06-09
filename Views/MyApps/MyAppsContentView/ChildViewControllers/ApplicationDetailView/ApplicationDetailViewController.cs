using System;
using Foundation;
using AppKit;

namespace Balsamic.Views.MyApps.MyAppsContentView
{
    public partial class ApplicationDetailViewController : NSViewController
    {
        #region Constructors

        public ApplicationDetailViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public ApplicationDetailViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public ApplicationDetailViewController() : base("ApplicationDetailView", NSBundle.MainBundle)
        {
            Initialize();
        }

        void Initialize() {}

        #endregion

        public new ApplicationDetailView View => (ApplicationDetailView)base.View;
    }
}
