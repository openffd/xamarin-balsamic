using static Balsamic.String.FileExtension;
using static Foundation.NSBundle;
using static Newtonsoft.Json.JsonConvert;
using System.Collections.Generic;
using System.IO;

namespace Balsamic.Models.Sample
{
    sealed class DataProvider
    {
        public AppleDevAccount? AppleDevAccount { get; set; }
        public List<ApplicationDetail>? ApplicationDetails { get; set; }
        public List<ApplicationVersion>? ApplicationVersions { get; set; }

        public DataProvider()
        {
            SetupAppleDevAccount();
            SetupApplicationDetails();
            SetupApplicationVersions();
        }

        private void SetupAppleDevAccount()
        {
            string? path = MainBundle.PathForResource(AppleDevAccount.ResourcePath, Json.String());
            AppleDevAccount = DeserializeObject<AppleDevAccount>(File.ReadAllText(path));
        }

        private void SetupApplicationDetails()
        {
            string? path = MainBundle.PathForResource(ApplicationDetail.ResourcePath, Json.String());
            ApplicationDetails = DeserializeObject<List<ApplicationDetail>>(File.ReadAllText(path));
        }

        private void SetupApplicationVersions()
        {
            string? path = MainBundle.PathForResource(ApplicationVersion.ResourcePath, Json.String());
            ApplicationVersions = DeserializeObject<List<ApplicationVersion>>(File.ReadAllText(path));
        }
    }
}
