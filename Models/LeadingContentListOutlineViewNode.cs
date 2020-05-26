using AppKit;
using System.Collections.Generic;
using static Balsamic.Models.LeadingContentListOutlineViewNodeType;

namespace Balsamic.Models
{
    enum LeadingContentListOutlineViewNodeType
    {
        AppleDevAccount,
        ApplicationDetail,
        ApplicationVersion,
        Separator,
        Unknown,
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

        internal bool IsAppleDevAccount     => NodeType == AppleDevAccount;
        internal bool IsApplicationDetail   => NodeType == ApplicationDetail;
        internal bool IsApplicationVersion  => NodeType == ApplicationVersion;
        internal bool IsSeparator           => NodeType == Separator;

        internal bool Leaf => IsApplicationVersion || IsSeparator;
    }
}
