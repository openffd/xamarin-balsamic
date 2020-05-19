using AppKit;
using static Balsamic.String;
using static Balsamic.String.Notification;
using Balsamic.Views.MyApps;
using CoreAnimation;
using Foundation;
using static Foundation.NSKeyValueObservingOptions;
using ReactiveUI;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Balsamic.Views
{
    sealed partial class MyAppsSplitViewController : NSSplitViewController
    {
        NSNotificationCenter NotificationCenter { get; } = NSNotificationCenter.DefaultCenter;

        LeadingContentListViewController LeadingContentListViewController   { get; } = new LeadingContentListViewController();
        MyAppsContentViewController MyAppsContentViewController             { get; } = new MyAppsContentViewController();
        TrailingSidebarViewController TrailingSidebarViewController         { get; } = new TrailingSidebarViewController();

        NSLayoutConstraint LeadingContentListViewWidthLayoutConstraint  { get; set; }
        NSLayoutConstraint MyAppsContentViewWidthLayoutConstraint       { get; set; }
        NSLayoutConstraint TrailingSidebarViewWidthLayoutConstraint     { get; set; }

        List<IDisposable> Disposables { get; set; } = new List<IDisposable>();

        #region Internal Methods

        internal void ToggleLeadingSidebar()
        {
            var animator = SplitViewItems.First().Animator as NSSplitViewItem;
            LeadingContentListViewWidthLayoutConstraint.Active = false;
            NSAnimationContext.RunAnimation(context => {
                context.Duration = 3;
                animator.Collapsed = !animator.Collapsed;
            }, () => {
                LeadingContentListViewWidthLayoutConstraint.Active = true;
            });
        }

        internal void ToggleTrailingSidebar()
        {
            var animator = SplitViewItems.Last().Animator as NSSplitViewItem;
            TrailingSidebarViewWidthLayoutConstraint.Active = false;
            NSAnimationContext.RunAnimation(context => {
                context.Duration = 3;
                animator.Collapsed = !animator.Collapsed;
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

        public override void ViewWillDisappear()
        {
            base.ViewWillDisappear();

            Disposables.ForEach(item => item.Dispose());
        }

        void SetupLeadingContentListViewControllerLayoutConstraint()
        {
            var layoutConstraint = LeadingContentListViewController.View.WidthAnchor.ConstraintGreaterThanOrEqualToConstant(280);
            layoutConstraint.Active = true;
            LeadingContentListViewWidthLayoutConstraint = layoutConstraint;
        }

        void SetupTrailingSidebarViewControllerLayoutConstraint()
        {
            var layoutConstraint = TrailingSidebarViewController.View.WidthAnchor.ConstraintGreaterThanOrEqualToConstant(12);
            layoutConstraint.Active = true;
            TrailingSidebarViewWidthLayoutConstraint = layoutConstraint;
        }

        void SetupLeadingSplitViewItem()
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

        void SetupCenterSplitViewItem()
        {
            var splitViewItem = new NSSplitViewItem()
            {
                ViewController = MyAppsContentViewController,
                MinimumThickness = 12,
            };
            AddSplitViewItem(splitViewItem);
        }

        void SetupTrailingSplitViewItem()
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
