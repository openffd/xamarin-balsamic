using AppKit;
using Foundation;

namespace Balsamic.Views.MyApps
{
    public class LeadingContentListOutlineViewDelegate : NSOutlineViewDelegate
    {
        private readonly LeadingContentListOutlineViewDataSource dataSource;

        public LeadingContentListOutlineViewDelegate(LeadingContentListOutlineViewDataSource dataSource)
        {
            this.dataSource = dataSource;
        }

        public override NSView GetView(NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
        {
            NSTableCellView tableCellView = (NSTableCellView)outlineView.MakeView(tableColumn.Title, this);
            if (tableCellView == null)
            {
                tableCellView = new NSTableCellView();

            }
            return tableCellView;
        }

        public override bool ShouldSelectItem(NSOutlineView outlineView, NSObject item)
        {
            return true;
        }
    }
}
