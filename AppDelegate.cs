using AppKit;
using Balsamic.Views;
using BalsamicSpark;
using Foundation;
using System.Linq;
using static AppKit.NSApplicationTerminateReply;
using static Balsamic.String;

namespace Balsamic
{
    internal sealed partial class AppDelegate : NSApplicationDelegate
    {
#if DEBUG
        IWindowController WindowController { get; } = new LaunchWindowController();
#else
        private IWindowController? WindowController { get; } = Storyboard.MyApps.InstantiateInitialController() as MyAppsWindowController;
#endif
        private NSApplication SharedApplication => NSApplication.SharedApplication;

        private SUUpdater SharedUpdater => SUUpdater.SharedUpdater;

        public AppDelegate() {}

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender)
        {
            return true;
        }

        public override void WillFinishLaunching(NSNotification notification)
        {
            SetupCheckForUpdatesMenuItem();
        }

        private void SetupCheckForUpdatesMenuItem()
        {
            using NSMenuItem menuItem = new NSMenuItem(MenuItemTitle.CheckForUpdates, handler: delegate {
                SharedUpdater.CheckForUpdates(this);
            });
            NSMenuItem mainMenuFirstItem = SharedApplication.MainMenu.Items.First();
            mainMenuFirstItem.Submenu.InsertItem(menuItem, 1);
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            WindowController?.ShowWindow();
        }
        
        public override NSApplicationTerminateReply ApplicationShouldTerminate(NSApplication sender)
        {
#pragma warning disable XI0001
            foreach (NSWindow window in NSApplication.SharedApplication.DangerousWindows)
#pragma warning restore XI0001
            {
                if (!(window.Delegate == null || window.Delegate.WindowShouldClose(this)))
                {
                    return Cancel;
                }
            }
            return Now;
        }

        public override void WillTerminate(NSNotification notification) {}
    }
}
