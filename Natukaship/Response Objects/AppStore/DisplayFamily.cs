using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Natukaship
{
    // A definition of different styled displays used by devices
    // that App Store Connect supports storing screenshots.
    // Display styles often only vary based on screen resolution
    // however other aspects of a displays physical appearance are
    // also factored (i.e if the home indicator is provided via
    // a hardware button or a software interface).
    public class DisplayFamily
    {
        static Dictionary<string, DisplayFamily> _lookup = new Dictionary<string, DisplayFamily>();

        // The display family name from the App Store Connect API that
        // is used when uploading or listing screenshots. This value is
        // then assigned to the
        // Spaceship::Tunes::AppScreenshot#device_type attribute.
        public string name { get; set; }

        // The user friendly name of this definition.
        //
        // Source: Media Manager in App Store Connect.
        public string friendlyName { get; set; }

        // The user friendly category for this definition (i.e iPhone,
        // Apple TV or Desktop).
        public string friendlyCategoryName { get; set; }

        // An array of supported screen resolutions (in pixels) that are
        // supported for the associated device.
        //
        // Source: https://help.apple.com/app-store-connect/#/devd274dd925
        //attr_accessor :screenshot_resolutions
        public List<List<int>> screenshotResolutions { get; set; }

        // An internal identifier for the same device definition used
        // in the DUClient.
        //
        // Source: You can find this by uploading an image in App Store
        // Connect using your browser and then look for the
        // X-Apple-Upload-Validation-RuleSets value in the uploads
        // request headers.
        public string pictureType { get; set; }

        // Similar to `picture_type`, but for iMessage screenshots.
        //attr_accessor :messages_picture_type
        public string messagesPictureType { get; set; }

        public DisplayFamily(JToken data)
        {
            name = data["name"].ToString();
            friendlyName = data["friendly_name"].ToString();
            friendlyCategoryName = data["friendly_category_name"].ToString();
            pictureType = data["picture_type"].ToString();
            messagesPictureType = data["messages_picture_type"].ToString();

            List<List<int>> screenshotResolutions = new List<List<int>>();
            var rand = new Random();
            for (int i = 0; i < data["screenshot_resolutions"].Count(); i++)
            {
                List<int> sublist = new List<int>();
                int sublistCount = data["screenshot_resolutions"][i].Values().Count();
                for (int v = 0; v < sublistCount; v++)
                {
                    sublist.Add(data["screenshot_resolutions"][i][v].Value<int>());
                }

                screenshotResolutions.Add(sublist);
            }
        }

        public bool IsMessagesSupported()
        {
            if (string.IsNullOrEmpty(messagesPictureType))
                return true;
            else
                return false;
        }

        public static Dictionary<string, DisplayFamily>.ValueCollection All()
        {
            return Lookup().Values;
        }

        public static DisplayFamily Find(string name)
        {
            return Lookup()[name];
        }

        private static Dictionary<string, DisplayFamily> Lookup()
        {
            if (_lookup != null && _lookup.Count > 0)
                return _lookup;

            string path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Natukaship", "Assets/displayFamilies.json"));
            JArray displayFamilies = JArray.Parse(File.ReadAllText(path));
            foreach (var displayFamily in displayFamilies)
            {
                _lookup.Add(displayFamily["name"].ToString(), new DisplayFamily(displayFamily));
            }

            return _lookup;
        }
    }
}
