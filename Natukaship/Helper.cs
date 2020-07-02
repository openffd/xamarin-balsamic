using System;
using System.Diagnostics;
using System.IO;

namespace Natukaship
{
    public class Helper
    {
        // @return the full path to the iTMSTransporter executable
        public static string TransporterPath
        {
            get
            {
                return Path.Combine(ItmsPath, "bin", "iTMSTransporter");
            }
        }

        public static string TransporterJavaExecutablePath
        {
            get
            {
                return Path.Combine(TransporterJavaPath, "bin", "java");
            }
        }

        public static string TransporterJavaExtDir
        {
            get
            {
                return Path.Combine(TransporterJavaPath, "lib", "ext");
            }
        }

        public static string TransporterJavaJarPath
        {
            get
            {
                return Path.Combine(ItmsPath, "lib", "itmstransporter-launcher.jar");
            }
        }

        public static string TransporterUserDir
        {
            get
            {
                return Path.Combine(ItmsPath, "bin");
            }
        }

        public static string ItmsPath
        {
            get
            {
                if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("FASTLANE_ITUNES_TRANSPORTER_PATH")))
                    return Environment.GetEnvironmentVariable("FASTLANE_ITUNES_TRANSPORTER_PATH");

                // First check for manually install iTMSTransporter
                string userLocalItmsPath = "/usr/local/itms";
                if (File.Exists(userLocalItmsPath))
                    return userLocalItmsPath;

                // Then check for iTMSTransporter in the Xcode path
                string[] itmsExpectedPaths = new string[] {
                  "../Applications/Application Loader.app/Contents/MacOS/itms",
                  "../Applications/Application Loader.app/Contents/itms",
                  "../SharedFrameworks/ContentDeliveryServices.framework/Versions/A/itms" // For Xcode 11
                };

                foreach (var itmsExpectedPath in itmsExpectedPaths)
                {
                    string expectedPath = Path.Combine(XcodePath, itmsExpectedPath);
                    expectedPath = Path.GetFullPath(expectedPath);

                    if (Directory.Exists(expectedPath))
                        return expectedPath;
                }

                throw new Exception($"Could not find transporter at {XcodePath}. Please make sure you set the correct path to your Xcode installation.");
            }
        }

        // @return the full path to the Xcode developer tools of the currently
        //  running system
        public static string XcodePath
        {
            get
            {
                if (Environment.GetEnvironmentVariable("XCS") != null && int.Parse(Environment.GetEnvironmentVariable("XCS")) == 1)
                {
                    // Xcode server always creates a link here
                    string xcodeServerXcodePath = "/Library/Developer/XcodeServer/CurrentXcodeSymlink/Contents/Developer";
                    Console.WriteLine($"We're running as XcodeServer, setting path to {xcodeServerXcodePath}");

                    return xcodeServerXcodePath;
                }

                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "xcode-select",
                    Arguments = "-p",
                    CreateNoWindow = false,
                    RedirectStandardOutput = true
                };

                Process proc = new Process()
                {
                    StartInfo = startInfo,
                };
                proc.Start();

                var xcodePathResult = proc.StandardOutput.ReadToEnd();
                xcodePathResult = xcodePathResult.Replace("\n", "/");

                return xcodePathResult;
            }
        }

        public static string TransporterJavaPath
        {
            get
            {
                return Path.Combine(ItmsPath, "java");
            }
        }
    }
}
