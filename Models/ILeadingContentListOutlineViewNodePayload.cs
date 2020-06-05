using AppKit;
using Foundation;
using static Balsamic.Models.LeadingContentListOutlineViewNodeType;

namespace Balsamic.Models
{
    internal interface ILeadingContentListOutlineViewNodePayload
    {
        LeadingContentListOutlineViewNodeType NodeType { get; }
        NSImage Image { get; }
        string Title { get; }
        string Subtitle { get; }
    }

    internal sealed class LeadingContentListOutlineViewSeparator : NSObject, ILeadingContentListOutlineViewNodePayload
    {
        public LeadingContentListOutlineViewNodeType NodeType => Separator;
        public NSImage Image => null;
        public string Title => string.Empty;
        public string Subtitle => string.Empty;
    }
}
