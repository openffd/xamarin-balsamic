using System;
using System.IO;
using System.IO.Compression;
using Claunia.PropertyList;
using GlobExpressions;

namespace Natukaship.Deliver
{
    public class IpaFileAnalyser
    {
        // Fetches the app identifier (e.g. com.facebook.Facebook) from the given ipa file.
        public static string FetchAppIdentifier(string path)
        {
            var plist = FetchInfoPlistFile(path);
            if (plist != null)
                return plist.ObjectForKey("CFBundleIdentifier").ToString();

            return null;
        }

        // Fetches the app version from the given ipa file.
        public static string FetchAppVersion(string path)
        {
            var plist = FetchInfoPlistFile(path);
            if (plist != null)
                return plist.ObjectForKey("CFBundleShortVersionString").ToString();

            return null;
        }

        //# Fetches the app platform from the given ipa file.
        public static string FetchAppPlatform(string path)
        {
            var plist = FetchInfoPlistFile(path);
            var platform = "ios";
            if (plist != null)
                platform = plist.ObjectForKey("DTPlatformName").ToString();
            if (platform == "iphoneos") // via https://github.com/fastlane/fastlane/issues/3484
                platform = "ios";

            return platform;
        }

        public static NSDictionary FetchInfoPlistFile(string path)
        {
            if (!File.Exists(path))
                Console.WriteLine($"Could not find file at path '{path}'");

            using (ZipArchive zip = ZipFile.Open(path, ZipArchiveMode.Read))
            {
                ZipArchiveEntry file = null;
                var globMatcher = new Glob("**/Payload/*.app/Info.plist");
                foreach (var entry in zip.Entries)
                {
                    if (globMatcher.IsMatch(entry.FullName))
                        file = entry;
                }

                if (file == null)
                    return null;

                // Creates a temporary directory with a unique name tagged with 'fastlane'
                // The directory is deleted automatically at the end of the block
                var tmpDir = Directory.CreateDirectory("fastlane");
                // The XML file has to be properly unpacked first
                var tmpPath = Path.Combine(tmpDir.FullName, "Info.plist");
                using (var fileStream = new FileStream(tmpPath, FileMode.Create))
                {
                    var stream = file.Open();
                    byte[] bytes;
                    using (var ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        bytes = ms.ToArray();
                    }
                    fileStream.Write(bytes, 0, bytes.Length);
                }

                var plistParsedDict = (NSDictionary)PropertyListParser.Parse(File.OpenRead(tmpPath));

                if (!string.IsNullOrEmpty(plistParsedDict.ObjectForKey("CFBundleIdentifier").ToString()) ||
                    !string.IsNullOrEmpty(plistParsedDict.ObjectForKey("CFBundleVersion").ToString()))
                    return plistParsedDict;
            }

            return null;
        }
    }
}
