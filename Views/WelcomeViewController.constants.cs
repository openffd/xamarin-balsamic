using AppKit;

namespace Balsamic.Views
{
    partial class WelcomeViewController
    {
        static class String
        {
            internal static string SigninInstructions => "Sign in with your Apple ID:";
            internal static string ForgotPasswordButtonTitle => "Forgot password?";

            internal static class Placeholder
            {
                internal static string AppleID => "Apple ID";
                internal static string Password => "Password";
            }
        }

        static class Image
        {
            internal static NSImage AppLogo => Balsamic.Image.UserAccounts;
            internal static NSImage GoForward => Balsamic.Image.GoForwardTemplate;
            internal static NSImage Close => Balsamic.Image.StatusUnavailable;
        }
    }
}
