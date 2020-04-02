using Foundation;

namespace Balsamic
{
    public static class Bundle
    {
        public static class Main
        {
            public enum DictionaryKey
            {
                CFBundleDisplayName = 0,
                CFBundleName = 1
            }
        }

        public static string DisplayName(this NSBundle bundle) => bundle.StringForInfoDictionary(Main.DictionaryKey.CFBundleDisplayName);

        public static string Name(this NSBundle bundle) => bundle.StringForInfoDictionary(Main.DictionaryKey.CFBundleName);

        private static string StringForInfoDictionary(this NSBundle bundle, Main.DictionaryKey key)
        {
            return bundle.ObjectForInfoDictionary(key.ToString()).ToString() ?? string.Empty;
        }
    }
}
