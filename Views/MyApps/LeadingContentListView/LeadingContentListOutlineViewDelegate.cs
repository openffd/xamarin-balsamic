using AppKit;
using Balsamic.Models;
using Balsamic.Views.MyApps.LeadingContentListViewControllerOutlineView;
using Foundation;
using System;
using static Balsamic.Models.LeadingContentListOutlineViewNodeType;

namespace Balsamic.Views.MyApps
{
    internal static class LeadingContentListOutlineViewNodeHelper
    {
        internal static LeadingContentListOutlineViewNode GetOutlineViewNode(this NSObject item)
        {
            return (LeadingContentListOutlineViewNode)((NSTreeNode)item).RepresentedObject;
        }
    }

    class LeadingContentListOutlineViewDelegate : NSOutlineViewDelegate
    {
        public override void DidAddRowView(NSOutlineView outlineView, NSTableRowView rowView, nint row)
        {
            Console.WriteLine($"Row: {row}");
        }

        public override NSCell GetCell(NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
        {
            nint row = outlineView.RowForItem(item); 
            return tableColumn.DataCellForRow(row);
        }

        public override nfloat GetRowHeight(NSOutlineView outlineView, NSObject item)
        {
            LeadingContentListOutlineViewNode node = item.GetOutlineViewNode();
            return node.NodeType.GetOutlineViewRowHeight();
        }

        public override NSView GetView(NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
        {
            LeadingContentListOutlineViewNode node = item.GetOutlineViewNode();
            return node.NodeType switch
            {
                ApplicationDetail   => SetupApplicationDetailCellView(outlineView, node),
                _                   => SetupDefaultTableCellView(outlineView, node),
            };
        }

        public override bool IsGroupItem(NSOutlineView outlineView, NSObject item)
        {
            return false;
        }

        public override bool ShouldEditTableColumn(NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
        {
            return false;
        }

        public override bool ShouldSelectItem(NSOutlineView outlineView, NSObject item)
        {
            LeadingContentListOutlineViewNode node = item.GetOutlineViewNode();
            return node.NodeType.GetOutlineViewRowSelectability();
        }

        public override bool ShouldShowOutlineCell(NSOutlineView outlineView, NSObject item)
        {
            return item.GetOutlineViewNode().HasChildren;
        }

        #region Private Methods

        private NSView SetupApplicationDetailCellView(NSOutlineView outlineView, LeadingContentListOutlineViewNode node)
        {
            string identifier = node.NodeType.GetOutlineViewTableCellViewIdentifier();
            ApplicationDetailTableCellView tableCellView = (ApplicationDetailTableCellView)outlineView.MakeView(identifier, this);
            if (tableCellView == null)
                tableCellView = new ApplicationDetailTableCellView();
            tableCellView.ImageView.Image = node.Image;
            tableCellView.AppNameLabelTextField.StringValue = node.Title;
            tableCellView.BundleIdentifierLabelTextField.StringValue = node.Subtitle;
            return tableCellView;
        }

        private NSView SetupDefaultTableCellView(NSOutlineView outlineView, LeadingContentListOutlineViewNode node)
        {
            string identifier = node.NodeType.GetOutlineViewTableCellViewIdentifier();
            NSTableCellView tableCellView = (NSTableCellView)outlineView.MakeView(identifier, this);
            if (tableCellView == null)
                tableCellView = new NSTableCellView();
            tableCellView.TextField.StringValue = node.Title;
            tableCellView.ImageView.Image = node.Image;
            tableCellView.ImageView.Rounded();
            return tableCellView;
        }

        #endregion
    }
}
