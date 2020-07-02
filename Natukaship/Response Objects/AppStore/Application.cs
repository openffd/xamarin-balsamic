using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Natukaship
{
    public class Application : ApplicationRaw
    {
        public bool watchOnly { get; set; }
        public bool watchCombo { get; set; }

        public string appleId => adamId;
        public long lastModified => lastModifiedDate;
        public string appIconPreviewUrl => iconUrl;

        public ApplicationRaw _rawData { get; set; }
        public ApplicationRaw RawData
        {
            get
            {
                if (_rawData != null)
                    return _rawData;

                _rawData = new ApplicationRaw
                {
                    issuesCount = issuesCount,
                    priceTier = priceTier,
                    bundleId = bundleId,
                    lastModifiedDate = lastModifiedDate,
                    appType = appType,
                    preOrderEndDate = preOrderEndDate,
                    buildVersionSets = buildVersionSets,
                    adamId = adamId,
                    versionSets = versionSets,
                    type = type,
                    iconUrl = iconUrl,
                    isAAG = isAAG,
                    name = name,
                    vendorId = vendorId,
                };

                return _rawData;
            }
        }

        // @return (List<Application>) Returns all apps available for this account
        public static List<Application> All()
        {
            return Globals.TunesClient.Applications();
        }

        // @return (Application) Returns the application matching the parameter
        //   as either the App ID or the bundle identifier
        public static Application Find(string identifier, bool isMac = false)
        {
            string[] supposedPlatform = null;
            if (isMac)
                supposedPlatform = new string[] { "osx" };
            else
                supposedPlatform = new string[] { "ios", "appletvos" };

            Application result = All().Find(app =>
            {
                return (!string.IsNullOrEmpty(app.appleId) && string.Equals(app.appleId, identifier, StringComparison.OrdinalIgnoreCase) == true ||
                    (!string.IsNullOrEmpty(app.bundleId) && string.Equals(app.bundleId, identifier, StringComparison.OrdinalIgnoreCase) == true)) &&
                    app.versionSets.Any(versionSet => supposedPlatform.Contains(versionSet.platform));
            });

            return result;
        }

        // Creates a new application on App Store Connect
        // @param name (String): The name of your app as it will appear on the App Store.
        //   This can't be longer than 255 characters.
        // @param primaryLanguage (String): If localized app information isn't available in an
        //   App Store territory, the information from your primary language will be used instead.
        // @param version *DEPRECATED: Use `ensure_version!` method instead*
        //   (String): The version number is shown on the App Store and should match the one you used in Xcode.
        // @param sku (String): A unique ID for your app that is not visible on the App Store.
        // @param bundleId (String): The bundle ID must match the one you used in Xcode. It
        //   can't be changed after you submit your first build.
        // @param companyName (String): The company name or developer name to display on the App Store for your apps.
        // It cannot be changed after you create your first app.
        // @param platform (String): Platform one of (ios,osx)
        //  should it be an ios or an osx app
        public static CreateApplication Create(string name = null,
                                  string primaryLanguage = null,
                                  string version = null,
                                  string sku = null,
                                  string bundleId = null,
                                  string bundleIdSuffix = null,
                                  string companyName = null,
                                  string platform = "ios",
                                  string itunesConnectUsers = null)
        {
            if (version != null)
                Console.WriteLine("The `version` parameter is deprecated. Use `Application.ensure_version!` method instead");

            var createdApp = Globals.TunesClient.CreateApplication(name: name,
                                                  primaryLanguage: primaryLanguage,
                                                  sku: sku,
                                                  bundleId: bundleId,
                                                  bundleIdSuffix: bundleIdSuffix,
                                                  companyName: companyName,
                                                  platform: platform,
                                                  itunesConnectUsers: itunesConnectUsers);

            return createdApp;
        }

        public static BundleData AvailableBundleIds(string platform = "ios")
        {
            return Globals.TunesClient.GetAvailableBundleIds(platform);
        }

        //////////////////////////
        /// @!group Getting information
        //////////////////////////

        public VersionSet VersionSetForPlatform(string platform)
        {
            foreach (var versionSet in versionSets)
            {
                if (versionSet.platformString == platform)
                    return versionSet;
            }
            return null;
        }

        // @return (Natukaship.AppVersion) Receive the version that is currently live on the
        //  App Store. You can't modify all values there, so be careful.
        public AppVersion LiveVersion(string platform = null)
        {
            return AppVersion.Find(this, appleId, true);
        }

        // @return (Natukaship.AppVersion) Receive the version that can fully be edited
        public AppVersion EditVersion(string platform = null)
        {
            return AppVersion.Find(this, appleId, false);
        }

        // @return (Natukaship.AppVersion) This will return the `edit_version` if available
        //   and fallback to the `live_version`. Use this to just access the latest data
        public AppVersion LatestVersion(string platform = null)
        {
            var editVersion = EditVersion(platform);
            var liveVersion = LiveVersion(platform);

            if (editVersion != null)
                return editVersion;
            else
                return liveVersion;
        }

        // @return (String) An URL to this specific resource. You can enter this URL into your browser
        public string url => $"https://appstoreconnect.apple.com/WebObjects/iTunesConnect.woa/ra/ng/app/{appleId}";

        // @return (Hash) Contains the reason for rejection.
        //  if everything is alright, the result will be
        //  `{"sectionErrorKeys"=>[], "sectionInfoKeys"=>[], "sectionWarningKeys"=>[], "replyConstraints"=>{"minLength"=>1, "maxLength"=>4000}, "appNotes"=>{"threads"=>[]}, "betaNotes"=>{"threads"=>[]}, "appMessages"=>{"threads"=>[]}}`
        public ResolutionData ResolutionCenter()
        {
            return Globals.TunesClient.GetResolutionCenter(appleId);
        }

        public ResolutionData ReplyResolutionCenter(string appId, string threadId, string versionId, string versionNumber, string from, string messageBody)
        {
            return Globals.TunesClient.PostResolutionCenter(appId, threadId, versionId, versionNumber, from, messageBody);
        }

        public string[] Platforms
        {
            get
            {
                string[] platforms = new string[] { };
                versionSets.ForEach(versionSet =>
                {
                    platforms.Append(versionSet.platform);
                });
                return platforms;
            }
        }

        public string Type()
        {
            if (versionSets == null)
                throw new System.Exception("The application has no version sets and Natukaship does not know what to do here.");

            if (versionSets.Count == 1)
                _ = versionSets[0].platform;

            var platform = AppVersionCommon.FindPlatform(versionSets);
            return platform.type;
        }

        //// kept for backward compatibility
        //// tries to guess the platform of the currently submitted apps
        //// note that as ITC now supports multiple app types, this might break
        //// if your app supports more than one
        public string Platform()
        {
            if (versionSets == null)
                throw new Exception("The application has no version sets and Natukaship does not know what to do here.");

            if (versionSets.Count == 1)
                _ = versionSets[0].platform;
            else if (Platforms.Contains("ios") || Platforms.Contains("appletvos"))
                _ = "ios";

            return AppVersionCommon.FindPlatform(RawData.versionSets).platformString;
        }

        public AppDetail Details()
        {
            AppDetail attrs = Globals.TunesClient.AppDetails(appleId);
            attrs.application = this;

            return AppDetail.Factory(attrs);
        }

        public List<AppVersionHistory> VersionsHistory()
        {
            EnsureNotABundle();
            var versions = Globals.TunesClient.VersionsHistory(appleId);
            versions.ForEach((version) =>
            {
                version.application = this;
            });

            return versions;
        }

        //////////////////////////
        /// @!group Modifying
        //////////////////////////

        // TODO: Finish this method
        // Create a new version of your app
        // Since we have stored the outdated rawData, we need to refresh this object
        // otherwise `edit_version` will return nil
        public void CreateVersion(string versionNumber, string platform = "ios")
        {
            if (EditVersion() != null)
                throw new Exception("Cannot create a new version for this app as there already is an `edit_version` available");

            Globals.TunesClient.CreateVersion(appleId, versionNumber);

            // Future: implemented -reload method
        }

        // Will make sure the current edit_version matches the given version number
        // This will either create a new version or change the version number
        // from an existing version
        // @return (Bool) Was something changed?
        public bool EnsureVersion(string versionNumber, string platform = "ios")
        {
            var editVersion = EditVersion(platform: platform);
            if (editVersion != null)
            {
                if (editVersion.version.value != versionNumber)
                {
                    // Update an existing version
                    editVersion.version.value = versionNumber;
                    editVersion.Save();
                    return true;
                }
                return false;
            }
            else
            {
                CreateVersion(versionNumber: versionNumber, platform: platform);
                return true;
            }
        }

        public bool RejectVersionIfPossible()
        {
            var editVersion = EditVersion();
            bool canReject = editVersion.canRejectVersion;

            if (canReject)
                Globals.TunesClient.Reject(appleId, editVersion.versionId);

            return canReject;
        }

        // set the price tier. This method doesn't require `save` to be called
        public void UpdatePriceTier(string priceTier)
        {
            Globals.TunesClient.UpdatePriceTier(appleId, priceTier);
        }

        // The current price tier
        public void PriceTier()
        {
            Globals.TunesClient.PriceTier(appleId);
        }

        // set the availability. This method doesn't require `save` to be called
        public Availability UppdateAvailability(Availability availability)
        {
            return Globals.TunesClient.UpdateAvailability(appleId, availability);
        }

        // The current availability.
        public Availability Availability()
        {
            return Globals.TunesClient.Availability(appleId);
        }

        //////////////////////////
        /// @!group Builds
        //////////////////////////

        // TestFlight: A reference to all the build trains
        // @return [Hash] a hash, the version number and platform being the key
        public void BuildTrains(string platform = "ios")
        {
            //TestFlight::BuildTrains.all(app_id: self.apple_id, platform: platform || self.platform)
        }

        // private to module
        internal void EnsureNotABundle()
        {
            // we only support applications
            if (type == "BUNDLE")
                throw new Exception("We do not support BUNDLE types right now");
        }

        public static implicit operator JToken(Application v)
        {
            throw new NotImplementedException();
        }
    }

    public class ApplicationRaw
    {
        public int issuesCount { get; set; }
        public string priceTier { get; set; }
        public string bundleId { get; set; }
        public long lastModifiedDate { get; set; }
        public object appType { get; set; }
        public string preOrderEndDate { get; set; }
        public List<object> buildVersionSets { get; set; }
        public string adamId { get; set; }
        public List<VersionSet> versionSets { get; set; }
        public string type { get; set; }
        public string iconUrl { get; set; }
        public bool isAAG { get; set; }
        public string name { get; set; }
        public string vendorId { get; set; }
    }
}
