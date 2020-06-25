using AppKit;
using Balsamic.Models;
using Balsamic.Models.Sample;
using Foundation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static Balsamic.Models.LeadingContentListOutlineViewNodeType;
using static Balsamic.String.BindingOption;
using static Balsamic.String.KeyPath;
using static Balsamic.String.Notification;

namespace Balsamic.Views.MyApps
{
    internal sealed partial class LeadingContentListViewController : NSViewController
    {
        internal NSViewController? GetViewControllerForSelectedNodes(NSTreeNode[] nodes)
        {
            if (nodes?.Length == 0)
                return null;

            LeadingContentListOutlineViewNode outlineViewNode = nodes.First().GetOutlineViewNode();
            return outlineViewNode.NodeType switch
            {
                LeadingContentListOutlineViewNodeType.AppleDevAccount       => new AppleDevAccountViewController(),
                LeadingContentListOutlineViewNodeType.ApplicationVersion    => new ApplicationVersionViewController(),
                LeadingContentListOutlineViewNodeType.ApplicationDetail     => new ApplicationDetailViewController(),
                Separator => null,
                _ => null,
            };
        }

        private readonly DataProvider DataProvider = new DataProvider();

        private NSNotificationCenter NotificationCenter { get; } = NSNotificationCenter.DefaultCenter;

        private IDisposable? TreeControllerObservationDisposable { get; set; }

        private NSIndexPath[]? TreeControllerSelectionIndexPaths { get; set; }

        [Export("Contents")]
        private NSMutableArray Contents { get; set; } = new NSMutableArray();

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

        private void Initialize() {}

        #endregion

        internal new LeadingContentListView View => (LeadingContentListView)base.View;

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var appleDevAccountNode = new LeadingContentListOutlineViewNode(payload: DataProvider.AppleDevAccount!);
            var applicationDetailNodes = DataProvider.ApplicationDetails.Select(
                item => new LeadingContentListOutlineViewNode(item) as Node).ToList();
            var applicationVersionNodes = DataProvider.ApplicationVersions.Select(
                item => new LeadingContentListOutlineViewNode(item) as Node).ToList();
            applicationDetailNodes.ForEach(node => {
                var versionNode = applicationVersionNodes.Find(versionNode => {
                    var detail = (ApplicationDetail)((LeadingContentListOutlineViewNode)node).Payload;
                    var version = (ApplicationVersion)((LeadingContentListOutlineViewNode)versionNode).Payload;
                    return detail.Id == version.AppId;
                });
                node.Add(versionNode);
                node.Add(new LeadingContentListOutlineViewNode(new LeadingContentListOutlineViewSeparator()));
            });
            appleDevAccountNode.AddRange(applicationDetailNodes);

            Contents.Add(appleDevAccountNode);

            SetupTreeController();
            SetupOutlineView();
        }

        public override void ViewWillDisappear()
        {
            base.ViewWillDisappear();
            TreeControllerObservationDisposable?.Dispose();
        }

        private void SetupTreeController()
        {
            TreeControllerObservationDisposable = TreeController.AddObserver(
                NSTreeControllerKeyPath.SelectedObjects.String(),
                NSKeyValueObservingOptions.New,
                observedChange => {
                    NSNotification notification = NSNotification.FromName(TreeControllerObservation.Name, TreeController);
                    NotificationCenter.PostNotification(notification);
                    TreeControllerSelectionIndexPaths = TreeController.SelectionIndexPaths;
                    InvalidateRestorableState();
                });
            TreeController.ObjectClass = new ObjCRuntime.Class(typeof(LeadingContentListOutlineViewNode));
            TreeController.LeafKeyPath = TreeControllerBindingKeyPath.Leaf.String();
            TreeController.ChildrenKeyPath = TreeControllerBindingKeyPath.Children.String();
            TreeController.CountKeyPath = TreeControllerBindingKeyPath.Count.String();
            TreeController.Bind(NSTreeControllerKeyPath.ContentArray.NSString(), this, KeyPath.Contents.String(), null);
        }

        private void SetupOutlineView()
        {
            OutlineView.RegisterNib(ApplicationDetailTableCellView.Nib, ApplicationDetailTableCellView.Identifier);
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

        private enum KeyPath
        {
            [Description("Contents")] Contents,
        }

        private enum TreeControllerBindingKeyPath
        {
            [Description("Leaf")]       Leaf,
            [Description("Count")]      Count,
            [Description("Children")]   Children,
        }
    }
}
