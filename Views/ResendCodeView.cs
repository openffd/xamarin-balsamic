using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace Balsamic.Views
{
    public partial class ResendCodeView : AppKit.NSView
    {
        #region Constructors

        // Called when created from unmanaged code
        public ResendCodeView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public ResendCodeView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion
    }
}
