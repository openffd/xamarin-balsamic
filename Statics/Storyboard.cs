using AppKit;

namespace Balsamic
{
    static class Storyboard
    {
        enum Name
        {
            MyApps,
            Welcome,
        }

        internal static NSStoryboard MyApps => NSStoryboard.FromName(Name.MyApps.ToString(), null);
        internal static NSStoryboard Welcome => NSStoryboard.FromName(Name.Welcome.ToString(), null);
    }
}
