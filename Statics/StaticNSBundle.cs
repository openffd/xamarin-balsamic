using Foundation;

namespace Balsamic
{
    public static class StaticNSBundle
    {
        public enum MainBundleKey
        {
            CFBundleDisplayName = 0,
            CFBundleName = 1
        }

        public static string DisplayName(this NSBundle bundle) => bundle.StringForInfoDictionary(MainBundleKey.CFBundleDisplayName);

        public static string Name(this NSBundle bundle) => bundle.StringForInfoDictionary(MainBundleKey.CFBundleName);

        private static string StringForInfoDictionary(this NSBundle bundle, MainBundleKey key)
        {
            return bundle.ObjectForInfoDictionary(key.ToString()).ToString() ?? string.Empty;
        }
    }
}
