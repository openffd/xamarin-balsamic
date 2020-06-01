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
            var node = (item as NSTreeNode).RepresentedObject as LeadingContentListOutlineViewNode;
            tableCellView.TextField.StringValue = node.Title;
            tableCellView.ImageView.Image = node.Image;
            return tableCellView;
        }

        public override nfloat GetRowHeight(NSOutlineView outlineView, NSObject item)
        {
            var height = outlineView.RowHeight;
            return (item as NSTreeNode).RepresentedObject is LeadingContentListOutlineViewNode node && node.IsSeparator ? 8 : height;
        }

        public override bool ShouldShowOutlineCell(NSOutlineView outlineView, NSObject item)
        {
            return (item as NSTreeNode).RepresentedObject is LeadingContentListOutlineViewNode node && node.HasChildren;
        }

        public override bool IsGroupItem(NSOutlineView outlineView, NSObject item) => false;

        public override bool ShouldSelectItem(NSOutlineView outlineView, NSObject item) => true;

        public override bool ShouldEditTableColumn(NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item) => false;
    }
}
