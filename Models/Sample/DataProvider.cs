using System;
using System.IO;
using Newtonsoft.Json;
using static Foundation.NSBundle;

namespace Balsamic.Models.Samples
{
    sealed class DataProvider
    {
        public Account Account { get; set; }

        public DataProvider()
        {
            var path = MainBundle.PathForResource("Data/Account/account", "json");
            Account = JsonConvert.DeserializeObject<Account>(File.ReadAllText(path));
        }


    }

    class Account
    {
        [JsonProperty(PropertyName = "apple-id-key")]
        public string AppleIdKey { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "obf-apple-id")]
        public string ObfuscatedAppleId { get; set; }

        [JsonProperty(PropertyName = "team-id-key")]
        public string TeamIdKey { get; set; }

        [JsonProperty(PropertyName = "team-name")]
        public string TeamName { get; set; }

        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
    }
}