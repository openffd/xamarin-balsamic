using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Natukaship
{
    // Base class for executing the iTMSTransporter
    public class TransporterExecutor
    {
        public List<string> errors;
        public List<string> warnings;
        public List<string> allLines;

        private Regex ERROR_REGEX = new Regex(@">\s*ERROR:\s+(.+)");
        private Regex WARNING_REGEX = new Regex(@">\s*WARN:\s+(.+)");
        private Regex OUTPUT_REGEX = new Regex(@">\s+(.+)");
        private Regex RETURN_VALUE_REGEX = new Regex(@">\sDBG-X:\sReturning\s+(\d+)");

        private List<string> SKIP_ERRORS = new List<string> { "ERROR: An exception has occurred: Scheduling automatic restart in 1 minute" };

        public virtual bool Execute(string[] commands, bool hideOutput)
        {
            int exitStatus = 0;
            errors = new List<string>();
            warnings = new List<string>();
            allLines = new List<string>();

            if (hideOutput)
            {
                // Show a one time message instead
                Console.WriteLine("Waiting for App Store Connect transporter to be finished.");
                Console.WriteLine("iTunes Transporter progress... this might take a few minutes...");
            }

            try
            {
                //exitStatus = FastlaneCore::FastlanePty.spawn(command) do |command_stdout, command_stdin, pid|
                //  begin
                //    command_stdout.each do | line |
                //      allLines << line
                //      ParseLine(line, hideOutput); // this is where the parsing happens
                //    end
                //  end
                //end

                var commandsList = commands.ToList();
                commandsList.Prepend("");
                commandsList.Prepend(@"#! /bin/bash");
                commands = commandsList.ToArray();

                File.WriteAllLines("/tmp/itmsRunner.command", commands);

                var itmsRunnerProcess = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = $"chmod",
                        Arguments = $"u+x /tmp/itmsRunner.command",
                        CreateNoWindow = false,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true
                    }
                };

                itmsRunnerProcess.Start();

                string itmsRunnerOutput = itmsRunnerProcess.StandardOutput.ReadToEnd();
                foreach (var line in itmsRunnerOutput.Split("\n"))
                {
                    allLines.Add(line);
                    ParseLine(line, hideOutput);
                }

                File.Delete("/tmp/itmsRunner.command");
            }
            catch (Exception ex)
            {
                exitStatus = 1;
                errors.Add(ex.Message);
            }

            if (exitStatus > 0)
                errors.Add($"The call to the iTMSTransporter completed with a non-zero exit status: {exitStatus}. This indicates a failure.");

            if (warnings.Count > 0)
                Console.WriteLine(string.Join("\n", warnings));

            if (string.Join("\n", errors).Contains("app-specific"))
                throw new Exception("TransporterRequiresApplicationSpecificPasswordError");

            if (errors.Count > 0 && allLines.Count > 0)
            {
                // Print out the last 15 lines, this is key for non-verbose mode
                foreach(string err in allLines.TakeLast(15))
                {
                    Console.WriteLine($"[iTMSTransporter] {err}");
                }

                Console.WriteLine("iTunes Transporter output above ^");
                Console.WriteLine(string.Join("\n", errors));
            }

            // this is to handle GitHub issue #1896, which occurs when an
            //  iTMSTransporter file transfer fails; iTMSTransporter will log an error
            //  but will then retry; if that retry is successful, we will see the error
            //  logged, but since the status code is zero, we want to return success
            if (errors.Count > 0 && exitStatus == 0)
                Console.WriteLine("Although errors occurred during execution of iTMSTransporter, it returned success status.");

            return exitStatus == 0;
        }

        private void ParseLine(string line, bool hideOutput)
        {
            // Taken from https://github.com/sshaw/itunes_store_transporter/blob/master/lib/itunes/store/transporter/output_parser.rb
            bool outputDone = false;

            var re = new Regex(string.Join("", SKIP_ERRORS));
            if (re.Match(line).Success && re.Match(line).Captures.Count > 0)
            {
                // Those lines will not be handled like errors or warnings
            }
            else if (ERROR_REGEX.Match(line).Success && ERROR_REGEX.Match(line).Captures.Count > 0)
            {
                var matchRegex = ERROR_REGEX.Match(line).Value;
                errors.Add(matchRegex);

                Console.WriteLine($"[Transporter Error Output]: {matchRegex}");

                // Check if it's a login error
                if (matchRegex.Contains("Your Apple ID or password was entered incorrectly") ||
                    matchRegex.Contains("This Apple ID has been locked for security reasons"))
                {
                    Console.WriteLine("Please run this tool again to apple the new password");
                }
                else if (matchRegex.Contains("Redundant Binary Upload. There already exists a binary upload with build"))
                {
                    Console.WriteLine(matchRegex);
                    Console.WriteLine("You have to change the build number of your app to upload your ipa file");
                }

                outputDone = true;
            }
            else if (WARNING_REGEX.Match(line).Success && WARNING_REGEX.Match(line).Captures.Count > 0)
            {
                var matchRegex = ERROR_REGEX.Match(line).Value;
                warnings.Add(matchRegex);

                Console.WriteLine($"[Transporter Error Output]: {matchRegex}");
                outputDone = true;
            }

            if (RETURN_VALUE_REGEX.Match(line).Success && RETURN_VALUE_REGEX.Match(line).Captures.Count > 0)
            {
                var matchRegex = RETURN_VALUE_REGEX.Match(line).Value;
                int.TryParse(matchRegex, out int res);
                if (res == 0)
                {
                    Console.WriteLine("Transporter transfer failed.");
                    Console.WriteLine(string.Join("\n", warnings));
                    Console.WriteLine(string.Join("\n", errors));
                    throw new Exception($"Return status of iTunes Transporter was #{matchRegex}: {string.Join("\n", errors)}");
                }
                else
                    Console.WriteLine("iTunes Transporter successfully finished its job");
            }

            if (!hideOutput && OUTPUT_REGEX.Match(line).Success && OUTPUT_REGEX.Match(line).Captures.Count > 0)
            {
                var matchRegex = OUTPUT_REGEX.Match(line).Value;

                // General logging for debug purposes
                if (!outputDone)
                    Console.WriteLine($"[Transporter]: {matchRegex}");
            }
        }

        public string AdditionalUploadParameters()
        {
            // Workaround because the traditional transporter broke on 1st March 2018
            // More information https://github.com/fastlane/fastlane/issues/11958
            // As there was no communication from Apple, we don't know if this is a temporary
            // server outage, or something they changed without giving a heads-up

            if (Environment.GetEnvironmentVariable("DELIVER_ITMSTRANSPORTER_ADDITIONAL_UPLOAD_PARAMETERS")?.Length == 0)
                Environment.SetEnvironmentVariable("DELIVER_ITMSTRANSPORTER_ADDITIONAL_UPLOAD_PARAMETERS", "-t DAV,Signiant");

            return Environment.GetEnvironmentVariable("DELIVER_ITMSTRANSPORTER_ADDITIONAL_UPLOAD_PARAMETERS");
        }
    }
}
