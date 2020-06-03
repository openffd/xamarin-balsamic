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
    }

    sealed class LeadingContentListOutlineViewSeparator : NSObject, ILeadingContentListOutlineViewNodePayload
    {
        public LeadingContentListOutlineViewNodeType NodeType   => Separator;
        public NSImage Image                                    => null;
        public string Title                                     => string.Empty;
        public string Subtitle                                  => string.Empty;
    }

    internal interface ILeadingContentListOutlineViewNodePayload
    {
        LeadingContentListOutlineViewNodeType NodeType  { get; }
        NSImage Image                                   { get; }
        string Title                                    { get; }
        string Subtitle                                 { get; }
    }

    [Register("LeadingContentListOutlineViewNode")]
    sealed class LeadingContentListOutlineViewNode : Node
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
