using System;
using System.Collections.Generic;
using Natukaship.Deliver;

namespace Natukaship
{
    public class DeliverRunner
    {
        public dynamic skipObjectDetection { get; set; }
        public DeliverOptions options { get; set; }

        public DeliverRunner(DeliverOptions opts, dynamic skipAutoDetection = null)
        {
            options = opts;

            var detectValues = new DetectValues();
            options = detectValues.Run(options, skipAutoDetection);
        }

        public void Run()
        {
            if (options.appVersion.Length > 0 && !options.skipAppVersionUpdate)
                VerifyVersion();

            UploadMetadata();

            bool hasBinary = false;
            if (!string.IsNullOrEmpty(options.ipa) || !string.IsNullOrEmpty(options.pkg))
                hasBinary = true;

            if (!options.skipBinaryUpload && !string.IsNullOrEmpty(options.buildNumber) && hasBinary)
                UploadBinary();

            if (!options.skipBinaryUpload)
                Console.WriteLine("Finished the upload to App Store Connect");

            if (options.rejectIfPossible)
                RejectVersionIfPossible();

            if (options.submitForReview)
                SubmitForReview();
        }

        public void SubmitForReview()
        {
            // SubmitForReview.new.submit!(options);
        }

        // Make sure the version on App Store Connect matches the one in the ipa
        // If not, the new version will automatically be created
        public void VerifyVersion()
        {
            string appVersion = options.appVersion;
            Console.WriteLine($"Making sure the latest version on App Store Connect matches '{appVersion}' from the ipa file...");

            bool changed = options.app.EnsureVersion(appVersion, platform: options.platform);

            if (changed)
                Console.WriteLine($"Successfully set the version to '{appVersion}'");
            else
                Console.WriteLine($"'{appVersion}' is the latest version on App Store Connect");
        }

        // Upload all metadata, screenshots, pricing information, etc. to App Store Connect
        public void UploadMetadata()
        {
            //var uploadMetadata = new UploadMetadata();
            //var uploadScreenshots = new UploadScreenshots();

            // First, collect all the things for the HTML Report
            //var screenshots = uploadScreenshots.CollectScreenshots(options);
            //uploadMetadata.LoadFromFilesystem(options);

            // Assign "default" values to all languages
            //uploadMetadata.AssignDefaults(options);

            // Handle app icon / watch icon
            //PrepareAppIcons(options);

            // Commit
            //uploadMetadata.Upload(options);
            //uploadScreenshots.Upload(options, screenshots);
            //var uploadPriceTier = new UploadPriceTier();
            //uploadPriceTier.Upload(options);
            // e.g. app icon
            //var uploadAssets = new UploadAssets();
            //uploadAssets.Upload(options);
        }

        // Upload the binary to App Store Connect
        public void UploadBinary()
        {
            Console.WriteLine("Uploading binary to App Store Connect");

            string packagePath = "";
            bool uploadIpa = !string.IsNullOrEmpty(options.ipa);
            bool uploadPkg = !string.IsNullOrEmpty(options.pkg);

            // 2020-01-27
            // Only verify platform if both ipa and pkg exists (for backwards support)
            if (uploadIpa && uploadPkg)
            {
                List<string> platforms = new List<string> { "ios", "appletvos" };
                uploadIpa = platforms.Contains(options.platform);
                uploadPkg = options.platform == "osx";
            }

            if (uploadIpa)
            {
                var builder = new IpaUploadPackageBuilder();
                packagePath = builder.Generate(appId: options.app.appleId, ipaPath: options.ipa, packagePath: "/tmp", platform: options.platform);
            }
            else if (uploadPkg)
            {
                var builder = new PkgUploadPackageBuilder();
                packagePath = builder.Generate(appId: options.app.appleId, pkgPath: options.pkg, packagePath: "/tmp", platform: options.platform);
            }

            var transporter = TransporterForSelectedTeam();
            bool result = transporter.Upload(options.app.appleId, packagePath);
            if (!result)
                Console.WriteLine("Could not upload binary to App Store Connect. Check out the error above");
        }

        // If itc_provider was explicitly specified, use it.
        // If there are multiple teams, infer the provider from the selected team name.
        // If there are fewer than two teams, don't infer the provider.
        private ItunesTransporter TransporterForSelectedTeam()
        {
            var genericTransporter = new ItunesTransporter(options.username, null, false, options.itcProvider);
            int teamCount = Globals.TunesClient.Teams.Count;
            if (!string.IsNullOrEmpty(options.itcProvider) || teamCount <= 1)
                return genericTransporter;

            try
            {
                var team = Globals.TunesClient.Teams.Find(team => team.contentProvider.contentProviderId.ToString() == Globals.TunesClient.teamId);
                var name = team.contentProvider.name;
                var providerId = genericTransporter.ProviderIds()[name];
                Console.WriteLine($"Inferred provider id {providerId} for team #{name}.");
                return new ItunesTransporter(options.username, null, false, providerId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't infer a provider short name for team with id {Globals.TunesClient.teamId} automatically: {ex.GetType().ToString()}. Proceeding without provider short name.");
                return genericTransporter;
            }
        }

        public void RejectVersionIfPossible()
        {
            var app = options.app;
            if (app.RejectVersionIfPossible())
                Console.WriteLine("Successfully rejected previous version!");
        }
    }
}
