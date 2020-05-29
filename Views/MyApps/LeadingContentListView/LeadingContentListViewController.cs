using AppKit;
using Balsamic.Models;
using Balsamic.Models.Sample;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using static Balsamic.String.BindingOption;
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

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var appleDevAccountNode = new LeadingContentListOutlineViewNode(DataProvider.AppleDevAccount);
            var applicationDetailNodes = DataProvider.ApplicationDetails.Select(
                item => new LeadingContentListOutlineViewNode(item) as Node).ToList();
            var applicationVersionNodes = DataProvider.ApplicationVersions.Select(
                item => new LeadingContentListOutlineViewNode(item) as Node).ToList();
            applicationDetailNodes.First().AddRange(applicationVersionNodes);
            appleDevAccountNode.AddRange(applicationDetailNodes);
            Contents.Add(appleDevAccountNode);

            TreeController.Bind(NSTreeControllerKeyPath.ContentArray.NSString(), this, "Contents", null);
            TreeController.ObjectClass = new ObjCRuntime.Class(typeof(LeadingContentListOutlineViewNode));

            SetupOutlineView();
        }

        void SetupOutlineView()
        {
            OutlineView.Bind(
                NSOutlineViewKeyPath.SelectionIndexPaths.NSString(),
                TreeController, NSTreeControllerKeyPath.SelectionIndexPaths.String(),
                new NSDictionary(NSRaisesForNotApplicableKeysBindingOption.NSString(), new NSNumber(true)));
            OutlineView.Bind(
                NSOutlineViewKeyPath.SortDescriptors.NSString(),
                TreeController, NSTreeControllerKeyPath.SortDescriptors.String(),
                null);
            OutlineView.Bind(
                NSOutlineViewKeyPath.Content.NSString(),
                TreeController, NSTreeControllerKeyPath.ArrangedObjects.String(),
                new NSDictionary(NSAlwaysPresentsApplicationModalAlertsBindingOption.NSString(), new NSNumber(true)));
            OutlineView.AutosaveExpandedItems = true;
            OutlineView.DataSource = new LeadingContentListOutlineViewDataSource();
            OutlineView.Delegate = new LeadingContentListOutlineViewDelegate();
            OutlineView.SelectionHighlightStyle = NSTableViewSelectionHighlightStyle.Regular;
            OutlineView.ReloadData();
        }
    }
}
