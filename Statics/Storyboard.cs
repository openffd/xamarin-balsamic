using AppKit;

namespace Balsamic
{
    static class Storyboard
    {
        enum Name
        {
            Welcome
        }

        internal static NSStoryboard Welcome => NSStoryboard.FromName(Name.Welcome.ToString(), null);
    }
}
