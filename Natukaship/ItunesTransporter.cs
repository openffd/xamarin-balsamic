using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Natukaship
{
    public class ItunesTransporter
    {
        public string _user { get; set; }
        public string _pass { get; set; }
        public dynamic _transporterExecutor { get; set; }
        public string _providerShortName { get; set; }

        // Matches a line in the provider table: "12  Initech Systems Inc     LG89CQY559"
        public Regex PROVIDER_REGEX = new Regex(@"^\d+\s{2,}.+\s{2,}[^\s]+$");
        public string TWO_STEP_HOSTP_REFIX = "deliver.appspecific";

        // This will be called from the Deliverfile, and disables the logging of the transporter output
        public static bool HideTransporterOutput()
        {
            return true;
        }

        public static bool IsHideTransporterOutput()
        {
            return true;
        }

        // Returns a new instance of the iTunesTransporter.
        // If no username or password given, it will be taken from
        // the #{CredentialsManager::AccountManager}
        // @param useShellScript if true, forces use of the iTMSTransporter shell script.
        //                         if false, allows a direct call to the iTMSTransporter Java app (preferred).
        //                         see: https://github.com/fastlane/fastlane/pull/4003
        // @param providerShortName The provider short name to be given to the iTMSTransporter to identify the
        //                            correct team for this work. The provider short name is usually your Developer
        //                            Portal team ID, but in certain cases it is different!
        //                            see: https://github.com/fastlane/fastlane/issues/1524#issuecomment-196370628
        //                            for more information about how to use the iTMSTransporter to list your provider
        //                            short names
        public ItunesTransporter(string user = null, string pass = null, bool useShellScript = false, string providerShortName = null)
        {
            // Xcode 6.x doesn't have the same iTMSTransporter Java setup as later Xcode versions, so
            // we can't default to using the newer direct Java invocation strategy for those versions.

            _user = user;
            _pass = pass;

            if (useShellScript)
                _transporterExecutor = new ShellScriptTransporterExecutor();
            else
                _transporterExecutor = new JavaTransporterExecutor();

            _providerShortName = providerShortName;
        }

        // Downloads the latest version of the app metadata package from iTC.
        // @param appId [Integer] The unique App ID
        // @param dir [String] the path in which the package file should be stored
        // @return (Bool) True if everything worked fine
        // @raise [TransporterTransferException] when something went wrong
        //   when transferring
        public bool Download(string appId, string dir = null)
        {
            var result = string.Empty;
            //if (dir == null)
            //    dir = "/tmp";

            Console.WriteLine("Going to download app metadata from App Store Connect");
            dynamic command = _transporterExecutor.BuildDownloadCommand(_user, _pass, appId, dir, _providerShortName);
            Console.WriteLine(_transporterExecutor.BuildDownloadCommand(_user, "YourPassword", appId, dir, _providerShortName));

            try
            {
                result = _transporterExecutor.Execute(command, IsHideTransporterOutput());
            }
            catch (Exception ex)
            {
                HandleTwoStepFailure(ex);
                return Download(appId, dir);
            }

            string itmspPath = Path.Combine(dir, $"{appId}.itmsp");
            bool successful = result != null && Directory.Exists(itmspPath);

            if (successful)
                Console.WriteLine($"✅ Successfully downloaded the latest package from App Store Connect to {itmspPath}");
            else
                HandleError(_pass);

            return successful;
        }

        // Uploads the modified package back to App Store Connect
        // @param appId [Integer] The unique App ID
        // @param dir [String] the path in which the package file is located
        // @return (Bool) True if everything worked fine
        // @raise [TransporterTransferException] when something went wrong
        //   when transferring
        public bool Upload(string appId, string dir)
        {
            bool? result = null;
            string actualDir = Path.Combine(dir, $"{appId}.itmsp");

            Console.WriteLine("Going to upload updated app to App Store Connect");
            Console.WriteLine("This might take a few minutes. Please don't interrupt the script.");

            string[] commands = _transporterExecutor.BuildUploadCommand(_user, _pass, actualDir, _providerShortName);
            Console.WriteLine(string.Join(" ", _transporterExecutor.BuildUploadCommand(_user, "YourPassword", actualDir, _providerShortName)));

            try
            {
                result = _transporterExecutor.Execute(commands, IsHideTransporterOutput());
            }
            catch (Exception ex)
            {
                HandleTwoStepFailure(ex);
                return Upload(appId, dir);
            }

            if (result != null)
                Console.WriteLine("Successfully uploaded package to App Store Connect. It might take a few minutes until it's visible online.");
            else
                HandleError(_pass);

            return (bool)result;
        }

        public Dictionary<string, string> ProviderIds()
        {
            dynamic command = _transporterExecutor.BuildProviderIdsCommand(_user, _pass);
            Console.WriteLine(_transporterExecutor.BuildProviderIdsCommand(_user, "YourPassword"));
            List<string> lines = new List<string>();
            try
            {
                var result = _transporterExecutor.Execute(command, IsHideTransporterOutput()); // { | xs | lines = xs };
            }
            catch (Exception ex)
            {
                HandleTwoStepFailure(ex);
                return ProviderIds();
            }

            Dictionary<string, string> dictResult = new Dictionary<string, string>();
            foreach (var line in lines)
            {
                Hashtable hashtable = new Hashtable();
                var res = ProviderPair(line);
                if (res != null)
                {
                    dictResult.Add(res[0], res[1]);
                }
            }

            return dictResult;
        }

        private string TWO_FACTOR_ENV_VARIABLE = "FASTLANE_APPLE_APPLICATION_SPECIFIC_pass";

        // Tells the user how to get an application specific password
        public bool HandleTwoStepFailure(Exception ex)
        {
            if (Environment.GetEnvironmentVariable("TWO_FACTOR_ENV_VARIABLE").Length > 0)
            {
                // Password provided, however we already used it
                Console.WriteLine("");
                Console.WriteLine("Application specific password you provided using");
                Console.WriteLine($"environment variable {TWO_FACTOR_ENV_VARIABLE}");
                Console.WriteLine("is invalid, please make sure it's correct");
                Console.WriteLine("");
                Console.WriteLine("Invalid application specific password provided");
            }

            Console.WriteLine("");
            Console.WriteLine("Your account has 2 step verification enabled");
            Console.WriteLine("Please go to https://appleid.apple.com/account/manage");
            Console.WriteLine("and generate an application specific password for");
            Console.WriteLine("the iTunes Transporter, which is used to upload builds");
            Console.WriteLine("");
            Console.WriteLine("To set the application specific password on a CI machine using");
            Console.WriteLine("an environment variable, you can set the");
            Console.WriteLine($"{TWO_FACTOR_ENV_VARIABLE} variable");
            _pass = Globals.TunesClient.account.Password; // to ask the user for the missing value

            return true;
        }

        public void HandleError(string pass)
        {
            _transporterExecutor.HandleError(pass);
        }

        public string[] ProviderPair(string line)
        {
            line = line.Trim();

            if (PROVIDER_REGEX.Match(line).Success && PROVIDER_REGEX.Match(line).Captures.Count > 0)
                return Regex.Split(line, @"\s{2,}").Skip(1).ToArray();
            else
                return null;
        }
    }
}
