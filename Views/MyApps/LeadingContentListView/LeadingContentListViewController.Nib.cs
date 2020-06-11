using AppKit;

namespace Balsamic.Views.MyApps
{
    internal sealed partial class LeadingContentListViewController
    {
        private static class ApplicationDetailTableCellView
        {
            internal static string Identifier = "ApplicationDetail";
            internal static NSNib Nib => new NSNib("ApplicationDetailTableCellView", null);
        }
    }
}
