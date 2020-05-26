using System;
using Foundation;
using AppKit;
using Balsamic.Models.Sample;

namespace Balsamic.Views.MyApps
{
    sealed partial class LeadingContentListViewController : NSViewController
    {
        readonly DataProvider DataProvider = new DataProvider();

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

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var dataSource = new LeadingContentListOutlineViewDataSource();

            OutlineView.AutosaveExpandedItems = true;
            OutlineView.DataSource = dataSource;
            OutlineView.Delegate = new LeadingContentListOutlineViewDelegate();
            OutlineView.SelectionHighlightStyle = NSTableViewSelectionHighlightStyle.SourceList;
            OutlineView.ReloadData();
        }
    }
}
