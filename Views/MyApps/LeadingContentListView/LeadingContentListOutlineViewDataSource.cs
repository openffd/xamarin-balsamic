using AppKit;
using Balsamic.Models;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Balsamic.Views.MyApps
{
    class LeadingContentListOutlineViewDataSource : NSOutlineViewDataSource
    {
        public LeadingContentListOutlineViewDataSource() {}

        #region Overrides

        public override nint GetChildrenCount(NSOutlineView outlineView, NSObject item)
        {
            //if (item == null)
            //    return Nodes.Count;

            return ((item as NSTreeNode).RepresentedObject as LeadingContentListOutlineViewNode).Count;
        }

        public override NSObject GetChild(NSOutlineView outlineView, nint childIndex, NSObject item)
        {
            //if (item == null)
            //    return Nodes[(int)childIndex];

            return ((item as NSTreeNode).RepresentedObject as LeadingContentListOutlineViewNode)[(int)childIndex];
        }

        public override NSObject GetObjectValue(NSOutlineView outlineView, NSTableColumn tableColumn, NSObject item)
        {
            return new NSString(((item as NSTreeNode).RepresentedObject as LeadingContentListOutlineViewNode).Title);
        }

        public override bool ItemExpandable(NSOutlineView outlineView, NSObject item)
        {
            //if (item == null)
            //    return Nodes.First().HasChildren;

            return ((item as NSTreeNode).RepresentedObject as LeadingContentListOutlineViewNode).HasChildren;
        }

        public override void SortDescriptorsChanged(NSOutlineView outlineView, NSSortDescriptor[] oldDescriptors)
        {
            if (oldDescriptors.Length <= 0)
                return;

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
