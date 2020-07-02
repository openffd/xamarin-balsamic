using Newtonsoft.Json;

namespace Natukaship
{
    public class NameAppApp
    {
        [JsonProperty(PropertyName = "application-identifier")]
        public string applicationIdentifier { get; set; }
        [JsonProperty(PropertyName = "get-task-allow")]
        public string getTaskAllow { get; set; }
        [JsonProperty(PropertyName = "beta-reports-active")]
        public string betaReportsActive { get; set; }
        [JsonProperty(PropertyName = "keychain-access-groups")]
        public string keychainAccessGroups { get; set; }
        [JsonProperty(PropertyName = "com.apple.developer.team-identifier")]
        public string comAppleDeveloperTeamIdentifier { get; set; }
    }

    public class Entitlements
    {
        [JsonProperty(PropertyName = "Name.app/App")]
        public NameAppApp NameappApp { get; set; }
    }

    public class SizeInBytesSharedValues
    {
        public int uncompressed { get; set; }
        public int compressed { get; set; }
    }

    public class SizesInBytes
    {
        public SizeInBytesSharedValues iPhone4S { get; set; }
        public SizeInBytesSharedValues iPad4Wifi { get; set; }
        public SizeInBytesSharedValues iPhone6Plus { get; set; }
        public SizeInBytesSharedValues iPhone6sPlus { get; set; }
        [JsonProperty(PropertyName = "iPadAirWifi+Cell")]
        public SizeInBytesSharedValues iPadAirWifiCell { get; set; }
        public SizeInBytesSharedValues iPhone6 { get; set; }
        public SizeInBytesSharedValues iPhone5 { get; set; }
        [JsonProperty(PropertyName = "iPadProWiFi+Cellular")]
        public SizeInBytesSharedValues iPadProWiFiCellular { get; set; }
        [JsonProperty(PropertyName = "iPadmini4WiFi+Cellular")]
        public SizeInBytesSharedValues iPadmini4WiFiCellular { get; set; }
        [JsonProperty(PropertyName = "iPad3Wifi+Cell")]
        public SizeInBytesSharedValues iPad3WifiCell { get; set; }
        public SizeInBytesSharedValues iPadAirWifi { get; set; }
        [JsonProperty(PropertyName = "iPadMiniWifi+Cell")]
        public SizeInBytesSharedValues iPadMiniWifiCell { get; set; }
        [JsonProperty(PropertyName = "iPadMini2Wifi+Cell")]
        public SizeInBytesSharedValues iPadMini2WifiCell { get; set; }
        [JsonProperty(PropertyName = "iPadMini3Wifi+Cell")]
        public SizeInBytesSharedValues iPadMini3WifiCell { get; set; }
        public SizeInBytesSharedValues iPadProWiFi { get; set; }
        public SizeInBytesSharedValues Universal { get; set; }
        public SizeInBytesSharedValues iPodTouchSixthGeneration { get; set; }
        public SizeInBytesSharedValues iPodTouchFifthGeneration { get; set; }
        public SizeInBytesSharedValues iPhone6s { get; set; }
        public SizeInBytesSharedValues iPad3Wifi { get; set; }
        public SizeInBytesSharedValues iPhone5S { get; set; }
        public SizeInBytesSharedValues iPadMini3Wifi { get; set; }
        public SizeInBytesSharedValues iPad23G { get; set; }
        public SizeInBytesSharedValues iPad2Wifi { get; set; }
        [JsonProperty(PropertyName = "iPad4Wifi+Cell")]
        public SizeInBytesSharedValues iPad4WifiCell { get; set; }
        public SizeInBytesSharedValues iPadMini2Wifi { get; set; }
        public SizeInBytesSharedValues iPhone5C { get; set; }
        public SizeInBytesSharedValues iPadmini4WiFi { get; set; }
        public SizeInBytesSharedValues iPadMiniWifi { get; set; }
        [JsonProperty(PropertyName = "iPadAir2Wifi+Cell")]
        public SizeInBytesSharedValues iPadAir2WifiCell { get; set; }
        public SizeInBytesSharedValues iPadAir2Wifi { get; set; }
    }

    public class BuildDetails
    {
        public string iconUrl { get; set; }
        public long uploadDate { get; set; }
        public string binaryState { get; set; }
        public string fileName { get; set; }
        public string buildSdk { get; set; }
        public string buildPlatform { get; set; }
        public string bundleId { get; set; }
        public string appName { get; set; }
        public string supportedArchitectures { get; set; }
        public string localizations { get; set; }
        public bool newsstandApp { get; set; }
        public bool prerenderedIconFlag { get; set; }
        public Entitlements entitlements { get; set; }
        public string appPlatform { get; set; }
        public string deviceProtocols { get; set; }
        public string cfBundleVersion { get; set; }
        public string cfBundleShortVersion { get; set; }
        public string minOsVersion { get; set; }
        public string deviceFamilies { get; set; }
        public string capabilities { get; set; }
        public int sizeInBytes { get; set; }
        public SizesInBytes sizesInBytes { get; set; }
        public bool containsODR { get; set; }
        public int numberOfAssetPacks { get; set; }
        public bool includesSymbols { get; set; }
        public string dsymurl { get; set; }
    }

    public class BuildDetailsResponseObject
    {
        public BuildDetails data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
