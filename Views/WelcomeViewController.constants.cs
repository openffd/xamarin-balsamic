using AppKit;

namespace Balsamic.Views
{
    partial class WelcomeViewController
    {
        static class String
        {
            internal static string SigninInstructions => "Sign in with your Apple ID:";
            internal static string ForgotPasswordButtonTitle => "Forgot password?";

            internal static class ImageName
            {
                internal static string AppLogo => "NSUserAccounts";
                internal static string GoForward => "NSGoForwardTemplate";
            }

            internal static class Placeholder
            {
                internal static string AppleID => "Apple ID";
                internal static string Password => "Password";
            }
        }

        static class Image
        {
            internal static NSImage AppLogo => NSImage.ImageNamed(String.ImageName.AppLogo);
            internal static NSImage GoForward => NSImage.ImageNamed(String.ImageName.GoForward);
        }
    }
}
