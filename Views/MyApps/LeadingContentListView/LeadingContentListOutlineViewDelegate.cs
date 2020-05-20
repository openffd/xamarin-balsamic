using System;
using AppKit;
using Balsamic.Models;
using Foundation;

namespace Balsamic.Views.MyApps
{
    public class LeadingContentListOutlineViewDelegate : NSOutlineViewDelegate
    {
        public LeadingContentListOutlineViewDelegate() {}

        public override NSCell GetCell(NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
        {
            nint row = outlineView.RowForItem(item); 
            return tableColumn.DataCellForRow(row);
        }

        public override NSView GetView(NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
        {
            NSTableCellView tableCellView = (NSTableCellView)outlineView.MakeView("DataCell", this);
            if (tableCellView == null)
            {
                tableCellView = new NSTableCellView();
            }
            tableCellView.TextField.StringValue = (item as Node).Title;
            return tableCellView;
        }

        public override bool IsGroupItem(NSOutlineView outlineView, NSObject item) => (item as Node).HasChildren;

        public override bool ShouldSelectItem(NSOutlineView outlineView, NSObject item)
        {
            return outlineView.GetParent(item) != null;
        }

        public override bool ShouldEditTableColumn(NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
        {
            return false;
        }
    }
}
