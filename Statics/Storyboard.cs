using AppKit;

namespace Balsamic
{
    internal static class Storyboard
    {
        private enum Name
        {
            MyApps,
            Welcome,
        }

        internal static NSStoryboard MyApps => NSStoryboard.FromName(Name.MyApps.ToString(), null);
        internal static NSStoryboard Welcome => NSStoryboard.FromName(Name.Welcome.ToString(), null);
    }
}
