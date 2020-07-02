using System.Collections.Generic;

namespace Natukaship
{
    public class DeliverOptions
    {
        public string appIdentifier { get; set; }
        public string username { get; set; }
        // Path to .ipa file
        public string ipa { get; set; }
        // Path to .pkg file
        public string pkg { get; set; }
        // Optional, usually automatically detected. Specify the version that should be created / edited on App Store Connect
        public string appVersion { get; set; }
        // In the case if deliver uploads your application to App Store Connect it will automatically update "Prepare for submission"
        //   app version(which could be found on App Store Connect->My Apps->App Store page)
        // The option allows uploading your app without updating "Prepare for submission" version.
        // This could be useful in the case if you are generating a lot of uploads while not submitting the latest build for Apple review.
        public bool skipAppVersionUpdate { get; set; } = false;
        // Automatically submit the app for review after uploading metadata/binary. This will select the latest build.
        public bool submitForReview { get; set; } = false;
        public int priceTier { get; set; }
        public string platform { get; set; } = "ios";
        public bool skipScreenshots { get; set; } = true;
        public bool skipMetadata { get; set; } = true;
        public bool skipBinaryUpload { get; set; } = false;
        public string buildNumber { get; set; }
        public bool rejectIfPossible { get; set; }
        public string itcProvider { get; set; }

        private Application _app = null;
        public Application app
        {
            get
            {
                if (_app != null)
                    return _app;

                _app = Application.Find(appIdentifier);

                if (_app == null)
                    throw new System.Exception($"Can't find app with identifier: {appIdentifier}.");

                return _app;
            }

            set
            {
                _app = value;
            }
        }

        public string screenshotsPath { get; internal set; }
        public string metadataPath { get; internal set; }
        public List<object> languages { get; internal set; }
    }
}
