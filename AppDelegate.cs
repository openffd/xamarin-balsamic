using AppKit;
using Foundation;

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

        public AppDelegate() {}

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender) => true;

        public override void DidFinishLaunching(NSNotification notification)
        {
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
                    return NSApplicationTerminateReply.Cancel;
                }
            }
            return NSApplicationTerminateReply.Now;
        }

        public override void WillTerminate(NSNotification notification) {}
    }
}
