using AppKit;
using Foundation;
using System;

namespace Balsamic.Views.MyApps
{
    class LeadingContentListOutlineViewDataSource : NSOutlineViewDataSource
    {
        public LeadingContentListOutlineViewDataSource() {}

        #region Overrides

        public override nint GetChildrenCount(NSOutlineView outlineView, NSObject item)
        {
            return item.GetOutlineViewNode().Count;
        }

        public override NSObject GetChild(NSOutlineView outlineView, nint childIndex, NSObject item)
        {
            return item.GetOutlineViewNode()[(int)childIndex];
        }

        public override NSObject GetObjectValue(NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
        {
            return new NSString(item.GetOutlineViewNode().Title);
        }

        public override bool ItemExpandable(NSOutlineView outlineView, NSObject item)
        {
            return item.GetOutlineViewNode().HasChildren;
        }

        public override void SortDescriptorsChanged(NSOutlineView outlineView, NSSortDescriptor[] oldDescriptors)
        {
            if (oldDescriptors.Length > 0)
                outlineView.ReloadData();
        }

        #endregion

        //internal LeadingContentListOutlineViewNode NodeForRow(int row)
        //{
        //    int index = 0;
        //    foreach (LeadingContentListOutlineViewNode node in Nodes)
        //    {
        //        if (row >= index && row <= (index + node.Count))
        //            return (LeadingContentListOutlineViewNode)node[row - index - 1];

        //        index += node.Count + 1;
        //    }
        //    return null;
        //}
    }
}
