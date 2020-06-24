using AppKit;
using Balsamic.Views.MyApps;
using Foundation;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using static Balsamic.String;
using static Balsamic.String.Notification;
using static Foundation.NSKeyValueObservingOptions;

namespace Balsamic.Views
{
    public sealed partial class MyAppsSplitViewController : NSSplitViewController
    {
        private NSNotificationCenter NotificationCenter { get; } = NSNotificationCenter.DefaultCenter;

        LeadingContentListViewController LeadingContentListViewController   { get; } = new LeadingContentListViewController();
        MyAppsContentViewController MyAppsContentViewController             { get; } = new MyAppsContentViewController();
        TrailingSidebarViewController TrailingSidebarViewController         { get; } = new TrailingSidebarViewController();

        NSLayoutConstraint? LeadingContentListViewWidthLayoutConstraint  { get; set; }
        NSLayoutConstraint? MyAppsContentViewWidthLayoutConstraint       { get; set; }
        NSLayoutConstraint? TrailingSidebarViewWidthLayoutConstraint     { get; set; }

        private List<IDisposable> Disposables { get; set; } = new List<IDisposable>();

        private NSViewController OutlineViewController => SplitViewItems.First().ViewController;
        private NSViewController DetailViewController => SplitViewItems[1].ViewController;
        private NSViewController SidebarViewController => SplitViewItems.Last().ViewController;

        #region Internal Methods

        internal void ToggleLeadingSidebar()
        {
            NSSplitViewItem splitViewItem = (NSSplitViewItem)SplitViewItems.First().Animator;
            LeadingContentListViewWidthLayoutConstraint!.Active = false;
            NSAnimationContext.RunAnimation(context => {
                context.Duration = 3;
                splitViewItem.Collapsed = !splitViewItem.Collapsed;
            }, () => {
                LeadingContentListViewWidthLayoutConstraint.Active = true;
            });
        }

        internal void ToggleTrailingSidebar()
        {
            NSSplitViewItem splitViewItem = (NSSplitViewItem)SplitViewItems.Last().Animator;
            TrailingSidebarViewWidthLayoutConstraint!.Active = false;
            NSAnimationContext.RunAnimation(context => {
                context.Duration = 3;
                splitViewItem.Collapsed = !splitViewItem.Collapsed;
            }, () => {
                TrailingSidebarViewWidthLayoutConstraint.Active = true;
            });
        }

        #endregion

        #region Constructors

        public MyAppsSplitViewController(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        [Export("initWithCoder:")]
        public MyAppsSplitViewController(NSCoder coder) : base(coder)
        {
            Initialize();
        }

        public MyAppsSplitViewController() : base("MyAppsSplitView", NSBundle.MainBundle)
        {
            Initialize();
        }

        void Initialize() {}

        #endregion

        #region NSViewController

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            SplitView.IsVertical = true;
            SplitView.DividerStyle = NSSplitViewDividerStyle.Thin;
            SplitView.SetValueForKey(NSColor.FromWhite((nfloat)0.15, (nfloat)0.9), KeyPath.NSSplitView.DividerColor.NSString());
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupLeadingContentListViewControllerLayoutConstraint();
            SetupTrailingSidebarViewControllerLayoutConstraint();

            SetupLeadingSplitViewItem();
            SetupCenterSplitViewItem();
            SetupTrailingSplitViewItem();
        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();

            Disposables.Add(
                NotificationCenter.AddObserver(TreeControllerObservation.Name, notification => {
                    HandleSelectionChange(notification);
                })
            );
        }

        public override void ViewWillDisappear()
        {
            base.ViewWillDisappear();
            Disposables.ForEach(item => item.Dispose());
            Console.WriteLine("Disappeared");
        }

        #endregion

