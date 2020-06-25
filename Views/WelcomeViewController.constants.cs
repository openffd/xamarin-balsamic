using AppKit;
using static Balsamic.Font.Name;
using static Foundation.NSBundle;

namespace Balsamic.Views
{
    internal partial class WelcomeViewController
    {
        private static class String
        {
            internal static string AppBuildVersion => $"Version {MainBundle.GetVersion()} ({MainBundle.GetBuild()})";
            internal static string SigninInstructions => "Sign in with your Apple ID:";
            internal static string ForgotPasswordButtonTitle => "Forgot your password?";

            internal static class Placeholder
            {
                internal static string AppleID => "Apple ID";
                internal static string Password => "Password";
            }
        }

        private static class Image
        {
            internal static NSImage AppLogo => Balsamic.Image.AppLogo;
            internal static NSImage GoForward => Balsamic.Image.GoForwardTemplate;
            internal static NSImage Close => Balsamic.Image.StatusUnavailable;
        }

        private static class Font
        {
            internal static NSFont WelcomeTextField => NSFont.FromFontName(SFProDisplayBold.String(), 34);
            internal static NSFont VersionTextField => NSFont.FromFontName(SFProDisplayRegular.String(), 10);
            internal static NSFont SigninHeaderTextField => NSFont.FromFontName(HelveticaNeueMedium.String(), 14);
            internal static NSFont AppleIDTextField => NSFont.FromFontName(HelveticaNeueLight.String(), 16);
            internal static NSFont PasswordTextField => NSFont.FromFontName(HelveticaNeueLight.String(), 16);
            internal static NSFont ForgotPasswordButton => NSFont.FromFontName(HelveticaNeueMedium.String(), 13);
        }
    }
}
