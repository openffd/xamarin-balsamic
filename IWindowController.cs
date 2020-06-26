using AppKit;

namespace Balsamic
{
    internal sealed partial class AppDelegate
    {
        internal interface IWindowController
        {
            NSWindow Window { get; }

            void ShowWindow();
        }
    }
}
