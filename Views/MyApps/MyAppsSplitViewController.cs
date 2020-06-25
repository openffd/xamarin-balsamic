using AppKit;
using Balsamic.Views.MyApps;
using Foundation;
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

        private LeadingContentListViewController LeadingContentListViewController { get; } = new LeadingContentListViewController();
        private MyAppsContentViewController MyAppsContentViewController { get; } = new MyAppsContentViewController();
        private TrailingSidebarViewController TrailingSidebarViewController { get; } = new TrailingSidebarViewController();

#pragma warning disable IDE0051
        private NSLayoutConstraint? LeadingContentListViewWidthLayoutConstraint { get; set; }
        private NSLayoutConstraint? MyAppsContentViewWidthLayoutConstraint { get; set; }
        private NSLayoutConstraint? TrailingSidebarViewWidthLayoutConstraint { get; set; }

        private NSViewController OutlineViewController => SplitViewItems.First().ViewController;
        private NSViewController DetailViewController => SplitViewItems[1].ViewController;
        private NSViewController SidebarViewController => SplitViewItems.Last().ViewController;
#pragma warning restore IDE0051

        private List<IDisposable> Disposables { get; set; } = new List<IDisposable>();

        #region Internal Methods

        internal void ToggleLeadingSidebar()
        {
            NSSplitViewItem splitViewItem = (NSSplitViewItem)SplitViewItems.First().Animator;
            LeadingContentListViewWidthLayoutConstraint!.Active = false;
            NSAnimationContext.RunAnimation(changes: context => {
                context.Duration = 3;
                splitViewItem.Collapsed = !splitViewItem.Collapsed;
            }, completionHandler: () => {
                LeadingContentListViewWidthLayoutConstraint.Active = true;
            });
        }

        internal void ToggleTrailingSidebar()
        {
            NSSplitViewItem splitViewItem = (NSSplitViewItem)SplitViewItems.Last().Animator;
            TrailingSidebarViewWidthLayoutConstraint!.Active = false;
            NSAnimationContext.RunAnimation(changes: context => {
                context.Duration = 3;
                splitViewItem.Collapsed = !splitViewItem.Collapsed;
            }, completionHandler: () => {
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

        private void Initialize() {}

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
            NSLayoutConstraint layoutConstraint = LeadingContentListViewController.View.WidthAnchor.ConstraintGreaterThanOrEqualToConstant(280);
            layoutConstraint.Active = true;
            LeadingContentListViewWidthLayoutConstraint = layoutConstraint;
        }

        private void SetupTrailingSidebarViewControllerLayoutConstraint()
        {
            NSLayoutConstraint layoutConstraint = TrailingSidebarViewController.View.WidthAnchor.ConstraintGreaterThanOrEqualToConstant(12);
            layoutConstraint.Active = true;
            TrailingSidebarViewWidthLayoutConstraint = layoutConstraint;
        }

        private void SetupLeadingSplitViewItem()
        {
            NSSplitViewItem splitViewItem = NSSplitViewItem.CreateContentList(LeadingContentListViewController);
            splitViewItem.CanCollapse = true;
            splitViewItem.MaximumThickness = 400;
            AddSplitViewItem(splitViewItem);

            Disposables.Add(splitViewItem.AddObserver(KeyPath.NSSplitViewItem.Collapsed.String(), New, change => {
                NSDictionary userInfo = new NSDictionary
                (
                    ToggleCollapsed.UserInfoKey.IsCollapsed.String(), change.NewValue,
                    ToggleCollapsed.UserInfoKey.SegmentIndex.String(), 0
                );
                NotificationCenter.PostNotificationName(ToggleCollapsed.Name, null, userInfo);
            }));
        }

        private void SetupCenterSplitViewItem()
        {
            NSSplitViewItem splitViewItem = new NSSplitViewItem()
            {
                ViewController = MyAppsContentViewController,
                MinimumThickness = 12,
            };
            AddSplitViewItem(splitViewItem);
        }

        private void SetupTrailingSplitViewItem()
        {
            NSSplitViewItem splitViewItem = NSSplitViewItem.CreateSidebar(TrailingSidebarViewController);
            splitViewItem.CanCollapse = true;
            splitViewItem.MaximumThickness = 280;
            splitViewItem.MinimumThickness = 12;
            AddSplitViewItem(splitViewItem);

            Disposables.Add(splitViewItem.AddObserver(KeyPath.NSSplitViewItem.Collapsed.String(), New, change => {
                NSDictionary userInfo = new NSDictionary
                (
                    ToggleCollapsed.UserInfoKey.IsCollapsed.String(), change.NewValue,
                    ToggleCollapsed.UserInfoKey.SegmentIndex.String(), 2
                );
                NotificationCenter.PostNotificationName(ToggleCollapsed.Name, null, userInfo);
            }));
        }
    }
}
