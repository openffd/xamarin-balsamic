using Foundation;
using System.ComponentModel;

namespace Balsamic
{
    static class String
    {
        internal enum KeyEquivalent
        {
            [Description("\r")]
            Return,
        }

        internal enum ErrorMessage
        {
            [Description("⚠️ Not a valid email")]
            InvalidEmail,
        }

        internal static class KeyPath
        {
            internal enum NSSplitView
            {
                [Description("dividerColor")]
                DividerColor,
            }

            internal enum NSSplitViewItem
            {
                [Description("collapsed")]
                Collapsed,
            }
        }

        internal static class Notification
        {
            internal static class ToggleCollapsed
            {
                internal static NSString Name => (NSString)"ToggleCollapsed";

                internal enum UserInfoKey
                {
                    [Description("IsCollapsed")]
                    IsCollapsed,

                    [Description("SegmentIndex")]
                    SegmentIndex,
                }
            }
        }

        internal static class MenuItemTitle
        {
            internal static string CheckForUpdates = "Check for Updates...";
        }
    }
}
