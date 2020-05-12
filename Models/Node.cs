using AppKit;
using Foundation;
using System.Collections.Generic;

namespace Balsamic.Models
{
    sealed class Node : NSObject 
    {
        internal List<Node> Nodes = new List<Node>();
        internal NSImage Image { get; set; }
        internal string Title { get; set; } = string.Empty;
        internal new string Description { get; set; } = string.Empty;
        internal bool HasChildren => Nodes.Count > 0;

        public Node() {}

        public Node(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}
