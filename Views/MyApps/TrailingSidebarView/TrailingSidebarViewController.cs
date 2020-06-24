using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace Balsamic.Views.MyApps
{
    sealed partial class TrailingSidebarViewController : NSViewController
    {
        #region Constructors

        public TrailingSidebarViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public TrailingSidebarViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public TrailingSidebarViewController() : base("TrailingSidebarView", NSBundle.MainBundle)
        {
            Initialize();
        }

        void Initialize() {}

        #endregion

        public new TrailingSidebarView View => (TrailingSidebarView)base.View;
    }
}
