using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl.Http;
using Natukaship.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Natukaship
{
    public class AccountManager
    {
        // Private Variables
        private readonly string DEFAULT_PREFIX = "deliver";
        private string _username;
        private string _password;
        private string _prefix;
        private string _note;
        private string _service_key = NatukashipClient.ServiceKey;
        private string _appleId;
        private string _scnt;
        private CookieContainer _cookieJar;
        private HttpClientHandler clientHandler;
        private HttpClient httpClient;
        private FlurlClient flurlClient;

        // Public Variables
        public Provider _provider;
        public CookieManager cookieManager;
        public OlympusSession olympusSession;

        // Properties
        public bool IsDefaultPrefix { get => _prefix == DEFAULT_PREFIX; }
        public bool IsLoggedIn { get; set; }
        public bool EnsureCSRFToken { get; set; }

        public string CsrfToken { get; set; }
        public string CsrfTokenTs { get; set; }

        /// <summary>
        /// Return the email of the User
        /// </summary>
        public string User
        {
            get
            {
                if (IsDefaultPrefix)
                {
                    if (_username == null)
                        _username = Environment.GetEnvironmentVariable("FASTLANE_USER");

                    if (_username == null)
                        _username = Environment.GetEnvironmentVariable("DELIVER_USER");
                }

                if (string.IsNullOrEmpty(_username))
                    AskForLogin();

                return _username;
            }

            set { _username = value; cookieManager = new CookieManager(value); }
        }

        /// <summary>
        /// Return the password of the User
        /// </summary>
        public string Password
        {
            get
            {
                if (IsDefaultPrefix)
                {
                    if (_password == null)
                        _password = Environment.GetEnvironmentVariable("FASTLANE_PASSWORD");

                    if (_password == null)
                        _password = Environment.GetEnvironmentVariable("DELIVER_PASSWORD");
                }

                if (string.IsNullOrEmpty(_password))
                    AskForLogin();

                return _password;
            }

            set { _password = value; }
        }

        // Functions
        public AccountManager(string username = null, string password = null, string prefix = null, string note = null)
        {
            _username = username;
            _password = password;
            _prefix = prefix ?? DEFAULT_PREFIX;
            _note = note;
            _cookieJar = new CookieContainer();

            clientHandler = new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                AllowAutoRedirect = false,
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true,
                CookieContainer = _cookieJar,
                UseCookies = true,
                UseDefaultCredentials = false
            };
            httpClient = new HttpClient(clientHandler);

            flurlClient = new FlurlClient(httpClient)
                .EnableCookies()
                .WithHeader("User-Agent", "Spaceship 2.135.2")
                .AllowAnyHttpStatus();

            if (!string.IsNullOrEmpty(_username))
                cookieManager = new CookieManager(_username);
        }

        /// <summary>
        /// Method for asking user the credentials needed
        /// </summary>
        public void AskForLogin()
        {
            if (!string.IsNullOrEmpty(_username) && !string.IsNullOrEmpty(_password))
            {
                cookieManager = new CookieManager(_username);
            }

            Console.WriteLine("-------------------------------------------------------------------------------------");
            Console.WriteLine("Please provide your Apple Developer Program account credentials");
            if (IsDefaultPrefix)
            {
                // We don't want to show this message, if we ask for the application specific password
                // which has a different prefix
                Console.WriteLine("You can also pass the password using the `FASTLANE_PASSWORD` environment variable");
            }
            Console.WriteLine("-------------------------------------------------------------------------------------");

            if (string.IsNullOrEmpty(_username))
            {
                Console.Write("Username: ");
                _username = Console.ReadLine();

                cookieManager = new CookieManager(_username);
            }

            if (string.IsNullOrEmpty(_password))
            {
                Console.Write($"Password (for {_username}): ");
                _password = FetchPasswordFromCLI();
            }
        }

        private async Task<bool> FetchOlympusSession()
        {
            try
            {
                // Get the `itctx` from the new (22nd May 2017) API endpoint "olympus"
                // Update (29th March 2019) olympus migrates to new appstoreconnect API
                (string responseResult, int responseCode) = await SendRequestAsync("GET", "https://appstoreconnect.apple.com/olympus/v1/session");

                try
                {
                    responseResult = JObject.Parse(responseResult).ToString(Formatting.None);
                    olympusSession = JObject.Parse(responseResult).ToObject<OlympusSession>();
                }
                catch { }

                if (responseResult.Contains("Unauthenticated"))
                    return false;

                cookieManager.WriteCookiesToDisk(_cookieJar);

                if (!string.IsNullOrEmpty(responseResult))
                {
                    var jsonResponse = JObject.Parse(responseResult);
                    _provider = jsonResponse["provider"].ToObject<Provider>();
                    return true;
                }

                return false;
            }
            catch (AggregateException)
            {
                return false;
            }
        }

        public async Task SendLoginRequestAsync()
        {
            if (LoadSessionFromFile())
            {
                bool success = await FetchOlympusSession();

                if (!success)
                {
                    Logger.Info("Available sesion is not valid any more. Continuing with normal login.");
                    Console.WriteLine("Available session is not valid any more. Continuing with normal login.");
                }
            }

            await DoLoginFlurlAsync();
        }

        private bool LoadSessionFromFile()
        {
            string path = Directory.GetCurrentDirectory();
            path += $"/natukaship/{_username}/cookie";

            if (File.Exists(path))
            {
                _cookieJar = cookieManager.ReadCookiesFromDisk();
                return true;
            }

            return false;
        }

        private (bool, Cookie, CookieCollection) HasImportantCookie(CookieContainer cookieJar)
        {
            CookieCollection cookies = cookieJar.GetCookies(new Uri("https://idmsa.apple.com"));
            foreach (Cookie cookie in cookies)
            {
                if (cookie.Name.Contains("DES"))
                {
                    return (true, cookie, cookies);
                }
            }
            return (false, new Cookie(), cookies);
        }

        private async Task DoLoginFlurlAsync()
        {
            var (hasImportantCookie, importantCookie, cookies) = HasImportantCookie(_cookieJar);
            string modifiedCookie = "";
            int responseCode = 200;
            string requestParams = "";
            string responseResult = "";
            var response = new HttpResponseMessage();

            try
            {
                var client = "https://idmsa.apple.com/appleauth/auth/signin".WithClient(flurlClient)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("X-Requested-With", "XMLHttpRequest")
                    .WithHeader("Accept", "application/json, text/javascript")
                    .WithHeader("X-Apple-Widget-Key", _service_key);

                if (hasImportantCookie && !string.IsNullOrEmpty(importantCookie.Value))
                {
                    foreach (Cookie cookie in cookies) { modifiedCookie += $"{cookie.ToString()};"; }

                    string unescapedImportantCookie = $"{importantCookie.Name}={importantCookie.Value}";
                    string escapedImportantCookie = $"{importantCookie.Name}=\"{importantCookie.Value}\"";
                    modifiedCookie = modifiedCookie.Replace(unescapedImportantCookie, escapedImportantCookie);

                }

                if (!string.IsNullOrEmpty(modifiedCookie))
                    client.WithHeader("Cookie", modifiedCookie);

                object data = new
                {
                    accountName = _username,
                    password = _password,
                    rememberMe = true
                };

                response = await client.PostJsonAsync(data);

                cookieManager.WriteCookiesToDisk(_cookieJar);

                responseResult = response.Content.ReadAsStringAsync().Result;
                try { responseResult = JObject.Parse(responseResult).ToString(Formatting.None); } catch { }

                requestParams = Regex.Replace(JObject.FromObject(data).ToString(Formatting.None), @"""password"":"".+"",", "\"password\":\"*****\",");
                responseCode = response.StatusCode.GetHashCode();
                _appleId = response.GetHeaderValue("X-Apple-ID-Session-Id");
                _scnt = response.GetHeaderValue("scnt");

                Logger.Info($">> POST https://idmsa.apple.com/appleauth/auth/signin: [undefined body]");
                Logger.Debug($"<< POST https://idmsa.apple.com/appleauth/auth/signin: {responseCode} {responseResult} - {requestParams}");

                switch (responseCode)
                {
                    case 200:
                        await FetchOlympusSession();
                        IsLoggedIn = true;
                        Logger.Info($">> Logged in Successfully with User: {_username}.");
                        break;
                    case 401:
                        if (responseResult.Contains("Your Apple ID or password was incorrect"))
                            throw new InvalidUserCredentialsException($"Invalid username and password combination. Used '{_username}' as the username!");

                        string msg = "Auth lost";
                        Logger.Warn(msg);
                        throw new Exceptions.UnauthorizedAccessException(msg);
                    case 403:
                        string errorMessage = $"Invalid username and password combination. Used '{_username}' as the username!";
                        Logger.Warn(errorMessage);
                        throw new InvalidUserCredentialsException(errorMessage);
                    case 409:
                        bool twoFactorSuccess = await HandleTwoStepOrFactorAuthentication(response);
                        bool sessionSuccess = await FetchOlympusSession();
                        if (twoFactorSuccess && sessionSuccess)
                        {
                            IsLoggedIn = true;
                            Logger.Info($">> Logged in Successfully with User: {_username}.");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.Warn($">> POST https://idmsa.apple.com/appleauth/auth/signin: {responseCode} - {responseResult} - {requestParams} : {response.ReasonPhrase}");
                Logger.Warn(ex.GetType() + ": " + ex.Message);
            }
        }

        private async Task<bool> HandleTwoStepOrFactorAuthentication(HttpResponseMessage response)
        {
            string responseResult = "";
            try
            {
                _appleId = response.GetHeaderValue("X-Apple-ID-Session-Id");
                _scnt = response.GetHeaderValue("scnt");

                var requestClient = "https://idmsa.apple.com/appleauth/auth".WithClient(flurlClient)
                    .WithHeader("X-Apple-Id-Session-Id", _appleId)
                    .WithHeader("X-Apple-Widget-Key", _service_key)
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Scnt", _scnt);

                var resp = await requestClient.GetAsync();

                responseResult = resp.Content.ReadAsStringAsync().Result;
                try { responseResult = JObject.Parse(responseResult).ToString(Formatting.None); } catch { }
                int responseCode = resp.StatusCode.GetHashCode();

                Logger.Info($">> GET https://idms.apple.com/appleauth/auth: [undefined body]");
                Logger.Debug($"<< GET https://idms.apple.com/appleauth/auth: {responseCode} {responseResult}");

                var jsonResponse = JObject.Parse(responseResult);
                var auth = new TwoStepOrFactorClient(jsonResponse, _appleId, _scnt, _cookieJar, _username, _service_key);

                return await auth.CheckAuthenticationAsync();
            }
            catch (Exception)
            {
                Logger.Warn($"Although response from Apple indicated activated Two-step Verification or Two-factor Authentication, natukaship didn't know how to handle this response: {JObject.Parse(responseResult).ToString(Formatting.None)}");
                return false;
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
        public async Task<(string, int)> SendRequestAsync(string method, string url, object data = null, string contentType = "application/json", Dictionary<string, string> headers = null)
        {
            var requestClient = url.WithClient(flurlClient);

            if (method == "POST")
            {
                HttpResponseMessage postResponse;
                string requestParams = "";

                if (EnsureCSRFToken && (!url.Contains("list") || url.Contains("validateDeveloperDetails")))
                {
                    requestClient.WithHeader("Csrf", CsrfToken);
                    requestClient.WithHeader("Csrf_ts", CsrfTokenTs);
                }

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        requestClient.WithHeader(header.Key, header.Value);
                    }

                    UploadFile uploadFile = JObject.FromObject(data).ToObject<UploadFile>();
                    HttpContent fileContent = new ByteArrayContent(uploadFile.bytes);

                    postResponse = await requestClient.PostAsync(fileContent);
                }
                else
                {
                    if (data == null)
                        postResponse = await requestClient.PostAsync(null);
                    else
                    {
                        if (data.GetType().IsArray)
                            requestParams = JArray.FromObject(data).ToString(Formatting.None);
                        else if (!data.GetType().IsArray && headers == null)
                            requestParams = JObject.FromObject(data).ToString(Formatting.None);

                        if (contentType == "application/x-www-form-urlencoded")
                            postResponse = await requestClient.PostUrlEncodedAsync(data);
                        else
                        {
                            if (data.GetType().IsArray)
                            {
                                var dData = JsonConvert.SerializeObject(data);
                                postResponse = await requestClient.PostStringAsync(dData);
                            }
                            else
                                postResponse = await requestClient.PostJsonAsync(data);
                        }
                    }
                }

                int statusCode = postResponse.StatusCode.GetHashCode();
                string responseResult = postResponse.Content.ReadAsStringAsync().Result;
                try { responseResult = JObject.Parse(responseResult).ToString(Formatting.None); } catch { }

                if (EnsureCSRFToken && (url.Contains("list") || url.Contains("validateDeveloperDetails")))
                {
                    IEnumerable<string> csrfValues;
                    if (postResponse.Headers.TryGetValues("csrf", out csrfValues))
                        CsrfToken = csrfValues.FirstOrDefault();

                    IEnumerable<string> csrfTsValues;
                    if (postResponse.Headers.TryGetValues("csrf_ts", out csrfTsValues))
                        CsrfTokenTs = csrfTsValues.FirstOrDefault();
                }

                Logger.Info($">> {method} {url}: [undefined body]");
                Logger.Debug($"<< {method} {url}: {statusCode} {responseResult} - {requestParams}");

                cookieManager.WriteCookiesToDisk(_cookieJar);

                return (responseResult, statusCode);
            }
            else if (method == "POST-string")
            {
                HttpResponseMessage postResponse;
                string requestParams = data.ToString();

                requestClient.WithHeader("Content-Type", "application/json");

                postResponse = await requestClient.PostStringAsync(data.ToString());

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
                if (contentType == "json/application")
                    requestClient.WithHeader("Content-Type", "application/json");

                HttpResponseMessage getResponse = await requestClient.GetAsync();

                int statusCode = getResponse.StatusCode.GetHashCode();
                string responseResult = getResponse.Content.ReadAsStringAsync().Result;
                try { responseResult = JObject.Parse(responseResult).ToString(Formatting.None); } catch { }

                if (responseResult.Contains("Unauthenticated"))
                    responseResult = Regex.Replace(responseResult, @"Unauthenticated\n\nRequest\sID:\s(\w|\.)*\n", "Unauthenticated");

                Logger.Info($">> {method} {url}: [undefined body]");
                Logger.Debug($"<< {method} {url}: {statusCode} {responseResult}");

                cookieManager.WriteCookiesToDisk(_cookieJar);

                return (responseResult, statusCode);
            }
        }

        /// <summary>
        /// Send request to the url
        /// </summary>
        /// <param name="method">method for request</param>
        /// <param name="url">url for request</param>
        /// <returns>file</returns>
        public async Task<string> SendDownloadRequestAsync(string method, string url, string downloadType)
        {
            var requestClient = url.WithClient(flurlClient);

            var (hasImportantCookie, importantCookie, cookies) = HasImportantCookie(_cookieJar);
            string modifiedCookie = "";

            if (hasImportantCookie && !string.IsNullOrEmpty(importantCookie.Value))
            {
                foreach (Cookie cookie in cookies) { modifiedCookie += $"{cookie.ToString()};"; }

                string unescapedImportantCookie = $"{importantCookie.Name}={importantCookie.Value}";
                string escapedImportantCookie = $"{importantCookie.Name}=\"{importantCookie.Value}\"";
                modifiedCookie = modifiedCookie.Replace(unescapedImportantCookie, escapedImportantCookie);

                requestClient.WithHeader("Cookie", modifiedCookie);
            }

            string path = Directory.GetCurrentDirectory();
            path += $"/natukaship/{_username}/{downloadType}";

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string downloadedFilePath = await requestClient.DownloadFileAsync(path);

            Logger.Info($">> {method} {url}: [undefined body]");
            Logger.Debug($"<< {method} {url}: File saved to \"{downloadedFilePath}\"");

            return downloadedFilePath;
        }

        /// <summary>
        /// Get password from the User via command line
        /// </summary>
        /// <returns>password</returns>
        private string FetchPasswordFromCLI()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                ConsoleKeyInfo cki = Console.ReadKey(true);
                if (cki.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                if (cki.Key == ConsoleKey.Backspace)
                {
                    //Prevent an exception when you hit backspace with no characters on the array.
                    if (sb.Length > 0)
                    {
                        sb.Remove(sb.Length - 1, 1);
                        Console.Write("\b \b");
                    }
                }
                sb.Append(cki.KeyChar);
            }

            return sb.ToString();
        }
    }
}
