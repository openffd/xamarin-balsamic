using AppKit;
using Foundation;
using System;

namespace Balsamic.Views
{
    sealed partial class ResendCodeViewController : NSViewController
    {
        #region Constructors

        public ResendCodeViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public ResendCodeViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public ResendCodeViewController() : base("ResendCodeView", NSBundle.MainBundle)
        {
            Initialize();
        }

        void Initialize() {}

        #endregion

        public new ResendCodeView View => (ResendCodeView)base.View;
    }
}
