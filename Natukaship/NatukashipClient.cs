using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Natukaship
{
    // Client
    // spaceship/lib/spaceship/client.rb
    public class NatukashipClient
    {
        public string ProtocolVersion { get => "QH65B2"; }

        public List<Team> teams;
        public List<ProvisioningProfile> provisioningProfiles;
        public List<AppId> appIDs;
        public List<Device> devices;
        public List<CertRequest> certRequests;
        public ProvisioningProfile provisioningProfile;
        public CookieContainer _cookieJar = new CookieContainer();
        public HttpClientHandler clientHandler;
        public HttpClient httpClient;
        public FlurlClient flurlClient;
        public CookieManager cookieManager;
        public static OlympusSession olympusSession;
        public NatukashipSetting GlobalSetting;

        public string User { get; set; }
        public string Password { get; set; }
        public bool IsLoggedIn { get; set; }
        public string teamId { get; set; }

        public static string _service_key;
        public string _prefix;

        public NatukashipClient(NatukashipSetting setting)
        {
            clientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                AllowAutoRedirect = false,
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true, // bypassing SSL
                CookieContainer = _cookieJar,
                UseCookies = true,
                UseDefaultCredentials = false
            };

            if (setting.UseProxy)
                clientHandler.Proxy = new WebProxy(setting.ProxyAddress);

            if (setting.Username?.Length > 0)
                User = setting.Username;

            if (setting.Password?.Length > 0)
                Password = setting.Password;

            httpClient = new HttpClient(clientHandler);

            flurlClient = new FlurlClient(httpClient)
                .EnableCookies()
                .WithHeader("User-Agent", setting.UserAgent)
                .AllowAnyHttpStatus();

            GlobalSetting = setting;
        }

        /// <summary>
        /// Return the ServiceKey
        /// </summary>
        public static string ServiceKey
        {
            get
            {
                if (!string.IsNullOrEmpty(_service_key))
                    return _service_key;

                // Check if we have a local cache of the key
                string path = Directory.GetCurrentDirectory();
                var itc_service_key_path = $"{path}/natukaship/natukaship_itc_service_key.txt";
                if (File.Exists(itc_service_key_path))
                {
                    ServiceKey = File.ReadAllText(itc_service_key_path);
                    return _service_key;
                }

                // Even though we are using https://appstoreconnect.apple.com, the service key needs to still use a
                // hostname through itunesconnect.apple.com
                var (responseResult, responseCode) = Task.Run(async () =>
                {
                    string uri = "https://appstoreconnect.apple.com/olympus/v1/app/config?hostname=itunesconnect.apple.com";
                    HttpResponseMessage response = await uri.GetAsync();
                    string result = response.Content.ReadAsStringAsync().Result;
                    int code = response.StatusCode.GetHashCode();

                    Logger.Info($">> GET {uri}: [undefined body]");
                    Logger.Debug($"<< GET {uri}: {code} {result}");

                    return (result, code);
                }).Result;

                var service_key_object = JObject.Parse(responseResult).ToObject<ServiceKeyObject>();

                if (service_key_object.authServiceKey.Length == 0)
                    throw new NullReferenceException();

                File.WriteAllText(itc_service_key_path, service_key_object.authServiceKey);
                ServiceKey = service_key_object.authServiceUrl;

                return _service_key;
            }

            set { _service_key = value; }
        }

        public async Task FetchOlympusSession()
        {
            try
            {
                // Get the `itctx` from the new (22nd May 2017) API endpoint "olympus"
                // Update (29th March 2019) olympus migrates to new appstoreconnect API
                var (responseResult, responseCode) = await SendRequestAsync("GET", "https://appstoreconnect.apple.com/olympus/v1/session");

                try
                {
                    responseResult = JObject.Parse(responseResult).ToString(Formatting.None);
                    olympusSession = JObject.Parse(responseResult).ToObject<OlympusSession>();
                }
                catch { }

                if (!responseResult.Contains("Unauthenticated"))
                    cookieManager.WriteCookiesToDisk(_cookieJar);
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Send request to the url
        /// </summary>
        /// <param name="method">method for request</param>
        /// <param name="url">url for request</param>
        /// <param name="data">data to be passed to Apple API</param>
        /// <param name="contentType">type of data</param>
        /// <returns>request response and status code</returns>
        public async Task<(string, int)> SendRequestAsync(string method, string url, object data = null, string contentType = "application/json")
        {
            var requestClient = url.WithClient(flurlClient);

            if (method == "POST")
            {
                HttpResponseMessage postResponse;
                string requestParams = "";

                if (data == null)
                {
                    postResponse = await requestClient.PostAsync(null);
                }
                else
                {
                    requestParams = JObject.FromObject(data).ToString(Formatting.None);

                    if (contentType == "application/x-www-form-urlencoded")
                        postResponse = await requestClient.PostUrlEncodedAsync(data);
                    else
                        postResponse = await requestClient.PostJsonAsync(data);
                }

                int statusCode = postResponse.StatusCode.GetHashCode();
                string responseResult = postResponse.Content.ReadAsStringAsync().Result;
                try { responseResult = JObject.Parse(responseResult).ToString(Formatting.None); } catch { }

                Logger.Info($">> {method} {url}: [undefined body]");
                Logger.Debug($"<< {method} {url}: {statusCode} {responseResult} - {requestParams}");

                cookieManager.WriteCookiesToDisk(_cookieJar);

                return (responseResult, statusCode);
            }
            else
            {
                HttpResponseMessage getResponse = await requestClient.GetAsync();

                int statusCode = getResponse.StatusCode.GetHashCode();
                string responseResult = getResponse.Content.ReadAsStringAsync().Result;
                try { responseResult = JObject.Parse(responseResult).ToString(Formatting.None); } catch { }

                Logger.Info($">> {method} {url}: [undefined body]");
                Logger.Debug($"<< {method} {url}: {statusCode} {responseResult}");

                cookieManager.WriteCookiesToDisk(_cookieJar);

                return (responseResult, statusCode);
            }
        }

        // Sometimes we get errors or info nested in our data
        // This method allows you to pass in a set of keys to check for
        // along with the name of the sub_section of your original data
        // where we should check
        // Returns a mapping of keys to data array if we find anything, otherwise, empty map
        public JObject FetchErrorsInData(JObject dataSection = null, string subSectionName = null, string[] keys = null)
        {
            JObject subSection;

            if (dataSection != null && !string.IsNullOrEmpty(subSectionName))
                subSection = dataSection.Value<JObject>(subSectionName);
            else
                subSection = dataSection;

            if (subSection == null)
                return null;

            JObject errorMap = new JObject();
            foreach (var key in keys)
            {
                JArray errors = subSection.Value<JArray>(key) ?? new JArray();

                if (errors.Count > 0)
                    errorMap[key] = errors;
            }

            return errorMap;
        }

        // If the response is coming from a flaky api, set flaky_api_call to true so we retry a little.
        // Patience is a virtue.
        public string HandleItcResponse(string response, bool flakyApiCall = false)
        {
            if (string.IsNullOrEmpty(response))
                return null;

            var data = JObject.Parse(response);

            if (data["data"] != null)
                data = (JObject)data["data"];

            string[] errorKeys = new string[] { "sectionErrorKeys", "validationErrors", "serviceErrors" };
            string[] infoKeys = new string[] { "sectionInfoKeys", "sectionWarningKeys" };
            string[] errorAndInfoKeysToCheck = { "sectionErrorKeys", "validationErrors", "serviceErrors", "sectionInfoKeys", "sectionWarningKeys" };
            List<string> errors = new List<string>();

            JObject errorsInData = FetchErrorsInData(dataSection: data, keys: errorAndInfoKeysToCheck);
            JObject errorsInVersionInfo = FetchErrorsInData(dataSection: data, subSectionName: "versionInfo", keys: errorAndInfoKeysToCheck);

            // If we have any errors or "info" we need to treat them as warnings or errors
            if (errorsInData?.Count == 0 && errorsInVersionInfo?.Count == 0)
                Console.WriteLine("Request was successfull");

            // We pass on the `current_language` so that the error message tells the user
            // what language the error was caused in
            List<string> _handleResponseHash(JObject responseHash, string currentLanguage = null)
            {
                if (responseHash["language"] != null)
                    currentLanguage = responseHash.Value<string>("language");

                foreach (var rh in responseHash)
                {
                    var result = new List<string>();

                    if (rh.Value.GetType().Name == "JArray")
                        result = _handleResponseArray((JArray)rh.Value);
                    else
                        result = _handleResponseHash((JObject)rh.Value, "");

                    errors.AddRange(result);

                    if (rh.Key != "errorKeys" && rh.Value.GetType().Name != "JArray" && rh.Value.Count() < 1)
                        continue;

                    // Prepend the error with the language so it's easier to understand for the user
                    errors.ForEach(currentErrorMessage =>
                    {
                        if (currentLanguage != null)
                        {
                            currentErrorMessage = $"[{currentLanguage}]: {currentErrorMessage}";
                        }
                    });
                }

                return errors;
            }

            List<string> _handleResponseArray(JArray responseArray, string currentLanguage = null)
            {
                foreach (var response in responseArray.ToObject<List<JObject>>())
                {
                    errors.AddRange(_handleResponseHash(response));
                }

                return errors;
            }

            errors = _handleResponseHash(data);

            // Search at data level, as well as "versionInfo" level for errors
            errorsInData = FetchErrorsInData(dataSection: data, keys: errorKeys);
            errorsInVersionInfo = FetchErrorsInData(dataSection: data, subSectionName: "versionInfo", keys: errorKeys);

            if (errorsInData?.Values()?.Count() > 0)
                errors.AddRange(errorsInData.Values<string>());

            if (errorsInVersionInfo?.Values()?.Count() > 0)
                errors.AddRange(errorsInVersionInfo.Values<string>());

            // Sometimes there is a different kind of error in the JSON response
            // e.g. {"warn"=>nil, "error"=>["operation_failed"], "info"=>nil}
            var differentError = JObject.Parse(response)["messages"]["error"]?.Value<List<string>>();
            if (differentError != null)
                errors.AddRange(differentError);

            //  they are separated by `.` by default
            if (errors.Count > 0)
            {
                // Sample `error` content: [["Forbidden"]]
                if (errors.Count == 1 && errors[0] == "You haven't made any changes.")
                    _ = ""; // This is a special error which we really don't care about
                else if (errors.Count == 1 && errors[0].Contains("try again later"))
                    throw new Exception($"ITunesConnectTemporaryError: {errors[0]}");
                else if (errors.Count == 1 && errors[0].Contains("Forbidden"))
                    throw new Exception($"InsufficientPermissions: {errors[0]}");
                else if (flakyApiCall)
                    throw new Exception($"ITunesConnectPotentialServerError: {string.Join(" ", errors)}");
                else
                    throw new Exception($"ITunesConnectError: {string.Join(" ", errors)}");
            }

            // Search at data level, as well as "versionInfo" level for info and warnings
            JObject infoInData = FetchErrorsInData(dataSection: data, keys: infoKeys);
            JObject infoInVersionInfo = FetchErrorsInData(dataSection: data, subSectionName: "versionInfo", keys: infoKeys);

            if (infoInData != null)
            {
                foreach (var info in infoInData)
                {
                    Console.WriteLine(info.Value.ToString());
                }
            }

            if (infoInVersionInfo != null)
            {
                foreach (var info in infoInVersionInfo)
                {
                    Console.WriteLine(info.Value.ToString());
                }
            }

            return data.ToString(Formatting.None);
        }
    }
}
