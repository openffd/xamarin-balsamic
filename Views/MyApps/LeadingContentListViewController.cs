using System;
using Foundation;
using AppKit;

namespace Balsamic.Views.MyApps
{
    sealed partial class LeadingContentListViewController : NSViewController
    {
        #region Constructors

        public LeadingContentListViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public LeadingContentListViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public LeadingContentListViewController() : base("LeadingContentListView", NSBundle.MainBundle)
        {
            Initialize();
        }

        void Initialize() {}

        #endregion

        internal new LeadingContentListView View => base.View as LeadingContentListView;
    }
}
