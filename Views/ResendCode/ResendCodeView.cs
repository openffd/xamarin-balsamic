using System;
using Foundation;

namespace Balsamic.Views
{
    public sealed partial class ResendCodeView : AppKit.NSView
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

        private void Initialize() {}

        #endregion
    }
}
