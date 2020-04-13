using Foundation;

namespace Balsamic
{
    static class Url
    {
        static class String
        {
            internal const string ForgotPassword = "iforgot.apple.com/password/verify/appleid";
            internal const string HttpsScheme = "https";
        }
        
        internal static readonly NSUrl ForgotPassword = new NSUrl($"{String.HttpsScheme}://{String.ForgotPassword}");
    }
}
