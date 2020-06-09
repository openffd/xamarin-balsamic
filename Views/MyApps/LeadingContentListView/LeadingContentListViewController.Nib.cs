using AppKit;

namespace Balsamic.Views.MyApps
{
    internal sealed partial class LeadingContentListViewController
    {
        private static class Nib
        {
            internal static NSNib ApplicationDetailTableCellView => new NSNib("ApplicationDetailTableCellView", null);
        }
    }
}
