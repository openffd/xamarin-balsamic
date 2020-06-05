using AppKit;
using Foundation;
using System;
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
    }

    internal static class LeadingContentListOutlineViewNodeTypeHelper
    {
        internal static string GetOutlineViewTableCellViewIdentifier(this LeadingContentListOutlineViewNodeType nodeType)
        {
            return nodeType switch
            {
                AppleDevAccount     => "AppleDevAccount",
                ApplicationDetail   => "ApplicationDetail",
                ApplicationVersion  => "ApplicationVersion",
                Separator           => "Separator",
                _                   => string.Empty,
            };
        }

        internal static nfloat GetOutlineViewRowHeight(this LeadingContentListOutlineViewNodeType nodeType)
        {
            return nodeType switch
            {
                AppleDevAccount     => 30,
                ApplicationDetail   => 44,
                ApplicationVersion  => 22,
                Separator           => 16,
                _                   => 0,
            };
        }

        internal static bool GetOutlineViewRowSelectability(this LeadingContentListOutlineViewNodeType nodeType)
        {
            return nodeType switch
            {
                Separator => false,
                _ => true,
            };
        }
    }

    [Register("LeadingContentListOutlineViewNode")]
    internal sealed class LeadingContentListOutlineViewNode : Node
    {
        internal ILeadingContentListOutlineViewNodePayload Payload { get; set; }

        public LeadingContentListOutlineViewNode(ILeadingContentListOutlineViewNodePayload payload)
        {
            Payload = payload;
        }

        internal List<Node> Children => _nodes;

        internal LeadingContentListOutlineViewNodeType NodeType => Payload.NodeType;
        internal NSImage Image                                  => Payload.Image;
        internal string Title                                   => Payload.Title;
        internal string Subtitle                                => Payload.Subtitle;

        [Export("Children")]
        internal NSArray ChildrenArray => NSArray.FromNSObjects(_nodes.ToArray());

        internal bool IsAppleDevAccount     => NodeType == AppleDevAccount;
        internal bool IsApplicationDetail   => NodeType == ApplicationDetail;
        internal bool IsApplicationVersion  => NodeType == ApplicationVersion;
        internal bool IsSeparator           => NodeType == Separator;

        [Export("Leaf")]
        internal bool Leaf => IsApplicationVersion || IsSeparator;
    }
}
