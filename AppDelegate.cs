using AppKit;
using static AppKit.NSApplicationTerminateReply;
using Foundation;
using Sparkle;

namespace Balsamic
{
    sealed partial class AppDelegate : NSApplicationDelegate
    {
        internal interface IWindowController
        {
            NSWindow Window { get; }

            void ShowWindow();
        }

        readonly IWindowController launchWindowController = new LaunchWindowController();
        readonly IWindowController myAppsWindowController = Storyboard.MyApps.InstantiateInitialController() as Views.MyAppsWindowController;
        readonly SUUpdater updater = new SUUpdater();

        public AppDelegate() {}

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender) => true;

        public override void WillFinishLaunching(NSNotification notification)
        {
            var item = new NSMenuItem("Check for Updates...", delegate {
                System.Console.WriteLine("Checking for updates...");
            });
            NSApplication.SharedApplication.MainMenu.Items[0].Submenu.InsertItem(item, 1);
        }

        void SetupCheckForUpdatesMenuItem()
        {

        }

        public override void DidFinishLaunching(NSNotification notification)
        {
            //NSApplication.SharedApplication.MainMenu.Items[0].Submenu.Inse //Items[1].Action = new ObjCRuntime.Selector("Hello");

            myAppsWindowController.ShowWindow();
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
