using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Natukaship
{
    // Generates commands and executes the iTMSTransporter by invoking its Java app directly, to avoid the crazy parameter
    // escaping problems in its accompanying shell script.
    public class JavaTransporterExecutor : TransporterExecutor
    {
        public string[] BuildUploadCommand(string user, string pass, string source = "/tmp", string providerShortName = "")
        {
            List<string> commands = new List<string>();
            commands.Add($@"{Helper.TransporterJavaExecutablePath}");
            commands.Add($@"-Djava.ext.dirs={Helper.TransporterJavaExtDir}");
            commands.Add("-XX:NewSize=2m");
            commands.Add("-Xms32m");
            commands.Add("-Xmx1024m");
            commands.Add("-Xms1024m");
            commands.Add("-Djava.awt.headless=true");
            commands.Add("-Dsun.net.http.retryPost=false");
            commands.Add(JavaCodeOption);
            commands.Add("-m upload");
            commands.Add($@"-u {user}");
            commands.Add($@"-p {pass}");
            commands.Add($@"-f {source}");
            commands.Add(AdditionalUploadParameters()); // that's here, because the user might overwrite the -t option
            commands.Add("-k 100000");

            if (!string.IsNullOrEmpty(providerShortName))
                commands.Add($"-itc_provider {providerShortName}");

            commands.Add("2>&1");

            commands.RemoveAll(item => item == null);

            return commands.ToArray();
        }

        // def build_download_command(username, password, apple_id, destination = "/tmp", provider_short_name = "")
        public string[] BuildDownloadCommand(string user, string pass, string appId, string destination = "/tmp", string providerShortName = "")
        {
            List<string> commands = new List<string>();
            commands.Add($@"{Helper.TransporterJavaExecutablePath}");
            commands.Add($@"-Djava.ext.dirs={Helper.TransporterJavaExtDir}");
            commands.Add("-XX:NewSize=2m");
            commands.Add("-Xms32m");
            commands.Add("-Xmx1024m");
            commands.Add("-Xms1024m");
            commands.Add("-Djava.awt.headless=true");
            commands.Add("-Dsun.net.http.retryPost=false");
            commands.Add(JavaCodeOption);
            commands.Add("-m lookupMetadata");
            commands.Add($@"-u {user}");
            commands.Add($@"-p {pass}");
            commands.Add($@"-apple_id {appId}");
            commands.Add($@"-destination {destination}");

            if (!string.IsNullOrEmpty(providerShortName))
                commands.Add($"-itc_provider {providerShortName}");

            commands.Add("2>&1");

            commands.RemoveAll(item => item == null);

            return commands.ToArray();
        }

        public string[] BuildProviderIdsCommand(string user, string pass)
        {
            List<string> commands = new List<string>();
            commands.Add($@"{Helper.TransporterJavaExecutablePath}");
            commands.Add($@"-Djava.ext.dirs={Helper.TransporterJavaExtDir}");
            commands.Add("-XX:NewSize=2m");
            commands.Add("-Xms32m");
            commands.Add("-Xmx1024m");
            commands.Add("-Xms1024m");
            commands.Add("-Djava.awt.headless=true");
            commands.Add("-Dsun.net.http.retryPost=false");
            commands.Add(JavaCodeOption);
            commands.Add("-m provider");
            commands.Add($@"-u {user}");
            commands.Add($@"-p {pass}");
            commands.Add("2>&1");

            commands.RemoveAll(item => item == null);

            return commands.ToArray();
        }

        public string JavaCodeOption
        {
            get
            {
                return $@"-jar {Helper.TransporterJavaJarPath}";
            }
        }

        public void HandleError(string pass)
        {
            if (!File.Exists(Helper.TransporterJavaJarPath))
            {
                Console.WriteLine($"The iTMSTransporter Java app was not found at '{Helper.TransporterJavaJarPath}'.");
                Console.WriteLine("If you're using Xcode 6, please select the shell script executor by setting the environment variable " +
                    "FASTLANE_ITUNES_TRANSPORTER_USE_SHELL_SCRIPT=1");
            }
        }

        public override bool Execute(string[] commands, bool hideOutput)
        {
            // The Java command needs to be run starting in a working directory in the iTMSTransporter
            // file area. The shell script takes care of changing directories over to there, but we'll
            // handle it manually here for this strategy.
            var commandsList = commands.ToList();
            commandsList.Prepend($@"cd {Helper.ItmsPath}");
            commands = commandsList.ToArray();

            return base.Execute(commands, hideOutput);
        }
    }
}
