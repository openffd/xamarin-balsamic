using Foundation;

namespace Balsamic
{
    internal static class Bundle
    {
        private static class Main
        {
            internal enum DictionaryKey
            {
                CFBundleDisplayName,
                CFBundleName,
                CFBundleShortVersionString,
                CFBundleVersion,
            }
        }

        internal static string GetDisplayName(this NSBundle bundle)
        {
            return bundle.StringForInfoDictionary(Main.DictionaryKey.CFBundleDisplayName);
        }

        internal static string GetName(this NSBundle bundle)
        {
            return bundle.StringForInfoDictionary(Main.DictionaryKey.CFBundleName);
        }

        internal static string GetVersion(this NSBundle bundle)
        {
            return bundle.StringForInfoDictionary(Main.DictionaryKey.CFBundleShortVersionString);
        }

        internal static string GetBuild(this NSBundle bundle)
        {
            return bundle.StringForInfoDictionary(Main.DictionaryKey.CFBundleVersion);
        }

        private static string StringForInfoDictionary(this NSBundle bundle, Main.DictionaryKey key)
        {
            return bundle.ObjectForInfoDictionary(key.ToString()).ToString() ?? string.Empty;
        }
    }
}
