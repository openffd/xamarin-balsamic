using AppKit;
using Foundation;

namespace Balsamic
{
    public interface IWindowController
    {
        NSWindow Window { get; }

        void ShowWindow();
    }

    public partial class AppDelegate : NSApplicationDelegate
    {
        private readonly IWindowController windowController = new LaunchWindowController();

        public AppDelegate() {}

        public override void DidFinishLaunching(NSNotification notification)
        {
            windowController.ShowWindow();
        }

        public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender) => true;

        public override void WillTerminate(NSNotification notification) {}
    }
}
