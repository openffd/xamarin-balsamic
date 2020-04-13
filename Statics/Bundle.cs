using Foundation;

namespace Balsamic
{
    static class Bundle
    {
        internal static class Main
        {
            internal enum DictionaryKey
            {
                CFBundleDisplayName = 0,
                CFBundleName = 1
            }
        }

        internal static string DisplayName(this NSBundle bundle) => bundle.StringForInfoDictionary(Main.DictionaryKey.CFBundleDisplayName);

        internal static string Name(this NSBundle bundle) => bundle.StringForInfoDictionary(Main.DictionaryKey.CFBundleName);

        static string StringForInfoDictionary(this NSBundle bundle, Main.DictionaryKey key)
        {
            return bundle.ObjectForInfoDictionary(key.ToString()).ToString() ?? string.Empty;
        }
    }
}
