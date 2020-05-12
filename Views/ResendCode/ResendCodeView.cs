using System;
using Foundation;

namespace Balsamic.Views
{
    sealed partial class ResendCodeView : AppKit.NSView
    {
        #region Constructors

        public ResendCodeView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public ResendCodeView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        void Initialize() {}

        #endregion
    }
}
