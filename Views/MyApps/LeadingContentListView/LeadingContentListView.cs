using System;
using Foundation;
using AppKit;

namespace Balsamic.Views.MyApps
{
    public sealed partial class LeadingContentListView : NSView
    {
        #region Constructors

        public LeadingContentListView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public LeadingContentListView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        private void Initialize() {}

        #endregion
    }
}
