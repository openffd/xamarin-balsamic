using AppKit;
using Foundation;
using System;

namespace Balsamic.Views.MyApps
{
    public sealed partial class AppleDevAccountViewController : NSViewController
    {
        #region Constructors

        public AppleDevAccountViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public AppleDevAccountViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public AppleDevAccountViewController() : base("AppleDevAccountView", NSBundle.MainBundle)
        {
            Initialize();
        }

        private void Initialize() {}

        #endregion

        public new AppleDevAccountView View => (AppleDevAccountView)base.View;
    }
}
