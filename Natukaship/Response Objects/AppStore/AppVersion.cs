using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Natukaship
{
    public class AppVersion : AppVersionRaw
    {
        public bool is_live { get; set; }

        public Application application { get; set; }
        public Dictionary<string, List<AppScreenshot>> screenshots { get; set; } = new Dictionary<string, List<AppScreenshot>>();
        public Dictionary<string, List<AppTrailer>> trailers { get; set; } = new Dictionary<string, List<AppTrailer>>();

        // attr_mapping
        public string Copyright => copyright.value;
        public List<DetailsValue> Languages => details?.value;
        public string appIconOriginalName => largeAppIcon?.value?.originalFileName;
        public string appIconUrl => largeAppIcon?.value.url;
        public string ReleaseOnApproval => releaseOnApproval?.value;
        public object AutoReleaseDate => autoReleaseDate?.value;
        public bool RatingsReset => (bool)ratingsReset?.value;
        public string RawStatus => status;
        public string BuildVersion => preReleaseBuild?.buildVersion;
        public string Version => version?.value;
        public List<object> InAppPurchases => submittableAddOns?.value;

        public LargeAppIcon _largeAppIcon { get; set; }
        public ImageSharedValues _watchAppIcon { get; set; }
        public AttachmentFile _reviewAttachmentFile { get; set; }
        public object _transitAppFile { get; set; }

        // Trade Representative Contact Information
        public string TradeRepresentativeTradeName
        {
            get { return appStoreInfo.tradeName.value; }
            set { appStoreInfo.tradeName.value = value; }
        }

        public string TradeRepresentativeFirstName
        {
            get { return appStoreInfo.firstName.value; }
            set { appStoreInfo.firstName.value = value; }
        }

        public string TradeRepresentativeLastName
        {
            get { return appStoreInfo.lastName.value; }
            set { appStoreInfo.lastName.value = value; }
        }

        public string TradeRepresentativeAddressLine1
        {
            get { return appStoreInfo.addressLine1.value; }
            set { appStoreInfo.addressLine1.value = value; }
        }

        public string TradeRepresentativeAddressLine2
        {
            get { return appStoreInfo.addressLine2.value; }
            set { appStoreInfo.addressLine2.value = value; }
        }

        public string TradeRepresentativeAddressLine3
        {
            get { return appStoreInfo.addressLine3.value; }
            set { appStoreInfo.addressLine3.value = value; }
        }

        public string TradeRepresentativeCityName
        {
            get { return appStoreInfo.cityName.value; }
            set { appStoreInfo.cityName.value = value; }
        }

        public string TradeRepresentativeState
        {
            get { return appStoreInfo.state.value; }
            set { appStoreInfo.state.value = value; }
        }

        public string TradeRepresentativeCountry
        {
            get { return appStoreInfo.country.value; }
            set { appStoreInfo.country.value = value; }
        }

        public string TradeRepresentativePostalCode
        {
            get { return appStoreInfo.postalCode.value; }
            set { appStoreInfo.postalCode.value = value; }
        }

        public string TradeRepresentativePhoneNumber
        {
            get { return appStoreInfo.phoneNumber.value; }
            set { appStoreInfo.phoneNumber.value = value; }
        }

        public string TradeRepresentativeEmail
        {
            get { return appStoreInfo.emailAddress.value; }
            set { appStoreInfo.emailAddress.value = value; }
        }

        public bool TradeRepresentativeIsDisplayedOnAppStore
        {
            get { return appStoreInfo.shouldDisplayInStore.value; }
            set { appStoreInfo.shouldDisplayInStore.value = value; }
        }


        // App Review Information
        public string ReviewFirstName
        {
            get { return appReviewInfo.firstName.value; }
            set { appReviewInfo.firstName.value = value; }
        }

        public string ReviewLastName
        {
            get { return appReviewInfo.lastName.value; }
            set { appReviewInfo.lastName.value = value; }
        }

        public string ReviewPhoneNumber
        {
            get { return appReviewInfo.phoneNumber.value; }
            set { appReviewInfo.phoneNumber.value = value; }
        }

        public string ReviewEmail
        {
            get { return appReviewInfo.emailAddress.value; }
            set { appReviewInfo.emailAddress.value = value; }
        }

        public object ReviewNotes
        {
            get { return appReviewInfo.reviewNotes.value; }
            set { appReviewInfo.reviewNotes.value = value; }
        }

        public bool ReviewUserNeeded
        {
            get { return appReviewInfo.accountRequired.value; }
            set { appReviewInfo.accountRequired.value = value; }
        }

        public string ReviewDemoUser
        {
            get { return appReviewInfo.userName.value; }
            set { appReviewInfo.userName.value = value; }
        }

        public string ReviewDemoPassword
        {
            get { return appReviewInfo.password.value; }
            set { appReviewInfo.password.value = value; }
        }

        public AppVersionRaw _rawData { get; set; }
        public AppVersionRaw RawData
        {
            get
            {
                if (_rawData != null)
                    return _rawData;

                _rawData = new AppVersionRaw {
                    sectionErrorKeys = sectionErrorKeys,
                    sectionInfoKeys = sectionInfoKeys,
                    sectionWarningKeys = sectionWarningKeys,
                    value = value,
                    primaryLanguage = primaryLanguage,
                    submittableAddOns = submittableAddOns,
                    gameCenterSummary = gameCenterSummary,
                    phasedRelease = phasedRelease,
                    canSendVersionLive = canSendVersionLive,
                    canPrepareForUpload = canPrepareForUpload,
                    canRejectVersion = canRejectVersion,
                    externalAppReviewState = externalAppReviewState,
                    appType = appType,
                    platform = platform,
                    transitAppFile = transitAppFile,
                    largeMessagesIcon = largeMessagesIcon,
                    appReviewInfo = appReviewInfo,
                    appStoreInfo = appStoreInfo,
                    appVersionPageLinks = appVersionPageLinks,
                    preReleaseBuild = preReleaseBuild,
                    isSaveError = isSaveError,
                    validationError = validationError,
                    releaseOnApproval = releaseOnApproval,
                    bundleInfo = bundleInfo,
                    autoReleaseDate = autoReleaseDate,
                    marketingOptInEnabled = marketingOptInEnabled,
                    ratingsReset = ratingsReset,
                    isZulu = isZulu,
                    versionId = versionId,
                    name = name,
                    version = version,
                    copyright = copyright,
                    primaryCategory = primaryCategory,
                    primaryFirstSubCategory = primaryFirstSubCategory,
                    primarySecondSubCategory = primarySecondSubCategory,
                    secondaryCategory = secondaryCategory,
                    secondaryFirstSubCategory = secondaryFirstSubCategory,
                    secondarySecondSubCategory = secondarySecondSubCategory,
                    status = status,
                    ratings = ratings,
                    details = details,
                    eula = eula,
                    largeAppIcon = largeAppIcon,
                    watchAppIcon = watchAppIcon,
                    atvHomeScreenIcon = atvHomeScreenIcon,
                    atvTopShelfIcon = atvTopShelfIcon,
                    preReleaseBuildVersionString = preReleaseBuildVersionString,
                    preReleaseBuildTrainVersionString = preReleaseBuildTrainVersionString,
                    preReleaseBuildIconUrl = preReleaseBuildIconUrl,
                    preReleaseBuildUploadDate = preReleaseBuildUploadDate,
                    preReleaseBuildsAreAvailable = preReleaseBuildsAreAvailable,
                    preReleaseBuildIsLegacy = preReleaseBuildIsLegacy,
                    canBetaTest = canBetaTest,
                };
                return _rawData;
            }
        }

        public static AppVersion Find(Application application, string appId, bool isLive)
        {
            // we only support applications
            if (application.type == "BUNDLE")
                throw new Exception("We do not support BUNDLE types right now");

            // too bad the "id" field is empty, it forces us to make more requests to the server
            // these could also be cached
            AppVersion attrs = Globals.TunesClient.AppVersion(appId, isLive);

            if (attrs == null)
                return null;

            attrs.application = application;
            attrs.is_live = isLive;

            return attrs;
        }

        public void Save()
        {
            Globals.TunesClient.UpdateAppVersion(application.appleId, versionId.ToString(), RawData);
        }

        // Uploads or removes the large icon
        // @param iconPath (String): The path to the icon. Use nil to remove it
        public void UploadLargeIcon(string iconPath)
        {
            if (string.IsNullOrEmpty(iconPath))
            {
                _largeAppIcon = new LargeAppIcon();
                return;
            }

            UploadFile uploadImage = UploadFile.FromPath(iconPath);
            var imageData = Globals.TunesClient.UploadLargeIcon(this, uploadImage);

            RawData.largeAppIcon.value = GenerateImageMetadata(imageData, uploadImage.fileName);
        }

        // Uploads or removes the watch icon
        // @param iconPath (String): The path to the icon. Use nil to remove it
        public void UploadWatchIcon(string iconPath)
        {
            if (string.IsNullOrEmpty(iconPath))
            {
                _watchAppIcon = new ImageSharedValues();
                return;
            }

            UploadFile uploadImage = UploadFile.FromPath(iconPath);
            var imageData = Globals.TunesClient.UploadWatchIcon(this, uploadImage);

            RawData.watchAppIcon.value = GenerateImageMetadata(imageData, uploadImage.fileName);
        }

        // TODO: Finish this method
        // Uploads or removes the transit app file
        // @param geojsonPath (String): The path to the geojson file. Use nil to remove it
        public void UploadGeojson(string geojsonPath)
        {
            if (string.IsNullOrEmpty(geojsonPath))
            {
                RawData.transitAppFile.value = null;
                _transitAppFile = null;
                return;
            }

            UploadFile uploadImage = UploadFile.FromPath(geojsonPath);
            var geojsonData = Globals.TunesClient.UploadGeojson(this, uploadImage);

            //@transit_app_file = Tunes::TransitAppFile.factory({}) if @transit_app_file.nil?
            //@transit_app_file.url = nil # response.headers['Location']
            //@transit_app_file.asset_token = geojson_data["token"]
            //@transit_app_file.name = upload_file.file_name
            //@transit_app_file.time_stamp = Time.now.to_i * 1000 # works without but...
        }

        // Uploads or removes a screenshot
        // @param screenshotPath (String): The path to the screenshot. Use nil to remove it
        // @param sortOrder (Fixnum): The sort_order, from 1 to 5
        // @param language (String): The language for this screenshot
        // @param device (string): The device for this screenshot
        // @param isMessages (Bool): True if the screenshot is for iMessage
        public void UploadScreenshot(string screenshotPath, int sortOrder, string language, string device, bool isMessages)
        {
            if (sortOrder < 1)
                throw new Exception("sortOrder must be higher than 0");

            if (sortOrder > 10)
                throw new Exception("sortOrder must be > 10");

            // this will also check both language and device parameters
            var deviceLangScreenshots = ScreenshotsDataForLanguageAndDevice(language, device, isMessages);

            var existingSortOrders = deviceLangScreenshots.Select(dls => dls.value.sortOrder).ToList();

            if (!string.IsNullOrEmpty(screenshotPath))
            {
                // adding or replacing
                UploadFile uploadFile = UploadFile.FromPath(screenshotPath);
                var screenshotData = Globals.TunesClient.UploadScreenshot(this, uploadFile, device, isMessages);

                // Since October 2016 we also need to pass the size, height, width and checksum
                // otherwise App Store Connect validation will fail at a later point
                var newScreenshot = new ScreenshotValue
                {
                    value = new ImageSharedValues
                    {
                        assetToken = screenshotData.token,
                        sortOrder = sortOrder,
                        originalFileName = uploadFile.fileName,
                        size = screenshotData.length,
                        height = screenshotData.height,
                        width = screenshotData.width,
                        checksum = screenshotData.md5
                    }
                };

                // We disable "scaling" for this device type / language combination
                // We only set this, if we actually successfully uploaded a new screenshot
                // for this device / language combination
                // if this value is not set, iTC will fallback to another device type for screenshots
                var languageDetails = RawDataDetails.Find(d => d.language == language).displayFamilies.value;
                var deviceLanguageDetails = languageDetails.Find(dp => dp.name == device);

                if (isMessages)
                    deviceLanguageDetails.messagesScaled.value = false;
                else
                    deviceLanguageDetails.scaled.value = false;

                if (existingSortOrders.Contains(sortOrder))
                {
                    // replace
                    deviceLangScreenshots[existingSortOrders.IndexOf(sortOrder)] = newScreenshot;
                }
                else
                {
                    // add
                    deviceLangScreenshots.Add(newScreenshot);
                }
            }
            else
            {
                // removing
                if (!existingSortOrders.Contains(sortOrder))
                    throw new Exception("cannot remove screenshot with non existing sortOrder");

                deviceLangScreenshots.RemoveAt(existingSortOrders.IndexOf(sortOrder));
            }

            SetupScreenshots();
        }

        // Uploads, removes a trailer video
        //
        // A preview image for the video is required by ITC and is usually automatically extracted by your browser.
        // This method will either automatically extract it from the video (using `ffmpeg`) or allow you
        // to specify it using +preview_image_path+.
        // If the preview image is specified, `ffmpeg` will not be used. The image resolution will be checked against
        // expectations (which might be different from the trailer resolution.
        //
        // It is recommended to extract the preview image using the spaceship related tools in order to ensure
        // the appropriate format and resolution are used.
        //
        // Note: to extract its resolution and a screenshot preview, the `ffmpeg` tool will be used
        //
        // @param trailerPath (String): The path to the trailer. Use nil to remove it
        // @param sortOrder (Fixnum): The sort_order, from 1 to 5
        // @param language (String): The language for this screenshot
        // @param device (String): The device for this screenshot
        // @param timestamp (String): The optional timestamp of the screenshot to grab
        // @param previewImagePath (String): The optional image path for the video preview
        public void UploadTrailer(string trailerPath, int sortOrder, string language, string device, string timestamp = "05.00", string previewImagePath = null)
        {
            string videoPreviewPath = "";

            if (device == "iphone35")
                throw new Exception("No app trailer supported for iphone35");

            if (sortOrder <= 0)
                throw new Exception("sortOrder must be higher than 0");

            if (sortOrder > 3)
                throw new Exception("sortOrder must not be > 3");

            var deviceLangTrailers = TrailerDataForLanguageAndDevice(language, device);
            var existingSortOrders = deviceLangTrailers.Select(deviceLangTrailer => deviceLangTrailer.value.sortPosition).ToList();

            if (string.IsNullOrEmpty(trailerPath)) // adding / replacing trailer
            {
                if (!Regex.IsMatch(timestamp, @"^[0-9][0-9].[0-9][0-9]$"))
                    throw new Exception($"Invalid timestamp {timestamp}");

                if (string.IsNullOrEmpty(previewImagePath))
                {
                    CheckPreviewScreenshotResolution(previewImagePath, device);
                    videoPreviewPath = previewImagePath;
                }
                else
                {
                    // IDEA: optimization, we could avoid fetching the screenshot if the timestamp hasn't changed
                    int[] videoPreviewResolution = VideoPreviewResolutionFor(device, trailerPath);

                    // Keep a reference of the video_preview here to avoid Ruby getting rid of the Tempfile in the meanwhile
                    var videoPreview = Utilities.GrabVideoPreview(trailerPath, timestamp, videoPreviewResolution[0], videoPreviewResolution[1]);
                    videoPreviewPath = videoPreview.FullName;
                }

                var videoPreviewFile = UploadFile.FromPath(videoPreviewPath);
                var videoPreviewData = Globals.TunesClient.UploadTrailerPreview(this, videoPreviewFile, device);

                var uploadFile = UploadFile.FromPath(trailerPath);
                var trailerData = Globals.TunesClient.UploadTrailer(this, uploadFile);

                string ts = $"00:00:{timestamp}";
                // replace . with :
                StringBuilder sb = new StringBuilder(ts);
                sb[8] = ':';
                ts = sb.ToString();

                var newTrailer = new TrailerValue
                {
                    value = new AppTrailer {
                        videoAssetToken = trailerData.responses[0].token,
                        descriptionXML = trailerData.responses[0].descriptionDoc,
                        contentType = uploadFile.contentType,
                        sortPosition = sortOrder,
                        size = videoPreviewData.length,
                        width = videoPreviewData.width,
                        height = videoPreviewData.height,
                        checksum = videoPreviewData.md5,
                        pictureAssetToken = videoPreviewData.token,
                        previewFrameTimeCode = ts.ToString(),
                        isPortrait = Utilities.IsPortrait(videoPreviewPath)
                    }
                };

                if (existingSortOrders.Contains(sortOrder)) // replace
                    deviceLangTrailers[existingSortOrders.IndexOf(sortOrder)] = newTrailer;
                else // add
                    deviceLangTrailers.Add(newTrailer);
            }
            else // removing trailer
            {
                if (!existingSortOrders.Contains(sortOrder))
                    throw new Exception("cannot remove trailer with non existing sort_order");

                deviceLangTrailers.RemoveAt(existingSortOrders.IndexOf(sortOrder));
            }

            SetupTrailers();
        }

        // Uploads, app review attachments
        //
        // while submitting for review, ITC allow developers to attach file.
        //
        // Following list can be found at https://appstoreconnect.apple.com
        // on iOS app edit version, above the attachment label/button there is
        // a question mark if it is press the a dialog is shown which has the list.
        //
        // File types allowed by Apple are: pdf, doc, docx, rtf, pages, xls, xlsx, numbers
        // zip, rar, plist, crash, jpg, png, mp4 or avi.
        //
        //
        // @param reviewAttachmentPath (String): The path to the attachment file.
        public void UploadReviewAttachment(string reviewAttachmentPath)
        {
            if (is_live)
                throw new Exception("cannot upload review attachment for live edition.");

            if (string.IsNullOrEmpty(reviewAttachmentPath))
            {
                _reviewAttachmentFile = new AttachmentFile();
                return;
            }

            if (!File.Exists(reviewAttachmentPath))
                throw new Exception($"cannot find file: {reviewAttachmentPath}");

            var reviewAttachmentFile = UploadFile.FromPath(reviewAttachmentPath);
            var reviewAttachmentData = Globals.TunesClient.UploadAppReviewAttachment(this, reviewAttachmentFile);

            RawData.appReviewInfo.attachmentFiles.value = GenerateReviewAttachmentFile(reviewAttachmentData, reviewAttachmentPath);
        }

        public void SetupTrailers()
        {
            RawDataDetails.ForEach(dataDetails =>
            {
                // Now that's one language right here
                trailers[dataDetails.language] = SetupTrailersFor(dataDetails);
            });
        }

        // generates the nested data structure to represent trailers
        public List<AppTrailer> SetupTrailersFor(DetailsValue row)
        {
            if (row == null || row.displayFamilies == null)
                return null;

            var displayFamilies = row?.displayFamilies?.value;
            if (displayFamilies == null)
                return null;

            List<AppTrailer> result = new List<AppTrailer>();

            displayFamilies.ForEach(displayFamily =>
            {
                displayFamily?.trailers?.value.ForEach(trailer =>
                {
                    AppTrailer trailerData = trailer.value;
                    trailerData.deviceType = displayFamily.name;
                    trailerData.language = row.language;

                    result.Add(trailerData);
                });
            });

            return result;
        }

        public void SetupScreenshots()
        {
            // Enable Scaling for all screen sizes that don't have at least one screenshot or at least one trailer (app_preview)
            // We automatically disable scaling once we upload at least one screenshot
            RawDataDetails.ForEach(currentLanguage =>
            {
                List<DisplayFamiliesValue> languageDetails = currentLanguage.displayFamilies.value;
                List<DisplayFamiliesValue> loopList = new List<DisplayFamiliesValue>();

                languageDetails.AddRange(loopList);

                foreach (var deviceLanguageDetails in languageDetails)
                {
                    // Do not enable scaling if a screenshot already exists
                    if (deviceLanguageDetails.screenshots == null)
                        continue;

                    if (deviceLanguageDetails.screenshots.value.Count > 0)
                        continue;

                    // Do not enable scaling if a trailer already exists
                    if (deviceLanguageDetails.trailers == null)
                        continue;

                    if (deviceLanguageDetails.trailers.value.Count > 0)
                        continue;

                    // The current row includes screenshots for all device types
                    // so we need to enable scaling for both iOS and watchOS apps
                    if (deviceLanguageDetails.scaled != null)
                        deviceLanguageDetails.scaled.value = true;

                    if (deviceLanguageDetails.messagesScaled != null)
                        deviceLanguageDetails.messagesScaled.value = true;
                    // we unset `scaled` or `messagesScaled` as soon as we upload a
                    // screenshot for this device/language combination
                }
            });

            foreach (var dataDetails in RawDataDetails)
            {
                // Now that's one language right here
                var collectedScreenshots = SetupScreenshotsFor(dataDetails);
                collectedScreenshots.AddRange(SetupMessagesScreenshotsFor(dataDetails));
                screenshots[dataDetails.language] = collectedScreenshots;
            }
        }

        // generates the nested data structure to represent screenshots
        public List<AppScreenshot> SetupScreenshotsFor(DetailsValue dataDetails)
        {
            if (dataDetails == null || dataDetails.displayFamilies == null)
                return null;

            var displayFamilies = dataDetails?.displayFamilies?.value;
            if (displayFamilies == null)
                return null;

            var result = new List<AppScreenshot>();

            displayFamilies.ForEach(displayFamily =>
            {
                // {
                //   "name": "iphone6Plus",
                //   "scaled": {
                //     "value": false,
                //     "isEditable": false,
                //     "isRequired": false,
                //     "errorKeys": null
                //   },
                //   "screenshots": {
                //     "value": [{
                //       "value": {
                //         "assetToken": "Purple62/v4/08/0a/04/080a0430-c2cc-2577-f491-9e0a09c58ffe/mzl.pbcpzqyg.jpg",
                //         "sortOrder": 1,
                //         "type": null,
                //         "originalFileName": "ios-414-1.jpg"
                //       },
                //       "isEditable": true,
                //       "isRequired": false,
                //       "errorKeys": null
                //     }, {
                //       "value": {
                //         "assetToken": "Purple71/v4/de/81/aa/de81aa10-64f6-332e-c974-9ee46adab675/mzl.cshkjvwl.jpg",
                //         "sortOrder": 2,
                //         "type": null,
                //         "originalFileName": "ios-414-2.jpg"
                //       },
                //       "isEditable": true,
                //       "isRequired": false,
                //       "errorKeys": null
                //     }],
                //   "messagesScaled": {
                //     "value": false,
                //     "isEditable": false,
                //     "isRequired": false,
                //     "errorKeys": null
                //   },
                //   "messagesScreenshots": {
                //     "value": [{
                //       "value": {
                //         "assetToken": "Purple62/v4/08/0a/04/080a0430-c2cc-2577-f491-9e0a09c58ffe/mzl.pbcpzqyg.jpg",
                //         "sortOrder": 1,
                //         "type": null,
                //         "originalFileName": "ios-414-1.jpg"
                //       },
                //       "isEditable": true,
                //       "isRequired": false,
                //       "errorKeys": null
                //     }, {
                //       "value": {
                //         "assetToken": "Purple71/v4/de/81/aa/de81aa10-64f6-332e-c974-9ee46adab675/mzl.cshkjvwl.jpg",
                //         "sortOrder": 2,
                //         "type": null,
                //         "originalFileName": "ios-414-2.jpg"
                //       },
                //       "isEditable": true,
                //       "isRequired": false,
                //       "errorKeys": null
                //     }],
                //     "isEditable": true,
                //     "isRequired": false,
                //     "errorKeys": null
                //   },
                //   "trailer": {
                //     "value": null,
                //     "isEditable": true,
                //     "isRequired": false,
                //     "errorKeys": null
                //   }
                // }

                displayFamily?.screenshots?.value.ForEach(screenshot =>
                {
                    var screenshotData = screenshot.value;
                    var data = JObject.FromObject(screenshotData).ToObject<AppScreenshot>();
                    data.deviceType = displayFamily.name;
                    data.language = dataDetails.language;

                    result.Add(data);
                });
            });

            return result;
        }

        public List<TrailerValue> TrailerDataForLanguageAndDevice(string language, string device)
        {
            var result = ContainerDataForLanguageAndDevice("trailers", language, device);

            return JObject.Parse(result).ToObject<List<TrailerValue>>();
        }

        // generates the nested data structure to represent screenshots
        public List<AppScreenshot> SetupMessagesScreenshotsFor(DetailsValue dataDetails)
        {
            if (dataDetails == null || dataDetails.displayFamilies == null)
                return null;

            var displayFamilies = dataDetails?.displayFamilies?.value;
            if (displayFamilies == null)
                return null;

            var result = new List<AppScreenshot>();

            foreach (var displayFamily in displayFamilies)
            {
                var displayFamilyScreenshots = displayFamily?.messagesScreenshots;
                if (displayFamilyScreenshots == null)
                    continue;

                displayFamilyScreenshots?.value.ForEach(screenshot =>
                {
                    var screenshotData = screenshot.value;
                    var data = JObject.FromObject(screenshotData).ToObject<AppScreenshot>();
                    data.deviceType = displayFamily.name;
                    data.language = dataDetails.language;
                    data.isImessage = true; // to identify imessage screenshots later on (e.g: during download)

                    result.Add(data);
                });
            };

            return result;
        }

        public List<ScreenshotValue> ScreenshotsDataForLanguageAndDevice(string language, string device, bool isMessages)
        {
            var dataField = isMessages ? "messagesScreenshots" : "screenshots";
            var result = ContainerDataForLanguageAndDevice(dataField, language, device);

            return JObject.Parse(result).ToObject<List<ScreenshotValue>>();
        }

        public string ContainerDataForLanguageAndDevice(string dataField, string language, string device)
        {
            try
            {
                if (DisplayFamily.Find(device) == null)
                    throw new Exception($"{device} isn't a valid device name");

                var languages = RawDataDetails.FindAll(d => d.language == language);

                // IDEA: better error for non existing language
                if (languages.Count() < 1)
                    throw new Exception($"{language} isn't an activated language");

                var langDetails = languages.First();
                var displayFamilies = langDetails.displayFamilies.value;
                var deviceDetails = displayFamilies.Find(df => df.name == device);

                if (deviceDetails == null)
                    throw new Exception($"Couldn't find device family for {device}");

                if (deviceDetails.GetType().GetProperty(dataField) == null)
                    throw new Exception($"Unexpected state: missing device details for {device}");

                var jsonSerializer = new JsonSerializer();
                StringWriter textWriter = new StringWriter();

                if (dataField == "messagesScreenshots")
                {
                    jsonSerializer.Serialize(textWriter, deviceDetails.messagesScreenshots.value);
                    return textWriter.ToString();
                    //return deviceDetails.messagesScreenshots.value;
                }
                else if (dataField == "trailers")
                {
                    jsonSerializer.Serialize(textWriter, deviceDetails.trailers.value);
                    return textWriter.ToString();
                    //return deviceDetails.trailers.value;
                }
                else
                {
                    jsonSerializer.Serialize(textWriter, deviceDetails.screenshots.value);
                    return textWriter.ToString();
                    //return deviceDetails.screenshots.value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"App Store Connect error: {ex.Message}");
            }
        }

        // ensure the specified preview screenshot has the expected resolution the specified target +device+
        public void CheckPreviewScreenshotResolution(string previewScreenshotPath, string device)
        {
            bool isPortrait = Utilities.IsPortrait(previewScreenshotPath);
            int[] expectedResolution = NatukashipTunesClient.VideoPreviewResolutionFor(device, isPortrait);
            int[] actualResolution = Utilities.Resolution(previewScreenshotPath);

            string orientation = isPortrait ? "portrait" : "landscape";


            if (actualResolution != expectedResolution)
                throw new Exception($"Invalid {orientation} screenshot resolution for device {device}. Should be {string.Join(" x ", expectedResolution)}");
        }

        // identify the required resolution for this particular video screenshot
        public int[] VideoPreviewResolutionFor(string device, string videoPath)
        {
            bool isPortrait = Utilities.IsPortrait(videoPath);
            return NatukashipTunesClient.VideoPreviewResolutionFor(device, isPortrait);
        }

        // This method will generate the required keys/values
        // for App Store Connect to validate the uploaded image
        private LargeAppIcon GenerateImageMetadata(dynamic imageData, string originalFileName)
        {
            return new LargeAppIcon
            {
                assetToken = imageData.token,
                originalFileName = originalFileName,
                size = imageData.length,
                height = imageData.height,
                width = imageData.width,
                checksum = imageData.md5
            };
        }

        // This method will generate the required keys/values
        // for App Store Connect to validate the review attachment file
        public AttachmentFile GenerateReviewAttachmentFile(UploadFileResponseObject reviewAttachmentData, string reviewAttachmentFile)
        {
            return new AttachmentFile
            {
                assetToken = reviewAttachmentData.token,
                name = Path.GetFileName(reviewAttachmentFile),
                fileType = Utilities.ContentType(reviewAttachmentFile),
                url = ""
            };
        }

        public List<DetailsValue> RawDataDetails
        {
            get
            {
                return RawData.details.value;
            }
        }

        public void Reject()
        {
            if (!canRejectVersion)
                throw new Exception("Version not rejectable");

            Globals.TunesClient.Reject(application.appleId, versionId);
        }
    }

    public class AppVersionRaw
    {
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public object value { get; set; }
        public object primaryLanguage { get; set; }
        public SubmittableAddOns submittableAddOns { get; set; }
        public GameCenterSummary gameCenterSummary { get; set; }
        public PhasedRelease phasedRelease { get; set; }
        public bool canSendVersionLive { get; set; }
        public bool canPrepareForUpload { get; set; }
        public bool canRejectVersion { get; set; }
        public string externalAppReviewState { get; set; }
        public string appType { get; set; }
        public string platform { get; set; }
        public TransitAppFile transitAppFile { get; set; }
        public ImageSharedValues largeMessagesIcon { get; set; }
        public AppReviewInfo appReviewInfo { get; set; }
        public AppStoreInfo appStoreInfo { get; set; }
        public Dictionary<string, string> appVersionPageLinks { get; set; }
        public PreReleaseBuild preReleaseBuild { get; set; }
        public bool isSaveError { get; set; }
        public bool validationError { get; set; }
        public ReleaseOnApproval releaseOnApproval { get; set; }
        public BundleInfo bundleInfo { get; set; }
        public AutoReleaseDate autoReleaseDate { get; set; }
        public object marketingOptInEnabled { get; set; }
        public RatingsReset ratingsReset { get; set; }
        public bool isZulu { get; set; }
        public int versionId { get; set; }
        public object name { get; set; }
        public Version version { get; set; }
        public Copyright copyright { get; set; }
        public PrimaryCategory primaryCategory { get; set; }
        public PrimaryFirstSubCategory primaryFirstSubCategory { get; set; }
        public PrimarySecondSubCategory primarySecondSubCategory { get; set; }
        public SecondaryCategory secondaryCategory { get; set; }
        public SecondaryFirstSubCategory secondaryFirstSubCategory { get; set; }
        public SecondarySecondSubCategory secondarySecondSubCategory { get; set; }
        public string status { get; set; }
        public Ratings ratings { get; set; }
        public Details details { get; set; }
        public object eula { get; set; }
        public LargeAppIconValue largeAppIcon { get; set; }
        public WatchAppIcon watchAppIcon { get; set; }
        public ImageSharedValues atvHomeScreenIcon { get; set; }
        public ImageSharedValues atvTopShelfIcon { get; set; }
        public PreReleaseBuildVersionString preReleaseBuildVersionString { get; set; }
        public string preReleaseBuildTrainVersionString { get; set; }
        public string preReleaseBuildIconUrl { get; set; }
        public long preReleaseBuildUploadDate { get; set; }
        public bool preReleaseBuildsAreAvailable { get; set; }
        public bool preReleaseBuildIsLegacy { get; set; }
        public bool canBetaTest { get; set; }
    }
}
