using System;
using Foundation;
using AppKit;

namespace Balsamic.Views.MyApps.LeadingContentListViewControllerOutlineView
{
    public sealed partial class ApplicationDetailTableCellView : NSTableCellView
    {
        #region Constructors

        public ApplicationDetailTableCellView(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public ApplicationDetailTableCellView(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public ApplicationDetailTableCellView()
        {
            Initialize();
        }

        private void Initialize() {}

        #endregion

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
            ImageView.Rounded();
        }
    }
}
