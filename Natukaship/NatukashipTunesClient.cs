using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Natukaship.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Natukaship
{
    // Tunes - Client
    // spaceship/lib/spaceship/tunes/tunes_client.rb
    public class NatukashipTunesClient : NatukashipClient
    {
        public AccountManager account;
        private string cwd = Directory.GetCurrentDirectory();
        public new List<AssociatedAccount> teams;
        public NatukashipDUClient DUClient;

        public static int[] VideoPreviewResolutionFor(string device, bool isPortrait)
        {
            Dictionary<string, int[]> resolutions = new Dictionary<string, int[]>
            {
                { "iphone4", new int[] { 1136, 640 } },
                { "iphone6", new int[] { 1334, 750 } },
                { "iphone6Plus", new int[] { 2208, 1242 } },
                { "iphone58", new int[] { 2436, 1125 } },
                { "iphone65", new int[] { 2688, 1242 } },
                { "ipad", new int[] { 1024, 768 } },
                { "ipad105", new int[] { 2224, 1668 } },
                { "ipadPro", new int[] { 2732, 2048 } },
                { "ipadPro11", new int[] { 2388, 1668 } },
                { "ipadPro129", new int[] { 2732, 2048 } }
            };

            int[] resolution = resolutions[device];

            if (isPortrait)
                resolution = new int[] { resolution[1], resolution[0] };

            return resolution;
        }

        //////////////////////////
        /// @!group Init and Login
        //////////////////////////

        #region Init & Login
        public NatukashipTunesClient(NatukashipSetting setting) : base(setting)
        {
            DUClient = new NatukashipDUClient(setting);
            Globals.TunesClient = this;
            Globals.DUClient = DUClient;
        }

        public string Hostname { get => "https://appstoreconnect.apple.com/WebObjects/iTunesConnect.woa"; }

        /// <summary>
        /// </summary>
        /// <param name="username">username (Optional)</param>
        /// <param name="password">password (Optional)</param>
        public async Task Login(string username = null, string password = null)
        {
            try
            {
                account = new AccountManager(username, password);
                DUClient.Account = account;
                User = account.User;
                Password = account.Password;

                if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password))
                {
                    throw new NoUserCredentialsException("No user credentials provided!");
                }

                await account.SendLoginRequestAsync();
                IsLoggedIn = account.IsLoggedIn;
                DUClient.IsLoggedIn = IsLoggedIn;

                if (IsLoggedIn)
                {
                    teams = Teams;
                    teamId = teams[0].contentProvider.contentProviderId.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        //////////////////////////
        /// @!group Teams + User
        //////////////////////////

        #region Teams + User
        /// Fetch the general information of the user, is used by various methods across spaceship
        public UserData UserDetailsData
        {
            get
            {
                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/user/detail"));
                (string responseResult, int responseCode) = task.Result;

                TunesUserDetailResponseObject responseObject = JObject.Parse(responseResult).ToObject<TunesUserDetailResponseObject>();

                return responseObject.data;
            }
        }

        /// @return (List) A list of all available teams
        public List<AssociatedAccount> Teams
        {
            get
            {
                return UserDetailsData.associatedAccounts
                    .OrderBy(x => x.contentProvider.name)
                    .ThenBy(x => x.contentProvider.contentProviderId).ToList();
            }
        }
        #endregion

        //////////////////////////
        /// @!group Applications
        //////////////////////////

        #region Applications
        public List<Application> Applications()
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/manageyourapps/summary/v2"));
                (string responseResult, int responseCode) = task.Result;

                ManageAppsResponseObject responseObject = JObject.Parse(responseResult).ToObject<ManageAppsResponseObject>();

                return responseObject.data.summaries;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public AppDetail AppDetails(string appId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/{appId}/details"));
                (string responseResult, int responseCode) = task.Result;

                AppDetailsResponseObject responseObject = JObject.Parse(responseResult).ToObject<AppDetailsResponseObject>();

                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public AppDetail UpdateAppDetails(string appId, AppDetail appData)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                JObject.FromObject(appData).ToString();

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/ra/apps/{appId}/details", appData));
                (string responseResult, int responseCode) = task.Result;

                HandleItcResponse(responseResult);

                // That's alright, we get this error message if nothing has changed
                if (!responseResult.Contains("operation_failed"))
                {
                    AppDetailsResponseObject responseObject = JObject.Parse(responseResult).ToObject<AppDetailsResponseObject>();

                    return responseObject.data;
                }

                return new AppDetail();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Creates a new application on App Store Connect
        /// <param name="name"> The name of your app as it will appear on the App Store. This can't be longer than 255 characters.</param>
        /// <param name="primaryLanguage"> If localized app information isn't available in an App Store territory, the information from your primary language will be used instead.</param>
        /// <param name="version"> *DEPRECATED* The version number is shown on the App Store and should match the one you used in Xcode.</param>
        /// <param name="sku"> A unique ID for your app that is not visible on the App Store.</param>
        /// <param name="bundle_id"> The bundle ID must match the one you used in Xcode. It can't be changed after you submit your first build.</param>
        /// </summary>
        public CreateApplication CreateApplication(string name = null,
                                      string primaryLanguage = null,
                                      string version = null,
                                      string sku = null,
                                      string bundleId = null,
                                      string bundleIdSuffix = null,
                                      string companyName = null,
                                      string platform = null,
                                      List<string> platforms = null,
                                      string itunesConnectUsers = null)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (version != null)
                    Console.WriteLine("The `version` parameter is deprecated. Use `Application.ensure_version!` method instead");

                // First, we need to fetch the data from Apple, which we then modify with the user's values
                var primaryLang = "";
                if (primaryLanguage != null)
                    primaryLang = primaryLanguage;
                else
                    primaryLang = "English";

                if (string.IsNullOrEmpty(platform))
                    platform = "ios";

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/create/v2/?platformString={platform}"));
                (string responseResult, int responseCode) = task.Result;

                GetCreateApplicationResponseObject responseObject = JObject.Parse(responseResult).ToObject<GetCreateApplicationResponseObject>();
                var data = responseObject.data;

                // Now fill in the values we have
                // some values are nil, that's why there is a hash
                data.name = new ApplicationName { value = name };
                data.bundleId = new BundleId { value = bundleId };
                data.primaryLanguage = new { value = primaryLang };
                data.primaryLocaleCode = new PrimaryLocaleCode { value = primaryLang.ToItcLocale() };
                data.vendorId = new VendorId { value = sku };
                data.bundleIdSuffix = new BundleIdSuffix { value = bundleIdSuffix };
                if (!string.IsNullOrEmpty(companyName))
                    data.companyName = new CompanyName { value = companyName };
                data.enabledPlatformsForCreation = new EnabledPlatformsForCreation { value = new List<string> { platform } };

                data.initialPlatform = "ios";
                var supposedPlatforms = platforms ?? new List<string> { platform };
                data.enabledPlatformsForCreation = new EnabledPlatformsForCreation { value = supposedPlatforms };

                if (itunesConnectUsers != null)
                {
                    /// Note: There's a closed issue regarding this --> https://github.com/fastlane/fastlane/issues/13540
                    /// I've commented this since I think we wouldn't add users through here
                    //data.iTunesConnectUsers.grantedAllUsers = false;
                    //data.iTunesConnectUsers.grantedUsers = data.iTunesConnectUsers.availableUsers.Where(user => itunesConnectUsers.Contains(user.username)).Select(user => user);
                }

                // Now send back the modified hash
                Task<(string, int)> createTask = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/ra/apps/create/v2", data));
                (string createResponseResult, int createResponseCode) = createTask.Result;

                HandleItcResponse(createResponseResult);

                CreateApplicationResponseObject createResponseObject = JObject.Parse(createResponseResult).ToObject<CreateApplicationResponseObject>();

                return createResponseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // TODO: Finish this method
        public void CreateVersion(string appId, string versionNumber, string platform = "ios")
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                var data = new
                {
                    version = new
                    {
                        value = versionNumber
                    }
                };

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/ra/apps/{appId}/platforms/{platform}/versions/create/", data));
                (string responseResult, int responseCode) = task.Result;

                HandleItcResponse(responseResult);

                // TODO: Finish this method; Can't try since 1 version per platform only
                //var responseObject = JObject.Parse(responseResult).ToObject<>();
                //return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public BundleData GetAvailableBundleIds(string platform = "ios")
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/create/v2/?platformString={platform}"));
                (string responseResult, int responseCode) = task.Result;

                GetAvailableBundleIdsResponseObject responseObject = JObject.Parse(responseResult).ToObject<GetAvailableBundleIdsResponseObject>();

                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public ResolutionData GetResolutionCenter(string appId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/{appId}/platforms/ios/resolutionCenter?v=latest"));
                (string responseResult, int responseCode) = task.Result;

                GetResolutionResponseObject responseObject = JObject.Parse(responseResult).ToObject<GetResolutionResponseObject>();

                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public ResolutionData PostResolutionCenter(string appId, string threadId, string versionId, string versionNumber, string from, string messageBody)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                object thread = new
                {
                    id = threadId,
                    versionId,
                    version = versionNumber,
                    messages = new object[] { new
                        {
                            from,
                            date = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                            body = messageBody,
                            tokens = new object[] { }
                        }
                    }
                };

                object data = new
                {
                    appNotes = new
                    {
                        threads = new object[] { thread }
                    }
                };

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/ra/apps/{appId}/platforms/ios/resolutionCenter", data));
                (string responseResult, int responseCode) = task.Result;

                HandleItcResponse(responseResult);

                GetResolutionResponseObject responseObject = JObject.Parse(responseResult).ToObject<GetResolutionResponseObject>();

                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public AppRatingsData GetRatings(string appId, string storeFront = "", string versionId = "")
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(storeFront) && string.IsNullOrEmpty(versionId))
                    throw new ArgumentNullException("storeFront or versionId is required");

                object parameters = new { };
                string ratingsUrl = $"{Hostname}/ra/apps/{appId}/platforms/ios/reviews/summary?";

                if (!string.IsNullOrEmpty(storeFront))
                    ratingsUrl += $"storefront={storeFront}";

                if (!string.IsNullOrEmpty(versionId))
                    ratingsUrl += $"version_id={versionId}";


                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", ratingsUrl));
                (string responseResult, int responseCode) = task.Result;

                AppRatingsResponseObject responseObject = JObject.Parse(responseResult).ToObject<AppRatingsResponseObject>();

                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // upToDate Format: MM/dd/yyyy
        public List<Review> GetReviews(string appId, string storeFront, string versionId, string upToDate = "")
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                int idx = 0;
                int perPage = 100; // apple default
                List<Review> allReviews = new List<Review>();
                string ratingsUrl = "";
                DateTime dateTime = DateTime.UtcNow;

                if (!string.IsNullOrEmpty(upToDate))
                    dateTime = DateTime.Parse(upToDate).ToUniversalTime();

                while (true)
                {
                    ratingsUrl = $"{Hostname}/ra/apps/{appId}/platforms/ios/reviews?";
                    ratingsUrl += $"sort=REVIEW_SORT_ORDER_MOST_RECENT";
                    ratingsUrl += $"&index={idx}";

                    if (!string.IsNullOrEmpty(storeFront))
                        ratingsUrl += $"&storefront={storeFront}";

                    if (!string.IsNullOrEmpty(versionId))
                        ratingsUrl += $"&versionId={versionId}";

                    Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", ratingsUrl));
                    (string responseResult, int responseCode) = task.Result;

                    AppReviewsResponseObject responseObject = JObject.Parse(responseResult).ToObject<AppReviewsResponseObject>();

                    allReviews.AddRange(responseObject.data.reviews);

                    // The following lines throw errors when there are no reviews so exit out of the loop before them if the app has no reviews
                    if (allReviews.Count == 0)
                        break;

                    DateTime lastReviewDate = DateTime.FromOADate(allReviews.Last().value.lastModified / 1000).ToUniversalTime();

                    if (!string.IsNullOrEmpty(upToDate) && lastReviewDate < dateTime)
                    {
                        allReviews = allReviews.Where(review => DateTime.FromOADate(review.value.lastModified / 1000).ToUniversalTime() > dateTime).ToList();
                        break;
                    }

                    if (allReviews.Count < responseObject.data.reviewCount)
                        idx += perPage;
                    else
                        break;
                }

                return allReviews;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        //////////////////////////
        /// @!group AppVersions
        //////////////////////////

        #region AppVersions
        public AppVersion AppVersion(string appId, bool isLive = false)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    return null;

                Task<(string, int)> overviewTask = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/{appId}/overview"));
                (string overviewResponseResult, int overviewResponseCode) = overviewTask.Result;

                GetAppVersionOverviewResponseObject overviewResponseObject = JObject.Parse(overviewResponseResult).ToObject<GetAppVersionOverviewResponseObject>();

                Platform platform = overviewResponseObject.data.platforms.FirstOrDefault(p => p.platformString == "ios");

                if (platform == null)
                    return null;

                string versionId = null;
                switch (isLive)
                {
                    case true:
                        versionId = platform.deliverableVersion.id;
                        break;
                    case false:
                        versionId = platform.inFlightVersion.id;
                        break;
                }

                if (versionId == null)
                    return null;

                string versionPlatform = platform.platformString;

                if (string.IsNullOrEmpty(versionPlatform))
                    throw new Exception("versionPlatform is required");

                if (string.IsNullOrEmpty(versionId))
                    throw new Exception("versionId is required");

                Task<(string, int)> versionTask = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/{appId}/platforms/ios/versions/{versionId}"));
                (string versionResponseResult, int versionResponseCode) = versionTask.Result;

                GetAppVersionResponseObject versionResponseObject = JsonConvert.DeserializeObject<GetAppVersionResponseObject>(versionResponseResult, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                return versionResponseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public AppVersion UpdateAppVersion(string appId, string versionId, object data)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId is required");

                if (string.IsNullOrEmpty(versionId))
                    throw new Exception("versionId is required");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/ra/apps/{appId}/platforms/ios/versions/{versionId}", data));
                (string responseResult, int responseCode) = task.Result;

                HandleItcResponse(responseResult);

                GetAppVersionResponseObject responseObject = JsonConvert.DeserializeObject<GetAppVersionResponseObject>(responseResult, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        //////////////////////////
        /// @!group Members
        //////////////////////////

        #region Members
        public MembersData Members()
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/users/itc"));

                (string responseResult, int responseCode) = task.Result;
                GetMembersResponseObject responseObject = JObject.Parse(responseResult).ToObject<GetMembersResponseObject>();

                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public MemberInvitationResponseObject ReInviteMember(string email)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/ra/users/itc/{email}/resendInvitation"));
                (string responseResult, int responseCode) = task.Result;

                return JObject.Parse(responseResult).ToObject<MemberInvitationResponseObject>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public MembersData DeleteMember(string userId, string email)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                object[] payload = new object[] { new { dsId = userId, email } };

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/ra/users/itc/delete", payload));
                (string responseResult, int responseCode) = task.Result;

                GetMembersResponseObject responseObject = JObject.Parse(responseResult).ToObject<GetMembersResponseObject>();
                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public CreateMemberData CreateMember(string firstName, string lastName, string emailAddress, string[] roles = null, string[] apps = null)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/users/itc/create"));
                (string responseResult, int responseCode) = task.Result;

                GetCreateMemberResponseObject responseObject = JObject.Parse(responseResult).ToObject<GetCreateMemberResponseObject>();
                CreateMemberData data = responseObject.data;

                data.user.firstName = new MemberFirstName { value = firstName };
                data.user.lastName = new MemberLastName { value = lastName };
                data.user.emailAddress = new MemberEmailAddress { value = emailAddress };

                if (roles == null || roles?.Length == 0)
                    roles = new string[] { "admin" };

                data.user.roles = new List<UserRole>();

                foreach (var role in roles)
                {
                    // find role from template
                    foreach (var tempRole in data.roles)
                    {
                        if (tempRole.value.name == role)
                            data.user.roles.Add(tempRole);
                    }
                }

                if (apps == null || apps?.Length == 0)
                {
                    data.user.userSoftwares = new UserSoftwares
                    {
                        value = new UserSoftwaresValue
                        {
                            grantAllSoftware = true,
                            grantedSoftwareAdamIds = new List<string> { }
                        }
                    };
                }
                else
                {
                    data.user.userSoftwares = new UserSoftwares
                    {
                        value = new UserSoftwaresValue
                        {
                            grantAllSoftware = false,
                            grantedSoftwareAdamIds = apps.ToList()
                        }
                    };
                }

                // send the changes back to Apple
                Task<(string, int)> createTask = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/ra/users/itc/create", data));
                (string createResponseResult, int createResponseCode) = createTask.Result;

                HandleItcResponse(createResponseResult);

                GetCreateMemberResponseObject createResponseObject = JObject.Parse(createResponseResult).ToObject<GetCreateMemberResponseObject>();
                return createResponseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public MemberRolesData UpdateMemberRoles(UserMember member, string[] roles = null, string[] apps = null)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/users/itc/{member.dsId}/roles"));
                (string responseResult, int responseCode) = task.Result;

                GetMemberRolesResponseObject responseObject = JObject.Parse(responseResult).ToObject<GetMemberRolesResponseObject>();
                MemberRolesData data = responseObject.data;

                if (roles == null || roles?.Length == 0)
                    roles = new string[] { "admin" };

                data.user.roles = new List<UserRole>();

                foreach (var role in roles)
                {
                    // find role from template
                    foreach (var tempRole in data.roles)
                    {
                        if (tempRole.value.name == role)
                            data.user.roles.Add(tempRole);
                    }
                }

                if (apps == null || apps?.Length == 0)
                {
                    data.user.userSoftwares = new UserSoftwares
                    {
                        value = new UserSoftwaresValue
                        {
                            grantAllSoftware = true,
                            grantedSoftwareAdamIds = new List<string> { }
                        }
                    };
                }
                else
                {
                    data.user.userSoftwares = new UserSoftwares
                    {
                        value = new UserSoftwaresValue
                        {
                            grantAllSoftware = false,
                            grantedSoftwareAdamIds = apps.ToList()
                        }
                    };
                }

                // send the changes back to Apple
                Task<(string, int)> updateTask = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/ra/users/itc/{member.dsId}/roles", data));
                (string updateResponseResult, int updateResponseCode) = updateTask.Result;

                HandleItcResponse(updateResponseResult);

                GetMemberRolesResponseObject updateResponseObject = JObject.Parse(updateResponseResult).ToObject<GetMemberRolesResponseObject>();
                return updateResponseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        //////////////////////////
        /// @!group Pricing
        //////////////////////////

        #region Pricing
        public PriceTierAvailability UpdatePriceTier(string appId, string priceTier)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/{appId}/pricing/intervals"));
                (string responseResult, int responseCode) = task.Result;

                PriceTierAvailabilityResponseObject responseObject = JObject.Parse(responseResult).ToObject<PriceTierAvailabilityResponseObject>();
                var data = responseObject.data;

                // first price
                bool firstPrice = data.pricingIntervalsFieldTO?.value?.Count == 0;

                if (data.pricingIntervalsFieldTO.value == null || data.pricingIntervalsFieldTO.value.Count == 0)
                    data.pricingIntervalsFieldTO.value = new List<PricingIntervalsFieldTOValue>();

                if (data.pricingIntervalsFieldTO.value.Count == 0)
                    data.pricingIntervalsFieldTO.value.Add(new PricingIntervalsFieldTOValue());
                data.pricingIntervalsFieldTO.value.First().tierStem = priceTier;

                int effectiveDate = firstPrice ? 0 : Utilities.EpochTime * 1000;
                data.pricingIntervalsFieldTO.value.First().priceTierEffectiveDate = effectiveDate;
                data.pricingIntervalsFieldTO.value.First().priceTierEndDate = null;
                data.countriesChanged = firstPrice;
                data.theWorld = true;

                // first price, need to set all countries
                if (firstPrice)
                {
                    List<CountryPricing> _supportedCountries = new List<CountryPricing>();
                    SupportedCountries().ForEach(country =>
                    {
                        country.region = "";
                        _supportedCountries.Add(country);
                    });

                    data.countries = _supportedCountries;
                }

                // send the changes back to Apple
                Task<(string, int)> updateTask = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/ra/apps/{appId}/pricing/intervals", data));
                (string updateResponseResult, int updateResponseCode) = updateTask.Result;

                HandleItcResponse(updateResponseResult);

                PriceTierAvailabilityResponseObject updateResponseObject = JObject.Parse(updateResponseResult).ToObject<PriceTierAvailabilityResponseObject>();
                return updateResponseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public void TransformToRawPricingIntervals(string appId = null, string purchaseId = null, int pricingIntervals = 5, string subscriptionPriceTarget = null)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                // TODO: Only used in In-App Purchase (IAP)
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public string PriceTier(string appId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/{appId}/pricing/intervals"));
                (string responseResult, int responseCode) = task.Result;

                PriceTierAvailabilityResponseObject responseObject = JObject.Parse(responseResult).ToObject<PriceTierAvailabilityResponseObject>();
                var data = responseObject.data;

                if (string.IsNullOrEmpty(data.pricingIntervalsFieldTO.value.First().tierStem))
                    return data.pricingIntervalsFieldTO.value.First().tierStem;
                else
                    return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // Returns an array of all available pricing tiers
        //
        // @note Although this information is publicly available, the current spaceship implementation requires you to have a logged in client to access it
        //
        // @return [Array] the PricingTier objects (PricingTier)
        // [{
        //   "tierStem": "0",
        //   "tierName": "Free",
        //   "pricingInfo": [{
        //       "country": "United States",
        //       "countryCode": "US",
        //       "currencySymbol": "$",
        //       "currencyCode": "USD",
        //       "wholesalePrice": 0.0,
        //       "retailPrice": 0.0,
        //       "fRetailPrice": "$0.00",
        //       "fWholesalePrice": "$0.00"
        //     }, {
        //     ...
        // }, {
        // ...
        private List<PricingTier> _pricingTiers = new List<PricingTier>();
        public List<PricingTier> PricingTiers()
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (_pricingTiers != null && _pricingTiers.Count > 0)
                    return _pricingTiers;

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/pricing/matrix"));
                (string responseResult, int responseCode) = task.Result;

                PricingMatrixResponseObject responseObject = JObject.Parse(responseResult).ToObject<PricingMatrixResponseObject>();
                var pricingTiers = responseObject.data.pricingTiers;

                pricingTiers.ForEach(pricingTier =>
                {
                    _pricingTiers.Add(pricingTier);
                });

                return _pricingTiers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        //////////////////////////
        /// @!group Availability
        //////////////////////////

        #region Availability
        public Availability Availability(string appId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/{appId}/pricing/intervals"));
                (string responseResult, int responseCode) = task.Result;

                GetPricingIntervalsResponseObject responseObject = JObject.Parse(responseResult).ToObject<GetPricingIntervalsResponseObject>();
                return JObject.FromObject(responseObject.data).ToObject<Availability>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public Availability UpdateAvailability(string appId, Availability availability)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/{appId}/pricing/intervals"));
                (string responseResult, int responseCode) = task.Result;

                GetPricingIntervalsResponseObject responseObject = JObject.Parse(responseResult).ToObject<GetPricingIntervalsResponseObject>();
                PricingIntervalsData data = responseObject.data;

                List<B2bUser> b2bUsers = new List<B2bUser>();
                availability.convertedB2bUsers.ForEach(user =>
                {
                    b2bUsers.Add(new B2bUser
                    {
                        value = new B2bUserValue
                        {
                            add = user.add,
                            delete = user.delete,
                            dsUsername = user.dsUsername
                        }
                    });
                });

                List<B2bOrganization> b2BOrganizations = new List<B2bOrganization>();
                availability.convertedB2bOrganizationValues.ForEach(org =>
                {
                    b2BOrganizations.Add(new B2bOrganization
                    {
                        value = new B2bOrganizationValue
                        {
                            type = org.type,
                            depCustomerId = org.depCustomerId,
                            organizationId = org.organizationId,
                            name = org.name
                        }
                    });
                });

                data.countriesChanged = true;
                var supportedCountries = SupportedCountries();
                data.countries = availability.territories.Select(territory =>
                {
                    return supportedCountries.Find(supportedCountry => supportedCountry.code == territory.code);
                }).ToList();

                if (availability?.includeFutureTerritories == null)
                    data.theWorld = true;
                else
                    data.theWorld = availability.includeFutureTerritories;

                // InitializespreOrder (if needed)
                if (data.preOrder == null)
                    data.preOrder = new PreOrder();

                // Sets appAvailableDate to null if clearedForPreorder if false
                // This is need for apps that have never set either of these before
                // API will error out if cleared_for_preorder is false and app_available_date has a date
                bool clearedForPreOrder = availability.clearedForPreOrder;
                string appAvailableDate = clearedForPreOrder ? availability.appAvailableDate : null;
                data.b2bAppEnabled = availability.b2bAppEnabled;
                data.educationalDiscount = availability.educationalDiscount;
                data.preOrder.clearedForPreOrder = new ClearedForPreOrder { value = clearedForPreOrder, isEditable = true, isRequired = true, errorKeys = null };
                data.preOrder.appAvailableDate = new AppAvailableDate { value = appAvailableDate, isEditable = true, isRequired = true, errorKeys = null };
                data.b2bUsers = availability.b2bAppEnabled ? b2bUsers : new List<B2bUser>();
                data.b2bOrganizations = availability.b2bAppEnabled ? b2BOrganizations : new List<B2bOrganization>();

                // send the changes back to Apple
                Task<(string, int)> updateTask = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/ra/apps/{appId}/pricing/intervals", data));
                (string updateResponseResult, int updateResponseCode) = updateTask.Result;

                HandleItcResponse(updateResponseResult);

                GetPricingIntervalsResponseObject updateResponseObject = JObject.Parse(updateResponseResult).ToObject<GetPricingIntervalsResponseObject>();
                return JObject.FromObject(updateResponseObject.data).ToObject<Availability>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public List<CountryPricing> SupportedCountries()
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/pricing/supportedCountries"));
                (string responseResult, int responseCode) = task.Result;

                GetSupportedCountriesResponseObject responseObject = JObject.Parse(responseResult).ToObject<GetSupportedCountriesResponseObject>();
                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public List<Territory> SupportedTerritories()
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                var territories = SupportedCountries().Select(country => JObject.FromObject(country).ToObject<Territory>()).ToList();
                return territories;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public List<string> AvailableLanguages()
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/ref"));
                (string responseResult, int responseCode) = task.Result;

                RefResponseObject responseObject = JObject.Parse(responseResult).ToObject<RefResponseObject>();
                return responseObject.data.detailLocales;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        //////////////////////////////
        /// @!group App Icons
        //////////////////////////////

        #region App Icons
        // Uploads a large icon
        // @param appVersion (AppVersion): The version of your app
        // @param uploadImage (UploadFile): The icon to upload
        // @return [JSON] the response
        public UploadFileResponseObject UploadLargeIcon(AppVersion appVersion, UploadFile uploadImage)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (appVersion == null)
                    throw new TunesException("App Version is Required");

                if (uploadImage == null)
                    throw new TunesException("Upload Image is Required");

                UploadFileResponseObject uploadResponse = DUClient.UploadLargeIcon(appVersion, uploadImage, contentProviderId, ssoTokenForImage);

                return uploadResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // NOTE: Pushed back to a later iteration
        // TODO: Continue working this method
        // Uploads a watch icon
        // @param appVersion (AppVersion): The version of your app
        // @param uploadImage (UploadFile): The icon to upload
        // @return [JSON] the response
        public UploadFileResponseObject UploadWatchIcon(AppVersion appVersion, UploadFile uploadImage)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (appVersion == null)
                    throw new TunesException("App Version is Required");

                if (uploadImage == null)
                    throw new TunesException("Upload Image is Required");

                UploadFileResponseObject uploadResponse = DUClient.UploadWatchIcon(appVersion, uploadImage, contentProviderId, ssoTokenForImage);

                return uploadResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // NOTE: Pushed back to a later iteration
        // Uploads an In-App-Purchase Promotional image
        // @param appId (string): The id of the app
        // @param uploadImage (UploadFile): The icon to upload
        // @return [JSON] the image data, ready to be added to an In-App-Purchase
        public PurchaseMechScreenshot UploadPurchaseMerchScreenshot(string appId, UploadFile uploadImage)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (appId == null)
                    throw new TunesException("App Id is Required");

                if (uploadImage == null)
                    throw new TunesException("Upload Image is Required");

                UploadFileResponseObject uploadResponse = DUClient.UploadPurchaseMerchScreenshot(appId, uploadImage, contentProviderId, ssoTokenForImage);

                var resultImage = new PurchaseMerchScreenshotImage
                {
                    id = null,
                    image = new PurchaseMerchImage
                    {
                        value = new PurchaseMerchImageValue
                        {
                            assetToken = uploadResponse.token,
                            originalFileName = uploadImage.fileName,
                            height = uploadResponse.height,
                            width = uploadResponse.width,
                            checksum = uploadResponse.md5
                        },
                        isEditable = true,
                        isRequired = false,
                        errorKeys = null
                    }
                };

                return new PurchaseMechScreenshot
                {
                    images = new List<PurchaseMerchScreenshotImage> { resultImage },
                    showByDefault = true,
                    isActive = false
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // NOTE: Pushed back to a later iteration
        // Uploads an In-App-Purchase Review screenshot
        // @param appId (string): The id of the app
        // @param uploadImage (UploadFile): The icon to upload
        // @return [JSON] the screenshot data, ready to be added to an In-App-Purchase
        public PurchaseReviewScreenshot UploadPurchaseReviewScreenshot(string appId, UploadFile uploadImage)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (appId == null)
                    throw new TunesException("App Id is Required");

                if (uploadImage == null)
                    throw new TunesException("Upload Image is Required");

                UploadFileResponseObject uploadResponse = DUClient.UploadPurchaseReviewScreenshot(appId, uploadImage, contentProviderId, ssoTokenForImage);

                var value = new PurchaseReviewScreenshotValue
                {
                    assetToken = uploadResponse.token,
                    sortOrder = 0,
                    type = DUClient.GetPictureType(uploadImage),
                    originalFileName = uploadImage.fileName,
                    size = uploadResponse.length,
                    height = uploadResponse.height,
                    width = uploadResponse.width,
                    checksum = uploadResponse.md5
                };

                return new PurchaseReviewScreenshot { value = value };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // Uploads a screenshot
        // @param appVersion (AppVersion): The version of your app
        // @param uploadImage (UploadFile): The image to upload
        // @param device (string): The target device
        // @param isMessages (Bool): True if the screenshot is for iMessage
        // @return [JSON] the response
        public UploadFileResponseObject UploadScreenshot(AppVersion appVersion, UploadFile uploadImage, string device, bool isMessages = false)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (appVersion == null)
                    throw new TunesException("App Version is Required");

                if (uploadImage == null)
                    throw new TunesException("Upload Image is Required");

                if (device == null)
                    throw new TunesException("Device is Required");

                UploadFileResponseObject uploadResponse = DUClient.UploadScreenshot(appVersion, uploadImage, contentProviderId, ssoTokenForImage, device, isMessages);

                return uploadResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // Uploads the transit app file
        // @param app_version (AppVersion): The version of your app
        // @param upload_file (UploadFile): The image to upload
        // @return [JSON] the response
        public UploadFileResponseObject UploadGeojson(AppVersion appVersion, UploadFile uploadFile)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (appVersion == null)
                    throw new TunesException("App Version is Required");

                if (uploadFile == null)
                    throw new TunesException("Upload File is Required");

                UploadFileResponseObject uploadResponse = DUClient.UploadGeojson(appVersion, uploadFile, contentProviderId, ssoTokenForVideo);

                return uploadResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // Uploads the transit app file
        // @param appVersion (AppVersion): The version of your app
        // @param uploadTrailer (UploadFile): The trailer to upload
        // @return [JSON] the response
        public UploadFileResponseObject UploadTrailer(AppVersion appVersion, UploadFile uploadTrailer)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (appVersion == null)
                    throw new TunesException("App Version is Required");

                if (uploadTrailer == null)
                    throw new TunesException("Upload Trailer is Required");

                UploadFileResponseObject uploadResponse = DUClient.UploadTrailer(appVersion, uploadTrailer, contentProviderId, ssoTokenForVideo);

                return uploadResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // Uploads the trailer preview
        // @param appVersion (AppVersion): The version of your app
        // @param uploadTrailerPreview (UploadFile): The trailer preview to upload
        // @param device (string): The target device
        // @return [JSON] the response
        public UploadFileResponseObject UploadTrailerPreview(AppVersion appVersion, UploadFile uploadTrailerPreview, string device)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (appVersion == null)
                    throw new TunesException("App Version is Required");

                if (uploadTrailerPreview == null)
                    throw new TunesException("Upload Trailer Preview is Required");

                UploadFileResponseObject uploadResponse = DUClient.UploadTrailerPreview(appVersion, uploadTrailerPreview, contentProviderId, ssoTokenForImage, device);

                return uploadResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        //////////////////////////////
        /// @!group Review Attachment File
        //////////////////////////////

        #region Review Attachment File
        // NOTE: Method not yet tested
        // Uploads a attachment file
        // @param appVersion (AppVersion): The version of your app(must be edit version)
        // @param uploadAttachmentFile (file): File to upload
        // @return [JSON] the response
        public UploadFileResponseObject UploadAppReviewAttachment(AppVersion appVersion, UploadFile uploadAttachmentFile)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (appVersion == null)
                    throw new TunesException("App Version is Required");

                if (uploadAttachmentFile == null)
                    throw new TunesException("Upload Attachment File is Required");

                UploadFileResponseObject uploadResponse = DUClient.UploadAppReviewAttachment(appVersion, uploadAttachmentFile, contentProviderId, ssoTokenForImage);

                return uploadResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // Fetches the App Version Reference information from ITC
        // @return [AppVersionRef] the response
        public AppVersionRef RefData()
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/version/ref"));
                (string responseResult, int responseCode) = task.Result;

                AppVersionRefResponseObject responseObject = JObject.Parse(responseResult).ToObject<AppVersionRefResponseObject>();
                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        //////////////////////////////
        /// @!group Candidate Builds
        //////////////////////////////

        // NOTE: Method not yet tested
        #region Candidate Builds
        public List<Build> CandidateBuilds(string appId, string versionId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new TunesException("App ID is Required");

                if (string.IsNullOrEmpty(versionId))
                    throw new TunesException("Version ID is Required");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/{appId}/version/{versionId}/candidateBuilds"));
                (string responseResult, int responseCode) = task.Result;

                CandidateBuildsResponseObject responseObject = JObject.Parse(responseResult).ToObject<CandidateBuildsResponseObject>();
                return responseObject.data.builds;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        //////////////////////////////
        /// @!group Build Trains
        //////////////////////////////

        #region Build Trains
        // NOTE: Method not yet tested
        // @param (testingType) internal or external
        public void BuildTrains(string appId, string testingType, int tries = 5, string platform = "ios")
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new TunesException("App ID is Required");

                string url = $"{Hostname}/ra/apps/{appId}/trains/?testingType={testingType}";

                if (!string.IsNullOrEmpty(platform))
                    url += $"&platform={platform}";

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", url));
                (string responseResult, int responseCode) = task.Result;

                if (responseCode == 404)
                {
                    Regex re = new Regex(@"<String>(\w|\s|\W)*</String>");
                    string matched = re.Match(responseResult).Groups[1].ToString();
                    throw new Exception(matched);
                }

                // TODO: Continue this method
                // return parse_response(r, 'data')
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Build trains fail randomly very often
                // we need to catch those errors and retry
                // https://github.com/fastlane/fastlane/issues/6419
                List<string> retryErrorMessages = new List<string> {
                  "ITC.response.error.OPERATION_FAILED",
                  "Internal Server Error",
                  "Service Unavailable"
                };

                if (retryErrorMessages.Any(message => ex.ToString().Contains(message)))
                {
                    tries -= 1;
                    if (tries > 0)
                    {
                        Console.WriteLine("Received temporary server error from App Store Connect. Retrying the request...");
                        BuildTrains(appId, testingType, tries, platform);
                    }
                }

                throw new Exception($"Temporary App Store Connect error: {ex.Message}");
            }
        }

        // NOTE: Method not yet tested
        public void UpdateBuildTrains(string appId, string testingType, object data)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new TunesException("App ID is Required");

                // The request fails if this key is present in the data
                //data.delete("dailySubmissionCountByPlatform")

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/ra/apps/{appId}/testingTypes/{testingType}/trains", data));
                (string responseResult, int responseCode) = task.Result;

                HandleItcResponse(responseResult);

                // TODO: return object
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // NOTE: Method not yet tested
        public void RemoveTestflightBuildFromReview(string appId = null, string train = null, string buildNumber = null, string platform = "ios")
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new TunesException("App ID is Required");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/ra/apps/{appId}/platforms/{platform}/trains/{train}/builds/{buildNumber}/reject", new object()));
                (string responseResult, int responseCode) = task.Result;

                HandleItcResponse(responseResult);

                // TODO: return object
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // NOTE: Method not yet tested
        // All build trains, even if there is no TestFlight
        public void AllBuildTrains(string appId = null, string platform = "ios")
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new TunesException("App ID is Required");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/{appId}/buildHistory?platform={platform}"));
                (string responseResult, int responseCode) = task.Result;

                HandleItcResponse(responseResult);

                // TODO: return object
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // NOTE: Method not yet tested
        public void AllBuildsForTrain(string appId = null, string train = null, string platform = "ios")
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new TunesException("App ID is Required");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/{appId}/trains/{train}/buildHistory?platform={platform}"));
                (string responseResult, int responseCode) = task.Result;

                HandleItcResponse(responseResult);

                // TODO: return object
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // NOTE: Method not yet tested
        public BuildDetails BuildDetails(string appId = null, string train = null, string buildNumber = null, string platform = "ios")
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new TunesException("App ID is Required");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/{appId}/platforms/{platform}/trains/{train}/builds/{buildNumber}/details"));
                (string responseResult, int responseCode) = task.Result;

                HandleItcResponse(responseResult);

                BuildDetailsResponseObject responseObject = JObject.Parse(responseResult).ToObject<BuildDetailsResponseObject>();

                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // NOTE: Method not yet tested
        public void UpdateBuildInformation(string appId = null,
                                           string train = null,
                                           string buildNumber = null,
                                           string whatsNew = null,
                                           string description = null,
                                           string feedbackEmail = null,
                                           string platform = "ios")
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new TunesException("App ID is Required");

                string url = $"{Hostname}/ra/apps/{appId}/platforms/{platform}/trains/{train}/builds/{buildNumber}/testInformation";

                // var buildInfo = GetBuildInfoForReview(appId: appId, train: train, buildNumber: buildNumber, platform: platform);
                // foreach(var buildInfoDetail in buildInfo.details)
                // {
                //     if (string.IsNullOrEmpty(whatsNew))
                //         buildInfoDetail.whatsNew.value = whatsNew;
                //
                //     if (string.IsNullOrEmpty(description))
                //         buildInfoDetail.description.value = description;
                //
                //     if (string.IsNullOrEmpty(feedbackEmail))
                //         buildInfoDetail.feedbackEmail.value = feedbackEmail;
                // }

                // string reviewUserName = buildInfo.reviewUserName.value;
                // string reviewPassword = buildInfo.reviewPassword.value;
                // buildInfo.reviewAccountRequired.value = (reviewUserName + reviewPassword).Length > 0

                // Now send everything back to iTC
                //Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("POST", url, buildInfo));
                //(string responseResult, int responseCode) = task.Result;

                //HandleItcResponse(responseResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // NOTE: Method not yet tested
        public void GetBuildInfoForReview(string appId = null, string train = null, string buildNumber = null, string platform = "ios")
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (appId == null)
                    throw new TunesException("App ID is Required");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/{appId}/platforms/{platform}/trains/{train}/builds/{buildNumber}/testInformation"));
                (string responseResult, int responseCode) = task.Result;

                HandleItcResponse(responseResult);

                // return response data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
        #endregion

        // Fetches the User Detail information from ITC. This gets called often and almost never changes
        // so we cache it
        // @return [UserDetail] the response
        private UserDetail _cachedUserDetailData { get; set; }
        public UserDetail UserDetailData
        {
            get
            {
                if (_cachedUserDetailData != null)
                    return _cachedUserDetailData;

                _cachedUserDetailData = JObject.FromObject(UserDetailsData).ToObject<UserDetail>();
                return _cachedUserDetailData;
            }

            set { _cachedUserDetailData = value; }
        }

        // the contentProviderId found in the UserDetail instance
        private string _contentProviderId { get; set; }
        public string contentProviderId
        {
            get
            {
                if (_contentProviderId != null)
                    return _contentProviderId;

                _contentProviderId = UserDetailData.contentProviderId;
                return _contentProviderId;
            }

            set { _contentProviderId = value; }
        }

        // the ssoTokenForImage found in the AppVersionRef instance
        private string _ssoTokenForImage { get; set; }
        public string ssoTokenForImage
        {
            get
            {
                if (_ssoTokenForImage != null)
                    return _ssoTokenForImage;

                _ssoTokenForImage = RefData().ssoTokenForImage;
                return _ssoTokenForImage;
            }

            set { _ssoTokenForImage = value; }
        }

        // the ssoTokenForVideo found in the AppVersionRef instance
        private string _ssoTokenForVideo { get; set; }
        public string ssoTokenForVideo
        {
            get
            {
                if (_ssoTokenForVideo != null)
                    return _ssoTokenForVideo;

                _ssoTokenForVideo = RefData().ssoTokenForVideo;
                return _ssoTokenForVideo;
            }

            set { _ssoTokenForVideo = value; }
        }

        //////////////////////////
        /// @!group Submit for Review
        //////////////////////////

        #region Submit for Review
        public VersionInfoData PrepareAppSubmission(string appId, string version)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId is required");

                if (string.IsNullOrEmpty(version))
                    throw new Exception("version is required");

                (string responseResult, int responseCode) = _prepareAppSubmission($"{Hostname}/ra/apps/{appId}/versions/{version}/submit/summary");

                AppSubmissionResponseObject responseObject = JObject.Parse(responseResult).ToObject<AppSubmissionResponseObject>();

                HandleItcResponse(responseResult);

                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public VersionInfoData PrepareAppSubmission(string appId, int? version)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId is required");

                if (version == null || version <= 0)
                    throw new Exception("version is required");

                (string responseResult, int responseCode) = _prepareAppSubmission($"{Hostname}/ra/apps/{appId}/versions/{version}/submit/summary");

                AppSubmissionResponseObject responseObject = JObject.Parse(responseResult).ToObject<AppSubmissionResponseObject>();

                HandleItcResponse(responseResult);

                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private (string responseResult, int responseCode) _prepareAppSubmission(string url)
        {
            Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", url, "json/application"));
            (string responseResult, int responseCode) = task.Result;

            return (responseResult, responseCode);
        }

        public VersionInfoData SendAppSubmission(string appId, string version, object data)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId is required");

                if (string.IsNullOrEmpty(version))
                    throw new Exception("version is required");

                // ra/apps/1039164429/version/submit/complete
                (string responseResult, int responseCode) = _sendAppSubmission($"{Hostname}/ra/apps/{appId}/versions/{version}/submit/complete", data);

                AppSubmissionResponseObject responseObject = JObject.Parse(responseResult).ToObject<AppSubmissionResponseObject>();

                HandleItcResponse(responseResult);

                // App Store Connect still returns a success status code even the submission
                // was failed because of Ad ID Info / Export Compliance. This checks for any section error
                // keys in returned adIdInfo / exportCompliance and prints them out.
                var adIdErrorKeys = responseObject.data.adIdInfo.sectionErrorKeys;
                var exportErrorKeys = responseObject.data.exportCompliance.sectionErrorKeys;
                if (adIdErrorKeys.Count > 0)
                    throw new Exception($"Something wrong with your Ad ID information: {string.Join(", ", adIdErrorKeys)}.");
                else if (exportErrorKeys.Count > 0)
                    throw new Exception($"Something wrong with your Export Compliance: {string.Join(", ", exportErrorKeys)}.");
                else if (responseObject.messages.info.TakeLast(1).ToString() == "Successful POST")
                    _ = responseObject; // success
                else
                    throw new Exception("Something went wrong when submitting the app for review. Make sure to pass valid options to submit your app for review");

                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public VersionInfoData SendAppSubmission(string appId, int? version, object data)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId is required");

                if (version == null || version <= 0)
                    throw new Exception("version is required");

                // ra/apps/1039164429/version/submit/complete
                (string responseResult, int responseCode) = _sendAppSubmission($"{Hostname}/ra/apps/{appId}/versions/{version}/submit/complete", data);

                AppSubmissionResponseObject responseObject = JObject.Parse(responseResult).ToObject<AppSubmissionResponseObject>();

                HandleItcResponse(responseResult);

                // App Store Connect still returns a success status code even the submission
                // was failed because of Ad ID Info / Export Compliance. This checks for any section error
                // keys in returned adIdInfo / exportCompliance and prints them out.
                var adIdErrorKeys = responseObject.data.adIdInfo.sectionErrorKeys;
                var exportErrorKeys = responseObject.data.exportCompliance.sectionErrorKeys;
                if (adIdErrorKeys.Count > 0)
                    throw new Exception($"Something wrong with your Ad ID information: {string.Join(", ", adIdErrorKeys)}.");
                else if (exportErrorKeys.Count > 0)
                    throw new Exception($"Something wrong with your Export Compliance: {string.Join(", ", exportErrorKeys)}.");
                else if (responseObject.messages.info.TakeLast(1).ToString() == "Successful POST")
                    _ = responseObject; // success
                else
                    throw new Exception("Something went wrong when submitting the app for review. Make sure to pass valid options to submit your app for review");

                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private (string responseResult, int responseCode) _sendAppSubmission(string url, object data)
        {
            Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("POST", url, "json/application"));
            (string responseResult, int responseCode) = task.Result;

            return (responseResult, responseCode);
        }
        #endregion

        //////////////////////////
        /// @!group State History
        //////////////////////////

        public List<AppVersionHistory> VersionsHistory(string appId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId is required");

                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", $"{Hostname}/ra/apps/{appId}/stateHistory?platform=ios"));
                (string responseResult, int responseCode) = task.Result;

                VersionsHistoryResponseObject responseObject = JObject.Parse(responseResult).ToObject<VersionsHistoryResponseObject>();

                return responseObject.data.versions;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public AppVersionHistory VersionStatesHistory(string appId, string versionId)
        {

            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId is required");

                AppVersionHistory data = _versionStatesHistory($"{Hostname}/ra/apps/{appId}/versions/{versionId}/stateHistory?platform=ios");

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public AppVersionHistory VersionStatesHistory(string appId, int versionId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId is required");

                AppVersionHistory data = _versionStatesHistory($"{Hostname}/ra/apps/{appId}/versions/{versionId}/stateHistory?platform=ios");

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private AppVersionHistory _versionStatesHistory(string url)
        {
            try
            {
                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("GET", url));
                (string responseResult, int responseCode) = task.Result;

                VersionStatesHistoryResponseObject responseObject = JObject.Parse(responseResult).ToObject<VersionStatesHistoryResponseObject>();

                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //////////////////////////
        /// @!group reject
        //////////////////////////

        // NOTE: Method not yet tested
        public RejectVersion Reject(string appId, string versionId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId is required");

                if (string.IsNullOrEmpty(versionId))
                    throw new Exception("versionId is required");

                RejectVersion data = _reject($"{Hostname}/ra/apps/{appId}/versions/{versionId}/reject", appId);

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // NOTE: Method not yet tested
        public RejectVersion Reject(string appId, int? versionId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId is required");

                if (versionId == null || versionId <= 0)
                    throw new Exception("versionId is required");

                RejectVersion data = _reject($"{Hostname}/ra/apps/{appId}/versions/{versionId}/reject", appId);

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private RejectVersion _reject(string url, object data)
        {
            try
            {
                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("POST-string", url, data));
                (string responseResult, int responseCode) = task.Result;

                RejectVersionResponseObject responseObject = JObject.Parse(responseResult).ToObject<RejectVersionResponseObject>();

                return responseObject.data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //////////////////////////
        /// @!group release
        //////////////////////////

        // NOTE: Method not yet tested
        public void Release(string appId, string version)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId is required");

                if (string.IsNullOrEmpty(version))
                    throw new Exception("version is required");

                _release($"{Hostname}/ra/apps/{appId}/versions/{version}/releaseToStore", appId);

                // HandleItcResponse();
                // return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // NOTE: Method not yet tested
        public void Release(string appId, int? version)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId is required");

                if (version == null || version <= 0)
                    throw new Exception("version is required");

                _release($"{Hostname}/ra/apps/{appId}/versions/{version}/releaseToStore", appId);

                // HandleItcResponse();
                // return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private void _release(string url, object data)
        {
            try
            {
                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("POST-string", url, data));
                (string responseResult, int responseCode) = task.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        //////////////////////////
        /// @!group release to all users
        //////////////////////////

        // NOTE: Method not yet tested
        public void ReleaseToAllUsers(string appId, string version)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId is required");

                if (string.IsNullOrEmpty(version))
                    throw new Exception("version is required");

                _release($"{Hostname}/ra/apps/{appId}/versions/{version}/phasedRelease/state/COMPLETE", appId);

                // HandleItcResponse();
                // return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // NOTE: Method not yet tested
        public void ReleaseToAllUsers(string appId, int? version)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appId))
                    throw new Exception("appId is required");

                if (version == null || version <= 0)
                    throw new Exception("version is required");

                _release($"{Hostname}/ra/apps/{appId}/versions/{version}/phasedRelease/state/COMPLETE", appId);

                // HandleItcResponse();
                // return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        private void _releaseToAllUsers(string url, object data)
        {
            try
            {
                Task<(string, int)> task = Task.Run(async () => await account.SendRequestAsync("POST-string", url, data));
                (string responseResult, int responseCode) = task.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }
    }
}
