using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Flurl.Http;
using Natukaship.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Natukaship
{
    class TwoStepOrFactorClient : JObject
    {
        private JObject _response;
        private string _username;
        private string _phoneNumber;
        private string _phoneNumberId;
        private string _codeType;
        private int _codeLength;
        private int depth = 0;
        private string _code;
        private string _appleId;
        private string _scnt;
        private HttpClientHandler clientHandler;
        private HttpClient httpClient;
        private FlurlClient flurlClient;
        public CookieManager cookieManager;
        public CookieContainer _cookieJar;
        public List<TrustedPhoneNumber> trustedPhoneNumbers;

        public string ENVSpacheshipCookiePath { get => Environment.GetEnvironmentVariable("NATUKASHIP_COOKIE_PATH"); }
        public string ENV2FASmsDefaultPhoneNumber { get => Environment.GetEnvironmentVariable("NATUKASHIP_2FA_SMS_DEFAULT_PHONE_NUMBER"); }

        private string ServiceKey { get; set; }

        public TwoStepOrFactorClient(JObject response, string appleId, string scnt, CookieContainer cookieJar, string username, string serviceKey)
        {
            _response = response;
            _appleId = appleId;
            _scnt = scnt;
            _cookieJar = cookieJar;
            _username = username;
            ServiceKey = serviceKey;
            cookieManager = new CookieManager(_username);

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
                .WithHeader("User-Agent", "Spaceship 2.134.0")
                .AllowAnyHttpStatus();
        }

        public async Task<bool> CheckAuthenticationAsync()
        {
            // get code using trusted devices
            if (_response.ContainsKey("trustedDevices"))
            {
                // handle_two_step(r)
                return false;
            }

            // get code using trusted phone numbers
            if (_response.ContainsKey("trustedPhoneNumbers"))
            {
                trustedPhoneNumbers = _response["trustedPhoneNumbers"].ToObject<List<TrustedPhoneNumber>>();
                return await HandleTwoFactorAsync();
            }

            return true;
        }

        private async Task<bool> HandleTwoFactorAsync()
        {
            if (depth == 0)
            {
                Console.WriteLine($"Two-factor Authentication (6 digits code) is enabled for account '{_username}'");
                Console.WriteLine("More information about Two-factor Authentication: https://support.apple.com/en-us/HT204915");
                Console.WriteLine("");
            }

            // "verification code" has already be pushed to devices

            var securityCode = _response["securityCode"].ToObject<SecurityCode>();
            _codeLength = securityCode.length;

            if (!(ENV2FASmsDefaultPhoneNumber == null))
            {
                if (string.IsNullOrEmpty(ENV2FASmsDefaultPhoneNumber))
                    throw new Exception("Environment variable NATUKASHIP_2FA_SMS_DEFAULT_PHONE_NUMBER is set, but empty.");

                Console.WriteLine("Environment variable `NATUKASHIP_2FA_SMS_DEFAULT_PHONE_NUMBER` is set, automatically requesting 2FA token via SMS to that number");
                Console.WriteLine($"NATUKASHIP_2FA_SMS_DEFAULT_PHONE_NUMBER = {ENV2FASmsDefaultPhoneNumber}");
                Console.WriteLine();

                _phoneNumber = ENV2FASmsDefaultPhoneNumber;
                _phoneNumberId = PhoneIdFromNumber(_phoneNumber);
                _codeType = "phone";
                await AskCodeFromPhone(_phoneNumber);
            }
            else
            {
                Console.WriteLine("(Input `sms` to escape this prompt and select a trusted phone number to send the code as a text message)");
                Console.WriteLine();
                Console.WriteLine("(You can also set the environment variable `SPACESHIP_2FA_SMS_DEFAULT_PHONE_NUMBER` to automate this)");
                Console.WriteLine("(Read more at: https://github.com/fastlane/fastlane/blob/master/spaceship/docs/Authentication.md#auto-select-sms-via-spaceship-2fa-sms-default-phone-number)");
                Console.WriteLine();

                _codeType = "trusteddevice";
                Console.WriteLine($"Please enter the {_codeLength} digit code:");
                _code = Console.ReadLine();

                // User exited by entering `sms` and wants to choose phone number for SMS
                if (_code == "sms")
                {
                    _codeType = "phone";
                    await RequestTwoFactorCodeFromPhoneChoose();
                }
            }

            Console.WriteLine("Requesting session...");

            // Send "verification code" back to server to get a valid session
            return await PostCodeAuthAsync();
        }

        private async Task RequestTwoFactorCodeFromPhoneChoose()
        {
            bool validSelection = false;
            int tries = 3;
            string selectedPhoneNumber = "";

            while (!validSelection)
            {
                if (tries == 0)
                    throw new Exception("Entered multiple incorrect verification code. Preventing from entering multiple incorrect verification code to avoid being locked.");

                Console.WriteLine("Please select a trusted phone number to send code to:");
                foreach (TrustedPhoneNumber current in trustedPhoneNumbers)
                {
                    Console.WriteLine($"{current.id}. {current.numberWithDialCode}");
                }
                Console.Write("?  ");
                _phoneNumberId = Console.ReadLine();

                string allPhoneNumbersId = "";
                trustedPhoneNumbers.ForEach(trustedPhoneNumber => allPhoneNumbersId += $"{trustedPhoneNumber.id.ToString()}; ");
                validSelection = allPhoneNumbersId.Contains($"{_phoneNumberId}; ");

                if (validSelection)
                {
                    selectedPhoneNumber = trustedPhoneNumbers.Find(trustedPhoneNumber => trustedPhoneNumber.id.ToString() == _phoneNumberId).numberWithDialCode;
                    break;
                }

                Console.WriteLine("Input a correct selection!\n");
            }

            await AskCodeFromPhone(selectedPhoneNumber);
        }

        private async Task<bool> AskCodeFromPhone(string phoneNumber)
        {
            if (depth == 0)
            {
                object data = new
                {
                    mode = "sms",
                    phoneNumber = new { id = _phoneNumberId }
                };

                var (responseResult, responseCode) = await SendRequestAsync("PUT", "https://idmsa.apple.com/appleauth/auth/verify/phone", data);
                var responseResultObject = Parse(responseResult).ToObject<VerifyPhoneResponseObject>();

                if (responseResultObject.serviceErrors == null)
                    Console.WriteLine($"Successfully requested text message to {phoneNumber}");
                else
                {
                    if (responseResultObject.serviceErrors.Count > 0)
                    {
                        string errMessage = responseResultObject.serviceErrors[0].message;
                        Logger.Warn($"PUT https://idmsa.apple.com/appleauth/auth/verify/phone: {responseCode} {errMessage}");
                        Console.WriteLine(errMessage);
                    }
                }
            }

            Console.WriteLine($"Please enter the {_codeLength} digit code you received at {phoneNumber}:");
            _code = Console.ReadLine();

            if (depth == 0)
                return true;

            return await PostCodeAuthAsync();
        }

        /// <summary>
        /// Send request to the url
        /// </summary>
        /// <param name="method">method for request</param>
        /// <param name="url">url for request</param>
        /// <returns>request response</returns>
        private async Task<(string, int)> SendRequestAsync(string method, string url, object data = null)
        {
            string requestParams = data == null ? "" : FromObject(data).ToString(Formatting.None);
            string responseResult;
            int statusCode;

            var requestClient = url.WithClient(flurlClient)
                    .WithClient(flurlClient)
                    .WithHeader("Content-Type", "application/json")
                    .WithHeader("Accept", "application/json")
                    .WithHeader("Scnt", _scnt)
                    .WithHeader("X-Apple-Id-Session-Id", _appleId)
                    .WithHeader("X-Apple-Widget-Key", ServiceKey);

            if (method == "POST")
            {
                HttpResponseMessage postResponse = await requestClient.PostJsonAsync(data);

                cookieManager.WriteCookiesToDisk(_cookieJar);

                statusCode = postResponse.StatusCode.GetHashCode();
                responseResult = postResponse.Content.ReadAsStringAsync().Result;
                try { responseResult = Parse(responseResult).ToString(Formatting.None); } catch { }

                Logger.Info($">> {method} {url}: [undefined body]");
                Logger.Debug($"<< {method} {url}: {statusCode} {responseResult} - {requestParams}");

                return (responseResult, statusCode);
            }

            if (method == "PUT")
            {
                HttpResponseMessage putResponse = await requestClient.PutJsonAsync(data);

                cookieManager.WriteCookiesToDisk(_cookieJar);

                statusCode = putResponse.StatusCode.GetHashCode();
                responseResult = putResponse.Content.ReadAsStringAsync().Result;
                try { responseResult = Parse(responseResult).ToString(Formatting.None); } catch { }

                Logger.Info($">> {method} {url}: [undefined body]");
                Logger.Debug($"<< {method} {url}: {statusCode} {responseResult} - {requestParams}");

                return (responseResult, statusCode);
            }

            HttpResponseMessage getResponse = await requestClient.GetAsync();

            cookieManager.WriteCookiesToDisk(_cookieJar);

            statusCode = getResponse.StatusCode.GetHashCode();
            responseResult = getResponse.Content.ReadAsStringAsync().Result;
            try { responseResult = Parse(responseResult).ToString(Formatting.None); } catch { }

            Logger.Info($">> {method} {url}: [undefined body]");
            Logger.Debug($"<< {method} {url}: {statusCode} {responseResult}");

            return (responseResult, statusCode);
        }

        private string PhoneIdFromNumber(string phoneNumber)
        {
            // start with e.g. +49 162 1234585 or +1-123-456-7866
            phoneNumber = Regex.Replace(phoneNumber, @"\s|\-|\(\)|""""", "");
            // cleaned: +491621234585 or +11234567866

            foreach (TrustedPhoneNumber trustedPhoneNumber in trustedPhoneNumbers)
            {
                // start with: +49 •••• •••••85 or +1 (•••) •••-••66
                string numberWithDialCodeMasked = Regex.Replace(trustedPhoneNumber.numberWithDialCode, @"\s|\-|\(\)|""""", "");
                // cleaned: +49•••••••••85 or +1••••••••66

                int maskingsCount = numberWithDialCodeMasked.Count(num => num == '•'); // => 9 or 8
                // following regex: range from maskings_count-2 because sometimes the masked number has 1 or 2 dots more than the actual number
                // e.g. https://github.com/fastlane/fastlane/issues/14969
                string numberWithDialCodeRegexPart = Regex.Replace(numberWithDialCodeMasked, $@"^(?<first>[0-9+]{{2,4}})([•]{{{maskingsCount}}})(?<last>[0-9]{{2}})$", $@"${{first}}([0-9]{{{maskingsCount - 2},{maskingsCount}}})${{last}}");
                // => +49([0-9]{8,9})85 or +1([0-9]{7,8})66

                string backslash = "\\";
                numberWithDialCodeRegexPart = backslash + numberWithDialCodeRegexPart;
                Regex numberWithDialCodeRegex = new Regex($"^{numberWithDialCodeRegexPart}$");
                // => /^\+49([0-9]{8})85$/ or /^\+1([0-9]{7,8})66$/

                if (numberWithDialCodeRegex.IsMatch(phoneNumber))
                    return trustedPhoneNumber.id.ToString();
                // +491621234585 matches /^\+49([0-9]{8})85$/
            }

            string trustedNumbersDetails = "";
            List<string> trustedNumbersDetailsList = new List<string> { };
            trustedPhoneNumbers.ForEach(num => trustedNumbersDetailsList.Add(FromObject(num).ToString(Formatting.None)));
            trustedNumbersDetails = string.Join(", ", trustedNumbersDetailsList.ToArray());
            string[] errors = {
                $"Could not find a matching phone number to {phoneNumber} in {trustedNumbersDetails}.",
                "Make sure your environment variable is set to the correct phone number.",
                "If it is, please open an issue at https://github.com/fastlane/fastlane/issues/new and include this output so we can fix our matcher. Thanks."
            };
            Console.WriteLine($"{string.Join("\n", errors)}");

            throw new TunesException($"{string.Join("\n", errors)}");
        }

        private async Task<bool> PostCodeAuthAsync()
        {
            try
            {
                object data = new { securityCode = new { code = _code } };

                if (_codeType == "phone")
                {
                    data = new { securityCode = new { code = _code }, mode = "sms", phoneNumber = new { id = _phoneNumberId } };
                }

                var (responseResult, responseCode) =
                    await SendRequestAsync("POST", $"https://idmsa.apple.com/appleauth/auth/verify/{_codeType}/securitycode", data);

                switch (responseCode)
                {
                    case 401:
                    case 403:
                        throw new InvalidUserCredentialsException($"Invalid username and password combination. Used '{_username}' as the username.!");
                    case 404:
                        string msg = "Auth lost";
                        Logger.Warn(msg);
                        throw new Exceptions.UnauthorizedAccessException(msg);
                    case 400:
                        if (depth == 2)
                        {
                            Console.WriteLine("Entered multiple incorrect verification code.");
                            throw new ExceededMaximumVerificationAttemptException("Exceeded maximum verification code attempt. Preventing from entering multiple incorrect verification code to avoid being locked.");
                        }
                        depth += 1;
                        Console.WriteLine("Error: Incorrect verification code");

                        if (!string.IsNullOrEmpty(ENV2FASmsDefaultPhoneNumber) && _codeType == "phone")
                            return await AskCodeFromPhone(_phoneNumber);

                        return await HandleTwoFactorAsync();
                    case 204:
                    default:
                        await StoreSession();
                        return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetType() + ": " + ex.Message);

                return false;
            }
        }

        private async Task StoreSession()
        {
            await SendRequestAsync("GET", "https://idmsa.apple.com/appleauth/auth/2sv/trust");
        }
    }
}
