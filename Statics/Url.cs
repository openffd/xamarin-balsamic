using Foundation;

namespace Balsamic
{
    public static class Url
    {
        private static class String
        {
            public const string ForgotPassword = "iforgot.apple.com/password/verify/appleid";
            public const string HttpsScheme = "https";
        }
        
        public static readonly NSUrl ForgotPassword = new NSUrl($"{String.HttpsScheme}://{String.ForgotPassword}");
    }
}
