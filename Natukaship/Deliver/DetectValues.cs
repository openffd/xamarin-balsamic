using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Natukaship.Deliver
{
    public class DetectValues
    {
        public DeliverOptions Run(DeliverOptions opts, JObject skipParams = null)
        {
            opts = FindPlatform(opts);
            opts = FindAppIdentifier(opts);
            opts = FindApp(opts);
            opts = FindFolders(opts);
            EnsureFoldersCreated(opts);
            if (!skipParams.Value<bool>("skipVersion"))
                opts = FindVersion(opts);

            VerifyLanguages(opts);

            return opts;
        }

        public DeliverOptions FindAppIdentifier(DeliverOptions opts)
        {
            try
            {
                if (string.IsNullOrEmpty(opts.appIdentifier))
                    return opts;

                string identifier = "";
                if (string.IsNullOrEmpty(opts.ipa))
                    identifier = IpaFileAnalyser.FetchAppIdentifier(opts.ipa);
                else if (string.IsNullOrEmpty(opts.pkg))
                    identifier = PkgFileAnalyser.FindAppIdentifier(opts.pkg);

                if (identifier.Length > 0)
                    opts.appIdentifier = identifier;

                Console.Write("The Bundle Identifier of your App: ");
                StringBuilder sb = new StringBuilder();
                while (true)
                {
                    ConsoleKeyInfo cki = Console.ReadKey(true);
                    if (cki.Key == ConsoleKey.Enter)
                    {
                        Console.WriteLine();
                        break;
                    }
                    if (cki.Key == ConsoleKey.Backspace)
                    {
                        //Prevent an exception when you hit backspace with no characters on the array.
                        if (sb.Length > 0)
                        {
                            sb.Remove(sb.Length - 1, 1);
                            Console.Write("\b \b");
                        }
                    }
                    sb.Append(cki.KeyChar);
                }

                opts.appIdentifier = sb.ToString();

                return opts;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Could not infer your App's Bundle Identifier");

                return null;
            }
        }

        public DeliverOptions FindApp(DeliverOptions opts)
        {
            var searchBy = opts.appIdentifier;
            if (searchBy.Length == 0)
                searchBy = opts.app.bundleId;

            var app = Application.Find(searchBy, isMac: false);
            if (app != null)
                opts.app = app;
            else
                Console.WriteLine($"Could not find app with app identifier '{opts.appIdentifier}' in your App Store Connect account ({opts.username} - Team: {Globals.TunesClient.teamId})");

            return opts;
        }

        public DeliverOptions FindFolders(DeliverOptions opts)
        {
            opts.screenshotsPath = Path.Combine(".", "screenshots");
            opts.metadataPath = Path.Combine(".", "metadata");

            return opts;
        }

        public void EnsureFoldersCreated(DeliverOptions opts)
        {
            Directory.CreateDirectory(opts.screenshotsPath);
            Directory.CreateDirectory(opts.metadataPath);
        }

        public DeliverOptions FindVersion(DeliverOptions opts)
        {
            if (string.IsNullOrEmpty(opts.appVersion))
                return opts;

            try
            {
                if (string.IsNullOrEmpty(opts.ipa))
                    opts.appVersion = IpaFileAnalyser.FetchAppVersion(opts.ipa);
                else if (string.IsNullOrEmpty(opts.pkg))
                    opts.appVersion = PkgFileAnalyser.FetchAppVersion(opts.pkg);

                return opts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.StackTrace}");
                Console.WriteLine("Could not infer your app's version");
                return null;
            }
        }

        public DeliverOptions FindPlatform(DeliverOptions opts)
        {
            if (string.IsNullOrEmpty(opts.ipa))
                opts.platform = IpaFileAnalyser.FetchAppPlatform(opts.ipa);
            else if (string.IsNullOrEmpty(opts.pkg))
                opts.platform = "osx";

            return opts;
        }

        public void VerifyLanguages(DeliverOptions opts)
        {
            var languages = opts.languages;

            if (languages == null || languages.Count == 0)
                return;

            var allLanguages = Globals.TunesClient.AvailableLanguages();
            var diffLanguages = languages.Except(allLanguages);

            if (diffLanguages != null && diffLanguages.Count() > 0)
                Console.WriteLine($"The following languages are invalid and cannot be activated: #{string.Join(",", diffLanguages)}\n\nValid languages are: {allLanguages}");
        }
    }
}
