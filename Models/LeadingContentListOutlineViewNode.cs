using AppKit;
using Foundation;
using System.Collections.Generic;
using static Balsamic.Models.LeadingContentListOutlineViewNodeType;

namespace Balsamic.Models
{
    internal enum LeadingContentListOutlineViewNodeType
    {
        AppleDevAccount,
        ApplicationDetail,
        ApplicationVersion,
        Separator,
        Unknown,
    }

    internal interface ILeadingContentListOutlineViewNodePayload
    {
        LeadingContentListOutlineViewNodeType NodeType { get; }
        NSImage Image { get; }
        string Title { get; }
        string Description { get; }
    }

    sealed class LeadingContentListOutlineViewNode : Node
    {
        internal ILeadingContentListOutlineViewNodePayload Payload { get; set; }

        public LeadingContentListOutlineViewNode(ILeadingContentListOutlineViewNodePayload payload)
        {
            Payload = payload;
        }

        internal List<Node> Children => _nodes;

        internal string Title => Payload.Title;

        internal LeadingContentListOutlineViewNodeType NodeType => Payload.NodeType;
        internal bool IsAppleDevAccount     => NodeType == AppleDevAccount;
        internal bool IsApplicationDetail   => NodeType == ApplicationDetail;
        internal bool IsApplicationVersion  => NodeType == ApplicationVersion;
        internal bool IsSeparator           => NodeType == Separator;

        internal bool Leaf => IsApplicationVersion || IsSeparator;
    }
}
