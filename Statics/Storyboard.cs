using AppKit;

namespace Balsamic
{
    public static class Storyboard
    {
        private enum Name
        {
            Welcome = 0
        }

        public static readonly NSStoryboard Welcome = NSStoryboard.FromName(Name.Welcome.ToString(), null);
    }
}
