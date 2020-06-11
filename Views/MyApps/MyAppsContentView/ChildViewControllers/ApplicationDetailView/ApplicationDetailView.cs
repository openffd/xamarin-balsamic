using System;
using Foundation;
using AppKit;

namespace Balsamic.Views.MyApps
{
    public partial class ApplicationDetailView : NSView
    {
        #region Constructors

        public ApplicationDetailView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public ApplicationDetailView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        void Initialize() {}

        #endregion
    }
}
