using AppKit;
using Balsamic.Views.MyApps;
using Foundation;
using System;
using System.Linq;

namespace Balsamic.Views
{
    sealed partial class MyAppsSplitViewController : NSSplitViewController
    {
        NSViewController LeadingSidebarViewController => SplitViewItems.First().ViewController;
        NSViewController ItemContentViewController => SplitViewItems[1].ViewController;
        NSViewController TrailingSidebarViewController => SplitViewItems.Last().ViewController;

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
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SetupLeadingContentListViewController();
            SetupContentViewController();
            SetupTrailingSidebarViewController();
        }

        public override void ViewDidAppear()
        {
            base.ViewDidAppear();
        }

        void SetupLeadingContentListViewController()
        {
            var viewController = new LeadingContentListViewController();
            viewController.View.WidthAnchor.ConstraintGreaterThanOrEqualToConstant(280).Active = true;

            var splitViewItem = NSSplitViewItem.CreateContentList(viewController);
            splitViewItem.MaximumThickness = 440;
            AddSplitViewItem(splitViewItem);
        }

        void SetupContentViewController()
        {
            var viewController = new MyAppsContentViewController();
            var splitViewItem = new NSSplitViewItem()
            {
                ViewController = viewController,
                MinimumThickness = 12,
            };
            AddSplitViewItem(splitViewItem);
        }

        void SetupTrailingSidebarViewController()
        {
            var viewController = new TrailingSidebarViewController();
            viewController.View.WidthAnchor.ConstraintGreaterThanOrEqualToConstant(12).Active = true;
            var splitViewItem = NSSplitViewItem.CreateSidebar(viewController);
            splitViewItem.MaximumThickness = 280;
            splitViewItem.MinimumThickness = 12;
            AddSplitViewItem(splitViewItem);
        }
    }
}
