using System.Collections.Generic;
using System.Linq;

namespace Natukaship
{
    public class AppVersionCommon
    {
        public static string FindVersionId(VersionSet platform, bool isLive)
        {
            if (isLive)
            {
                var version = platform?.deliverableVersion;

                if (version == null)
                    return null;

                return version.id;
            }
            else
            {
                var version = platform.inFlightVersion;

                if (version == null)
                    return null;

                return version.id;
            }
        }

        public static VersionSet FindPlatform(List<VersionSet> versions, string searchPlatform = null)
        {
            // We only support platforms that exist ATM

            VersionSet platform = versions.Find(version => new string[] { "ios", "osx", "appletvos" }.Contains(version.platformString));

            if (platform == null)
                throw new System.Exception("Could not find platform 'ios', 'osx' or 'appletvos'");

            // If your app has versions for both iOS and tvOS we will default to returning the iOS version for now.
            // This is intentional as we need to do more work to support apps that have hybrid versions.
            if (versions.Count > 1 && searchPlatform != null)
                platform = versions.Find(version => version.platformString == "ios");
            else if (searchPlatform != null)
                platform = versions.Find(version => version.platformString == searchPlatform);

            return platform;
        }
    }
}
