namespace Balsamic
{
    public static class NSUrl
    {
        private static class String
        {
            public const string ForgotPassword = "iforgot.apple.com/password/verify/appleid";
            public const string HttpsScheme = "https";
        }
        
        public static Foundation.NSUrl ForgotPassword = new Foundation.NSUrl($"{String.HttpsScheme}://{String.ForgotPassword}");
    }
}
