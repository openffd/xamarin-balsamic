using AppKit;
using static AppKit.NSApplicationTerminateReply;
using static Balsamic.String;
using Balsamic.Views;
using Foundation;
using System.Linq;

namespace Balsamic
{
    sealed partial class AppDelegate : NSApplicationDelegate
    {
        internal interface IWindowController
        {
            NSWindow Window { get; }

            void ShowWindow();
        }

#if DEBUG
        IWindowController WindowController { get; } = new LaunchWindowController();
#else
        IWindowController WindowController { get; } = Storyboard.MyApps.InstantiateInitialController() as MyAppsWindowController;
#endif
        NSApplication SharedApplication => NSApplication.SharedApplication;

        public AppDelegate() {}

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender) => true;

        public override void WillFinishLaunching(NSNotification notification)
        {
            SetupCheckForUpdatesMenuItem();
        }

        void SetupCheckForUpdatesMenuItem()
        {
            var menuItem = new NSMenuItem(MenuItemTitle.CheckForUpdates, delegate {
                System.Console.WriteLine("Checking for updates...");
            });
            var mainMenuFirstItem = SharedApplication.MainMenu.Items.First();
            mainMenuFirstItem.Submenu.InsertItem(menuItem, 1);
        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            WindowController.ShowWindow();
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
