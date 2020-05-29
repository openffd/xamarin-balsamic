using AppKit;
using Balsamic.Models;
using Balsamic.Models.Sample;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using static Balsamic.String.KeyPath;

namespace Balsamic.Views.MyApps
{
    sealed partial class LeadingContentListViewController : NSViewController
    {
        readonly DataProvider DataProvider = new DataProvider();

        [Export("Contents")]
        NSMutableArray Contents { get; set; } = new NSMutableArray();

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
