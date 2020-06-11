using System;
using Foundation;
using AppKit;

namespace Balsamic.Views.MyApps
{
    public partial class ApplicationVersionViewController : NSViewController
    {
        #region Constructors

        public ApplicationVersionViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public ApplicationVersionViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public ApplicationVersionViewController() : base("ApplicationVersionView", NSBundle.MainBundle)
        {
            Initialize();
        }

        void Initialize() {}

        #endregion

        public new ApplicationVersionView View => (ApplicationVersionView)base.View;
    }
}
