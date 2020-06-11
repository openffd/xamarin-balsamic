﻿using AppKit;
using Foundation;
using System;

namespace Balsamic.Views.MyApps
{
    public partial class AppleDevAccountViewController : NSViewController
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

        void Initialize() {}

        #endregion

        public new AppleDevAccountView View => (AppleDevAccountView)base.View;
    }
}
