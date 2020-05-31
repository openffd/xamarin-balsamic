using AppKit;
using System;
using System.ComponentModel;
using static AppKit.NSColor;
using static Balsamic.Enums.ApplicationVersion.Status;

namespace Balsamic.Enums
{
    static class ApplicationVersion
    {
        internal enum Status
        {
            [Description("Developer Removed from Sale")]    DeveloperRemovedFromSale,
            [Description("Prepare for Submission")]         PrepareForSubmission,
            [Description("Ready for Sale")]                 ReadyForSale,
            [Description("Unknown")]                        Unknown,
        }

        internal static NSColor StatusColor(this Status status)
        {
            return status switch
            {
                DeveloperRemovedFromSale    => Red,
                PrepareForSubmission        => Yellow,
                ReadyForSale                => Green,
                _                           => White,
            };
        }
	}
}
