using System;
using Foundation;
using AppKit;

namespace Balsamic.Views.MyApps
{
    sealed partial class TrailingSidebarView : NSView
    {
        #region Constructors

        public TrailingSidebarView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public TrailingSidebarView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        void Initialize() {}

        #endregion
    }
}
