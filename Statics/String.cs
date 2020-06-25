using Foundation;
using System.ComponentModel;

namespace Balsamic
{
    internal static class String
    {
        internal enum BindingOption
        {
            [Description("NSAlwaysPresentsApplicationModalAlertsBindingOption")] NSAlwaysPresentsApplicationModalAlertsBindingOption,
            [Description("NSRaisesForNotApplicableKeysBindingOption")] NSRaisesForNotApplicableKeysBindingOption,
        }

        internal enum KeyEquivalent
        {
            [Description("\r")] Return,
        }

        internal enum ErrorMessage
        {
            [Description("⚠️ Not a valid email")] InvalidEmail,
        }

        internal static class KeyPath
        {
            internal enum NSOutlineViewKeyPath
            {
                [Description("content")]                Content,
                [Description("selectionIndexPaths")]    SelectionIndexPaths,
                [Description("sortDescriptors")]        SortDescriptors,
            }

            internal enum NSSplitView
            {
                [Description("dividerColor")] DividerColor,
            }

            internal enum NSSplitViewItem
            {
                [Description("collapsed")] Collapsed,
            }

            internal enum NSTreeControllerKeyPath
            {
                [Description("arrangedObjects")]        ArrangedObjects,
                [Description("contentArray")]           ContentArray,
                [Description("selectedObjects")]        SelectedObjects,
                [Description("selectionIndexPaths")]    SelectionIndexPaths,
                [Description("sortDescriptors")]        SortDescriptors,
            }
        }

        internal enum FileExtension
        {
            [Description("json")]   Json,
            [Description("xml")]    Xml,
        }

        internal static class Notification
        {
            internal static class ToggleCollapsed
            {
                internal static NSString Name => (NSString)"ToggleCollapsed";

                internal enum UserInfoKey
                {
                    [Description("IsCollapsed")]    IsCollapsed,
                    [Description("SegmentIndex")]   SegmentIndex,
                }
            }

            internal static class TreeControllerObservation
            {
                internal static NSString Name => (NSString)"TreeControllerObservation";
            }
        }

        internal static class MenuItemTitle
        {
            internal static string CheckForUpdates = "Check for Updates...";
        }
    }
}
