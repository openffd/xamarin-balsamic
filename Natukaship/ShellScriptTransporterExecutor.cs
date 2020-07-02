using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Natukaship
{
    // Generates commands and executes the iTMSTransporter through the shell script it provides by the same name
    public class ShellScriptTransporterExecutor : TransporterExecutor
    {
        public string[] BuildUploadCommand(string user, string pass, string source = "/tmp", string providerShortName = "")
        {
            List<string> commands = new List<string>();
            commands.Add("\"" + Helper.TransporterPath + "\"");
            commands.Add("-m upload");
            commands.Add($@"-u {user}");
            commands.Add($@"-p {ShellEscapedPassword(pass)}");
            commands.Add($"-f \"{source}\"");
            commands.Add(AdditionalUploadParameters()); // that's here, because the user might overwrite the -t option
            commands.Add("-k 100000");

            if (!string.IsNullOrEmpty(providerShortName))
                commands.Add($"-itc_provider {providerShortName}");

            commands.RemoveAll(item => item == null);

            return commands.ToArray();
        }

        // def build_download_command(username, password, apple_id, destination = "/tmp", provider_short_name = "")
        public string[] BuildDownloadCommand(string user, string pass, string appId, string destination = "/tmp", string providerShortName = "")
        {
            List<string> commands = new List<string>();
            commands.Add("\"" + Helper.TransporterPath + "\"");
            commands.Add("-m lookupMetadata");
            commands.Add($@"-u {user}");
            commands.Add($@"-p {ShellEscapedPassword(pass)}");
            commands.Add($"-apple_id #{appId}");
            commands.Add($"-destination '#{destination}'");

            if (!string.IsNullOrEmpty(providerShortName))
                commands.Add($"-itc_provider {providerShortName}");

            commands.RemoveAll(item => item == null);

            return commands.ToArray();
        }

        public string[] BuildProviderIdsCommand(string user, string pass)
        {
            List<string> commands = new List<string>();
            commands.Add("\"" + Helper.TransporterPath + "\"");
            commands.Add("-m provider");
            commands.Add($@"-u {user}");
            commands.Add($@"-p {ShellEscapedPassword(pass)}");

            commands.RemoveAll(item => item == null);

            return commands.ToArray();
        }

        public void HandleError(string pass)
        {
            if (!new Regex(@"^[0-9a-zA-Z\.\$\_\-]*$").Match(pass).Success)
            {
                Console.WriteLine(string.Join(" ", new List<string> {
                  "Password contains special characters, which may not be handled properly by iTMSTransporter.",
                  "If you experience problems uploading to App Store Connect, please consider changing your password to something with only alphanumeric characters."
                }));
            }

            Console.WriteLine("Could not download/upload from App Store Connect! It's probably related to your password or your internet connection.");
        }

        private string ShellEscapedPassword(string pass)
        {
            pass = $@"{pass}";
            // because the shell handles passwords with single-quotes incorrectly, use `gsub` to replace `shellescape`'d single-quotes of this form:
            //    \'
            // with a sequence that wraps the escaped single-quote in double-quotes:
            //    '"\'"'
            // this allows us to properly handle passwords with single-quotes in them
            // background: https://stackoverflow.com/questions/1250079/how-to-escape-single-quotes-within-single-quoted-strings/1250098#1250098
            pass = pass.Replace("\\'", "'\"\\'\"'");

            // wrap the fully-escaped password in single quotes, since the transporter expects a escaped password string (which must be single-quoted for the shell's benefit)
            pass = "'" + pass + "'";
            return pass;
        }
    }
}