        private void EmbedChildViewController(NSViewController viewController)
        {
            DetailViewController.AddChildViewController(viewController);
            NSView firstSubview = DetailViewController.View.Subviews.First();
            firstSubview.AddSubview(viewController.View);
            viewController.View.TranslatesAutoresizingMaskIntoConstraints = false;

            NSLayoutConstraint[] horizontalConstraints = NSLayoutConstraint.FromVisualFormat(
                "H:|[targetView]|",
                NSLayoutFormatOptions.None,
                null,
                NSDictionary.FromObjectAndKey(viewController.View, (NSString)"targetView"));
            NSLayoutConstraint.ActivateConstraints(horizontalConstraints);

            NSLayoutConstraint[] verticalConstraints = NSLayoutConstraint.FromVisualFormat(
                "V:|[targetView]|",
                NSLayoutFormatOptions.None,
                null,
                NSDictionary.FromObjectAndKey(viewController.View, (NSString)"targetView"));
            NSLayoutConstraint.ActivateConstraints(verticalConstraints);
        }

        private void HandleSelectionChange(NSNotification notification)
        {
            if (!(notification.Object is NSTreeController treeController))
                return;

            if (OutlineViewController is LeadingContentListViewController outlineViewController)
            {
                NSTreeNode[] selectedNodes = treeController.SelectedNodes;
                NSViewController? viewControllerForSelection = outlineViewController.GetViewControllerForSelectedNodes(selectedNodes);
                if (viewControllerForSelection is null)
                {
                    DetailViewController.RemoveFirstChildViewController();
                    return;
                }

                SetupViewControllerFromSelection(viewControllerForSelection);
            }
        }

        private void SetupViewControllerFromSelection(NSViewController viewController)
        {
            if (DetailViewController.HasChildViewController())
            {
                if (viewController == DetailViewController.ChildViewControllers.First())
                    return;

                DetailViewController.RemoveFirstChildViewController();
                EmbedChildViewController(viewController);
            }
            else
            {
                EmbedChildViewController(viewController);
            }
        }

        private void SetupLeadingContentListViewControllerLayoutConstraint()
        {
            var layoutConstraint = LeadingContentListViewController.View.WidthAnchor.ConstraintGreaterThanOrEqualToConstant(280);
            layoutConstraint.Active = true;
            LeadingContentListViewWidthLayoutConstraint = layoutConstraint;
        }

        private void SetupTrailingSidebarViewControllerLayoutConstraint()
        {
            var layoutConstraint = TrailingSidebarViewController.View.WidthAnchor.ConstraintGreaterThanOrEqualToConstant(12);
            layoutConstraint.Active = true;
            TrailingSidebarViewWidthLayoutConstraint = layoutConstraint;
        }

        private void SetupLeadingSplitViewItem()
        {
            var splitViewItem = NSSplitViewItem.CreateContentList(LeadingContentListViewController);
            splitViewItem.CanCollapse = true;
            splitViewItem.MaximumThickness = 400;
            AddSplitViewItem(splitViewItem);

            Disposables.Add(splitViewItem.AddObserver(KeyPath.NSSplitViewItem.Collapsed.String(), New, change => {
                var userInfo = new NSDictionary
                (
                    ToggleCollapsed.UserInfoKey.IsCollapsed.String(), change.NewValue,
                    ToggleCollapsed.UserInfoKey.SegmentIndex.String(), 0
                );
                NotificationCenter.PostNotificationName(ToggleCollapsed.Name, null, userInfo);
            }));
        }

        private void SetupCenterSplitViewItem()
        {
            var splitViewItem = new NSSplitViewItem()
            {
                ViewController = MyAppsContentViewController,
                MinimumThickness = 12,
            };
            AddSplitViewItem(splitViewItem);
        }

        private void SetupTrailingSplitViewItem()
        {
            var splitViewItem = NSSplitViewItem.CreateSidebar(TrailingSidebarViewController);
            splitViewItem.CanCollapse = true;
            splitViewItem.MaximumThickness = 280;
            splitViewItem.MinimumThickness = 12;
            AddSplitViewItem(splitViewItem);

            Disposables.Add(splitViewItem.AddObserver(KeyPath.NSSplitViewItem.Collapsed.String(), New, change => {
                var userInfo = new NSDictionary
                (
                    ToggleCollapsed.UserInfoKey.IsCollapsed.String(), change.NewValue,
                    ToggleCollapsed.UserInfoKey.SegmentIndex.String(), 2
                );
                NotificationCenter.PostNotificationName(ToggleCollapsed.Name, null, userInfo);
            }));
        }
    }
}
