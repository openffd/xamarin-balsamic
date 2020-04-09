using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace Balsamic.Views
{
    public partial class ResendCodeViewController : AppKit.NSViewController
    {
        #region Constructors

        // Called when created from unmanaged code
        public ResendCodeViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        // Called when created directly from a XIB file
        [Export("initWithCoder:")]
        public ResendCodeViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        // Call to load from the XIB/NIB file
        public ResendCodeViewController() : base("ResendCodeView", NSBundle.MainBundle)
        {
            Initialize();
        }

        // Shared initialization code
        void Initialize()
        {
        }

        #endregion

        //strongly typed view accessor
        public new ResendCodeView View
        {
            get
            {
                return (ResendCodeView)base.View;
            }
        }
    }
}
