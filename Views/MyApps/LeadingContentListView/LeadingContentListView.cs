using System;
using Foundation;
using AppKit;

namespace Balsamic.Views.MyApps
{
    public partial class LeadingContentListView : NSView
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

        void Initialize() {}

        #endregion
    }
}
