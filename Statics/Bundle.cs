using Foundation;

namespace Balsamic
{
    static class Bundle
    {
        static class Main
        {
            internal enum DictionaryKey
            {
                CFBundleDisplayName,
                CFBundleName,
                CFBundleShortVersionString,
                CFBundleVersion,
            }
        }

        internal static string DisplayName(this NSBundle bundle) => bundle.StringForInfoDictionary(Main.DictionaryKey.CFBundleDisplayName);
        internal static string Name(this NSBundle bundle) => bundle.StringForInfoDictionary(Main.DictionaryKey.CFBundleName);
        internal static string Version(this NSBundle bundle) => bundle.StringForInfoDictionary(Main.DictionaryKey.CFBundleShortVersionString);
        internal static string Build(this NSBundle bundle) => bundle.StringForInfoDictionary(Main.DictionaryKey.CFBundleVersion);

        static string StringForInfoDictionary(this NSBundle bundle, Main.DictionaryKey key)
        {
            return bundle.ObjectForInfoDictionary(key.ToString()).ToString() ?? string.Empty;
        }
    }
}
