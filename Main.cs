using AppKit;

namespace Balsamic
{
    internal static class MainClass
    {
        private static void Main(string[] args)
        {
            NSApplication.Init();
            NSApplication.Main(args);
        }
    }
}
