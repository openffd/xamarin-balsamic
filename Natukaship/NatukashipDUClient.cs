using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Natukaship
{
    // DU - Client
    // spaceship/lib/spaceship/du/du_client.rb
    public class NatukashipDUClient : NatukashipClient
    {
        private string cwd = Directory.GetCurrentDirectory();
        public AccountManager Account { get; set; }

        public NatukashipDUClient(NatukashipSetting setting) : base(setting)
        {
            Globals.DUClient = this;
        }

        //////////////////////////
        /// @!group Init and Login
        //////////////////////////

        public string Hostname { get => $"https://du-itc.itunes.apple.com"; }

        //////////////////////////
        /// @!group Images
        //////////////////////////
        public UploadFileResponseObject UploadScreenshot(AppVersion appVersion, UploadFile uploadFile, string contentProviderId, string ssoTokenForImage, string device, bool isMessages)
        {
            return UploadFile(appVersion: appVersion, uploadFile: uploadFile, path: "/upload/image", contentProviderId: contentProviderId, ssoToken: ssoTokenForImage, duValidationRuleSet: ScreenshotPictureType(device, isMessages));
        }

        public UploadFileResponseObject UploadPurchaseMerchScreenshot(string appId, UploadFile uploadFile, string contentProviderId, string ssoTokenForImage)
        {
            // TODO: Pushed back to a later iteration
            return UploadFile(appId: appId, uploadFile: uploadFile, path: "/upload/image", contentProviderId: contentProviderId, ssoToken: ssoTokenForImage, duValidationRuleSet: "MZPFT.MerchandisingIAPIcon");
        }

        public UploadFileResponseObject UploadPurchaseReviewScreenshot(string appId, UploadFile uploadFile, string contentProviderId, string ssoTokenForImage)
        {
            // TODO: Pushed back to a later iteration
            return UploadFile(appId: appId, uploadFile: uploadFile, path: "/upload/image", contentProviderId: contentProviderId, ssoToken: ssoTokenForImage, duValidationRuleSet: GetPictureType(uploadFile));
        }

        public UploadFileResponseObject UploadLargeIcon(AppVersion appVersion, UploadFile uploadFile, string contentProviderId, string ssoTokenForImage)
        {
            return UploadFile(appVersion: appVersion, uploadFile: uploadFile, path: "/upload/image", contentProviderId: contentProviderId, ssoToken: ssoTokenForImage, duValidationRuleSet: "MZPFT.LargeApplicationIcon");
        }

        public UploadFileResponseObject UploadWatchIcon(AppVersion appVersion, UploadFile uploadFile, string contentProviderId, string ssoTokenForImage)
        {
            // TODO: Pushed back to a later iteration
            return UploadFile(appVersion: appVersion, uploadFile: uploadFile, path: "/upload/image", contentProviderId: contentProviderId, ssoToken: ssoTokenForImage, duValidationRuleSet: "MZPFT.GizmoAppIcon");
        }

        public UploadFileResponseObject UploadGeojson(AppVersion appVersion, UploadFile uploadFile, string contentProviderId, string ssoTokenForImage)
        {
            // TODO: Pushed back to a later iteration
            return UploadFile(appVersion: appVersion, uploadFile: uploadFile, path: "/upload/geo-json", contentProviderId: contentProviderId, ssoToken: ssoTokenForImage);
        }

        public UploadFileResponseObject UploadTrailer(AppVersion appVersion, UploadFile uploadFile, string contentProviderId, string ssoTokenForVideo)
        {
            return UploadFile(appVersion: appVersion, uploadFile: uploadFile, path: "/upload/purple-video", contentProviderId: contentProviderId, ssoToken: ssoTokenForVideo);
        }

        public UploadFileResponseObject UploadTrailerPreview(AppVersion appVersion, UploadFile uploadFile, string contentProviderId, string ssoTokenForImage, string device)
        {
            return UploadFile(appVersion: appVersion, uploadFile: uploadFile, path: "/upload/image", contentProviderId: contentProviderId, ssoToken: ssoTokenForImage, duValidationRuleSet: ScreenshotPictureType(device, false));
        }

        public UploadFileResponseObject UploadAppReviewAttachment(AppVersion appVersion, UploadFile uploadFile, string contentProviderId, string ssoTokenForAttachment)
        {
            // TODO: Continue this method
            return UploadFile(appVersion: appVersion, uploadFile: uploadFile, path: "/upload/app-resolution-file", contentProviderId: contentProviderId, ssoToken: ssoTokenForAttachment);
        }

        public string GetPictureType(UploadFile uploadFile)
        {
            List<int> resolution = Utilities.Resolution(uploadFile.filePath).ToList();
            KeyValuePair<string, List<List<int>>> result = DeviceResolutionMap().First(devRes => devRes.Value.Contains(resolution));

            return PictureTypeMap()[result.Key];
        }

        private UploadFileResponseObject UploadFile(AppVersion appVersion = null,
                                UploadFile uploadFile = null,
                                string path = null,
                                string contentProviderId = null,
                                string ssoToken = null,
                                string duValidationRuleSet = null,
                                string appId = null)
        {
            if (uploadFile.fileSize == 0)
                throw new System.Exception($"File {uploadFile.filePath} is empty");

            string appType = null;
            Version version = null;
            string referrer = null;

            if (!string.IsNullOrEmpty(appId))
            {
                appType = null;
                version = null;
                referrer = null;
            }
            else
            {
                version = appVersion.version;
                appId = appVersion.application.appleId;
                appType = appVersion.appType;
                referrer = appVersion.application.url;
            }

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { "Accept", "application/json, text/plain, */*" },
                { "Content-Type", uploadFile.contentType },
                { "X-Apple-Upload-AppleId", appId },
                { "X-Apple-Upload-itctoken", ssoToken },
                { "X-Apple-Upload-ContentProviderId", contentProviderId },
                { "Content-Length", uploadFile.fileSize.ToString() },
                { "X-Original-Filename", uploadFile.fileName },
                { "Connection", "keep-alive" },
            };

            if (!string.IsNullOrEmpty(referrer))
            {
                headers.Add("X-Apple-Upload-Referrer", referrer);
                headers.Add("Referrer", referrer);
            }

            if (!string.IsNullOrEmpty(appType))
                headers.Add("X-Apple-Jingle-Correlation-Key", $"{appType}:AdamId={appId}:Version={version}");

            if (!string.IsNullOrEmpty(duValidationRuleSet))
                headers.Add("X-Apple-Upload-Validation-RuleSets", duValidationRuleSet);

            Task<(string, int)> uploadTask = Task.Run(async () => await Account.SendRequestAsync("POST", $"{Hostname}{path}", uploadFile, headers: headers));
            (string uploadResponseResult, int uploadResponseCode) = uploadTask.Result;

            if (uploadResponseCode == 500 && uploadResponseResult.Contains("Server Error"))
            {
                UploadFile(appVersion: appVersion, uploadFile: uploadFile, path: path, contentProviderId: contentProviderId, ssoToken: ssoToken, duValidationRuleSet: duValidationRuleSet, appId: appId);
            }

            return ParseUploadResponse(uploadResponseResult, uploadResponseCode);
        }

        private Dictionary<string, string> PictureTypeMap()
        {
            Dictionary<string, string> _lookup = new Dictionary<string, string>();
            foreach (var displayFamily in DisplayFamily.All())
            {
                _lookup.Add(displayFamily.name.ToString(), displayFamily.pictureType.ToString());
            }

            return _lookup;
        }

        private Dictionary<string, string> MessagePictureTypeMap()
        {
            Dictionary<string, string> _lookup = new Dictionary<string, string>();
            foreach (var displayFamily in DisplayFamily.All())
            {
                if (displayFamily.IsMessagesSupported())
                    _lookup.Add(displayFamily.name.ToString(), displayFamily.messagesPictureType.ToString());
            }

            return _lookup;
        }

        private Dictionary<string, List<List<int>>> DeviceResolutionMap()
        {
            Dictionary<string, List<List<int>>> _lookup = new Dictionary<string, List<List<int>>>();
            foreach (var displayFamily in DisplayFamily.All())
            {
                if (displayFamily.IsMessagesSupported())
                    _lookup.Add(displayFamily.name.ToString(), displayFamily.screenshotResolutions);
            }

            return _lookup;
        }

        private string ScreenshotPictureType(string device, bool isMessages)
        {
            var map = isMessages ? MessagePictureTypeMap() : PictureTypeMap();
            if (!map.Keys.Contains(device))
                throw new System.Exception($"Unknown picture type for device: {device}");

            return map[device];
        }

        private UploadFileResponseObject ParseUploadResponse(string response, int? responseCode)
        {
            var content = JObject.Parse(response).ToObject<UploadFileResponseObject>();
            if (responseCode != null && responseCode != 200 && responseCode != 201)
            {
                string errorCodes = "";
                if (content.errorCodes != null)
                    errorCodes = string.Join(",", content.errorCodes.Select(x => x.ToString()).ToArray());

                string errorMessage = $"[{errorCodes}] [{content.localizedMessage}]";

                throw new System.Exception(errorMessage);
            }
            return content;
        }
    }
}
