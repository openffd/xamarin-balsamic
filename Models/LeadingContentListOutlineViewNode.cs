using AppKit;
using System.Collections.Generic;
using static Balsamic.Models.LeadingContentListOutlineViewNodeType;

namespace Balsamic.Models
{
    enum LeadingContentListOutlineViewNodeType
    {
        Account,
        Application,
        Version,
        Separator,
        Unknown
    }

    interface ILeadingContentListOutlineViewNodePayload
    {
        LeadingContentListOutlineViewNodeType NodeType { get; }
        NSImage Image { get; }
        string Title { get; }
        string Description { get; }
    }

    sealed class LeadingContentListOutlineViewNode : Node
    {
        internal LeadingContentListOutlineViewNodeType NodeType { get; set; } = Unknown;

        internal List<Node> Children => _nodes;

        internal bool IsAccount     => NodeType == Account;
        internal bool IsApplication => NodeType == Application;
        internal bool IsVersion     => NodeType == Version;
        internal bool IsSeparator   => NodeType == Separator;

        internal bool Leaf => IsVersion || IsSeparator;
    }
}
