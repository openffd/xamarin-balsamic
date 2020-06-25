using AppKit;
using System;
using static Balsamic.Font.Name;

namespace Balsamic.Views
{
    internal sealed partial class ResendCodeViewController
    {
        private static class String
       {
            internal static class ResendCode
            {
                internal static string Header => "Resend Code";
                internal static string Description => "Get a new verification code";
                internal static string ImageName => "NSRefreshTemplate";
            }

            internal static class UsePhoneNumber
            {
                internal static string Header => "Use Phone Number";
                internal static string Description => "Get a text or phone call with a code";
                internal static string ImageName => "NSTouchBarNewMessageTemplate";
            }

            internal static class MoreOptions
            {
                internal static string Header => "More Options";
                internal static string Description => "Confirm your phone number";
                internal static string ImageName => "NSTouchBarIconViewTemplate";
            }
        }

        private static class Image
        {
            internal static NSImage ResendCode => NSImage.ImageNamed(String.ResendCode.ImageName);
            internal static NSImage UsePhoneNumber => NSImage.ImageNamed(String.UsePhoneNumber.ImageName);
            internal static NSImage MoreOptions => NSImage.ImageNamed(String.MoreOptions.ImageName);
        }

        private static class Selector
        {
            internal static ObjCRuntime.Selector ResendCode_ => new ObjCRuntime.Selector("ResendCode:");
            internal static ObjCRuntime.Selector UsePhoneNumber_ => new ObjCRuntime.Selector("UsePhoneNumber:");
            internal static ObjCRuntime.Selector MoreOptions_ => new ObjCRuntime.Selector("MoreOptions:");
        }

        private static class Font
        {
            internal static NSFont Header => NSFont.FromFontName(HelveticaNeueBold.String(), 14);
            internal static NSFont Description => NSFont.FromFontName(HelveticaNeue.String(), 13);
        }

        private static class Color
        {
            internal static NSColor Header => NSColor.White;
            internal static NSColor Description => NSColor.FromWhite((nfloat)0.6, 1);
        }
    }
}
