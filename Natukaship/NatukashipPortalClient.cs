using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Flurl.Http;
using Natukaship.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unidecode.NET;

namespace Natukaship
{
    // Dev Portal - Client
    // spaceship/lib/spaceship/portal/portal_client.rb
    public class NatukashipPortalClient : NatukashipClient
    {
        private AccountManager account;
        private string cwd = Directory.GetCurrentDirectory();

        public NatukashipPortalClient(NatukashipSetting setting) : base(setting)
        {
            Globals.PortalClient = this;
        }

        //////////////////////////
        /// @!group Init and Login
        //////////////////////////

        public string Hostname { get => $"https://developer.apple.com/services-account/{ProtocolVersion}"; }

        /// <summary>
        /// </summary>
        /// <param name="username">username (Optional)</param>
        /// <param name="password">password (Optional)</param>
        public async Task Login(string username = null, string password = null)
        {
            try
            {
                account = new AccountManager(username, password);
                User = account.User;
                Password = account.Password;

                if (string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password))
                {
                    throw new NoUserCredentialsException("No user credentials provided!");
                }

                await account.SendLoginRequestAsync();
                IsLoggedIn = account.IsLoggedIn;

                if (IsLoggedIn)
                {
                    teams = Teams;
                    teamId = teams[0].teamId;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Return a list of all available teams
        /// </summary>
        /// <returns>List<Team></returns>
        public List<Team> Teams
        {
            get
            {
                try
                {
                    if (!IsLoggedIn)
                        throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                    var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/listTeams.action"));
                    var (responseResult, responseCode) = task.Result;

                    var responseObject = JObject.Parse(responseResult).ToObject<ListTeamResponseObject>();
                    teams = responseObject.teams;

                    return teams;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new List<Team> { };
                }
            }
        }

        /// <summary>
        /// Fetches all information of the currently used team
        /// </summary>
        /// <returns>Team</returns>
        public Team TeamInformation()
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                return Teams.Find(team => team.teamId == teamId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new Team();
            }
        }

        //////////////////////////
        /// @!group Apps
        //////////////////////////

        /// <summary>
        /// Returns all apps available
        /// </summary>
        /// <returns>List<AppId></returns>
        public List<AppId> Apps()
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                object data = new
                {
                    teamId,
                    pageNumber = 1,
                    pageSize = 500,
                    sort = "name=asc"
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/identifiers/listAppIds.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;

                string requestParams = JObject.FromObject(data).ToString(Formatting.None);

                if (string.IsNullOrEmpty(teamId))
                {
                    Console.WriteLine("Please select a team.");
                    Logger.Warn($">> POST {Hostname}/account/ios/identifiers/listAppIds.action: {responseCode} - {responseResult} - {requestParams}");
                    return new List<AppId> { };
                }

                var appIdsList = JObject.Parse(responseResult).ToObject<ListAppsIdsResponseObject>();
                appIDs = appIdsList.appIds;

                return appIDs;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<AppId> { };
            }
        }

        /// <summary>
        /// Fetch a specific App ID details based on the bundle_id
        /// </summary>
        /// <param name="appIdId">ID of App ID</param>
        /// <returns>AppId</returns>
        public AppId DetailsForApp(string appIdId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appIdId))
                    throw new ArgumentException("Provide a valid app id", nameof(appIdId));

                object data = new
                {
                    teamId,
                    appIdId
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/identifiers/getAppIdDetail.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;

                var appIdDetailResponseObject = JObject.Parse(responseResult).ToObject<GetAppIdDetailResponseObject>();
                return appIdDetailResponseObject.appId;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new AppId();
            }
        }

        /// <summary>
        /// Update a service for the App ID with given AppService object
        /// </summary>
        /// <param name="appId">App ID</param>
        /// <param name="service">AppService</param>
        /// <returns>AppId</returns>
        public AppId UpdateServiceForApp(AppId appId, AppService service)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (appId == null)
                    throw new ArgumentException("Provide a valid app id", nameof(appId));

                if (service == null)
                    throw new ArgumentException("Provide a valid service", nameof(service));

                object data = new
                {
                    teamId,
                    displayId = appId.appIdId,
                    featureType = service.ServiceId,
                    featureValue = service.Value
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/identifiers/getAppIdDetail.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;

                return DetailsForApp(appId.appIdId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new AppId();
            }
        }

        /// <summary>
        /// Creates a new App ID on the Apple Dev Portal
        /// </summary>
        /// <param name="bundleId">ID of the app</param>
        /// <param name="name">name of the app</param>
        /// <param name="enabledServices">services enabled for newly created app (Optional)</param>
        public void CreateApp(string bundleId, string name, List<AppService> enabledServices = null)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(bundleId))
                    throw new ArgumentException("Provide a valid bundle id", nameof(bundleId));

                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("Provide a valid name", nameof(name));

                JObject requestParams = new JObject();

                if (bundleId.EndsWith("*", StringComparison.CurrentCulture))
                {
                    requestParams["type"] = "wildcard";
                    requestParams["identifier"] = bundleId;
                }
                else
                {
                    requestParams["type"] = "explicit";
                    requestParams["identifier"] = bundleId;
                    requestParams["inAppPurchase"] = "on";
                    requestParams["gameCenter"] = "on";
                }

                requestParams["name"] = name.Unidecode();
                requestParams["teamId"] = teamId;

                if (enabledServices != null)
                {
                    foreach (var appService in enabledServices)
                    {
                        requestParams[appService.ServiceId] = appService.Value;
                    }
                }

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/identifiers/addAppId.action", requestParams, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Delete this App ID. This action will most likely fail if the
        /// App ID is already in the store or there are active profiles
        /// </summary>
        /// <param name="appIdId">ID of the app</param>
        public void DeleteApp(string appIdId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appIdId))
                    throw new ArgumentException("Provide a valid app id", nameof(appIdId));

                object data = new
                {
                    teamId,
                    appIdId
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/identifiers/deleteAppId.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Update name of App ID
        /// </summary>
        /// <param name="appIdId">ID of the app</param>
        /// <param name="appName">name of the app</param>
        public void UpdateAppName(string appIdId, string appName)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appIdId))
                    throw new ArgumentException("Provide a valid app id", nameof(appIdId));

                if (string.IsNullOrEmpty(appName))
                    throw new ArgumentException("Provide a valid app name", nameof(appName));

                object data = new
                {
                    teamId,
                    appIdId,
                    name = appName.Unidecode()
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/identifiers/updateAppIdName.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //////////////////////////
        /// @!group Certificates
        //////////////////////////

        /// <summary>
        /// Returns the certificates of the User
        /// </summary>
        /// <param name="types">array of strings of certificate types</param>
        public List<CertRequest> Certificates(string[] types)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (types == null || types.Length < 1)
                    throw new ArgumentException("Provide a valid types", nameof(types));

                object data = new
                {
                    teamId,
                    types = string.Join(",", types),
                    pageNumber = 1,
                    pageSize = 500,
                    sort = "certRequestStatusCode=asc"
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/certificate/listCertRequests.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;

                var responseObject = JObject.Parse(responseResult).ToObject<ListCertRequestsResponseObject>();
                certRequests = responseObject.certRequests;

                return certRequests;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<CertRequest> { };
            }
        }

        /// <summary>
        /// Creates a distribution certificate
        /// </summary>
        /// <param name="appIdId">ID of App ID (Optional)</param>
        public CertRequest CreateDistributionCertificate(string appIdId = null, string csrFilename = null, string keyFilename = null)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appIdId))
                    throw new ArgumentException("Provide a valid app id", nameof(appIdId));

                if (string.IsNullOrEmpty(csrFilename))
                    throw new ArgumentException("Provide a valid csr filename", nameof(csrFilename));

                if (string.IsNullOrEmpty(keyFilename))
                    throw new ArgumentException("Provide a valid key filename", nameof(keyFilename));

                return CreateCertificate("WXV89964HE", appIdId, csrFilename: csrFilename, keyFilename: keyFilename);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new TunesException(ex.Message);
            }
        }

        /// <summary>
        /// Creates a development certificate
        /// </summary>
        /// <param name="appIdId">ID of App ID (Optional)</param>
        public CertRequest CreateDevelopmentCertificate(string appIdId = null, string csrFilename = null, string keyFilename = null)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(appIdId))
                    throw new ArgumentException("Provide a valid app id", nameof(appIdId));

                if (string.IsNullOrEmpty(csrFilename))
                    throw new ArgumentException("Provide a valid csr filename", nameof(csrFilename));

                if (string.IsNullOrEmpty(keyFilename))
                    throw new ArgumentException("Provide a valid key filename", nameof(keyFilename));

                return CreateCertificate("83Q87W3TGH", appIdId, csrFilename: csrFilename, keyFilename: keyFilename);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new TunesException(ex.Message);
            }
        }

        public void EnsureCSRFToken(string forType)
        {
            switch(forType)
            {
                case "Certificate":
                    // Send post request for certificate then save the csrf token
                    Certificates(new string[] { "83Q87W3TGH", "WXV89964HE" });
                    break;
                case "Provisioning":
                    // Send post request for provisioning profiles then save the csrf token
                    ProvisioningProfiles();
                    break;
            }
        }

        /// <summary>
        /// Creates a development certificate
        /// </summary>
        /// <param name="type"></param>
        /// <param name="appIdId">ID of App ID (Optional)</param>
        /// <param name="commonName">Common Name for CSR (Optional)</param>
        /// <param name="csrFilename">CSR Filename (Optional)</param>
        /// <param name="keyFilename">Key Filename (Optional)</param>
        public CertRequest CreateCertificate(string type, string appIdId = null, string commonName = "Example", string csrFilename = null, string keyFilename = null)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(type))
                    throw new ArgumentException("Provide a valid certificate type", nameof(type));

                if (string.IsNullOrEmpty(appIdId))
                    throw new ArgumentException("Provide a valid app id", nameof(appIdId));

                if (string.IsNullOrEmpty(csrFilename))
                    throw new ArgumentException("Provide a valid csr filename", nameof(csrFilename));

                if (string.IsNullOrEmpty(keyFilename))
                    throw new ArgumentException("Provide a valid key filename", nameof(keyFilename));

                SubmitCertificateRequestResponseObject result = null;
                account.EnsureCSRFToken = true;
                EnsureCSRFToken("Certificate");

                var csr = string.Empty;
                if (!Directory.Exists($"{cwd}/natukaship/Convertion"))
                    Directory.CreateDirectory($"{cwd}/natukaship/Convertion");

                string csrFilePath = $"{cwd}/natukaship/Convertion/{csrFilename}.certSigningRequest";
                string typeName = type == "83Q87W3TGH" ? "Development" : "Distribution";

                if (!File.Exists(csrFilePath))
                    csr = CreateCertificateSigningRequest(commonName, csrFilename, keyFilename);
                else
                    csr = File.ReadAllText(csrFilePath);

                object data = new
                {
                    teamId,
                    type,
                    csrContent = csr,
                    appIdId, // optional
                    specialIdentifierDisplayId = appIdId, // For requesting Web Push certificates
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/certificate/submitCertificateRequest.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;
                account.EnsureCSRFToken = false;

                try
                {
                    result = JObject.Parse(responseResult).ToObject<SubmitCertificateRequestResponseObject>();
                }
                catch (Exception)
                {
                    throw new TunesException($"Could not create another {typeName} certificate, reached the maximum number of available {typeName} certificates.");
                }

                Console.WriteLine($"{typeName} certificate successfully created!");

                return result.certRequest;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new TunesException(ex.Message);
            }
        }

        /// <summary>
        /// Create a certificate signing request
        /// </summary>
        /// <param name="commonName"></param>
        /// <returns>PEM Value of Created CertificateSigningRequest</returns>
        private string CreateCertificateSigningRequest(string commonName, string csrFilename, string keyFilename)
        {
            if (string.IsNullOrEmpty(commonName))
                throw new ArgumentException("Provide a valid common name", nameof(commonName));

            if (string.IsNullOrEmpty(csrFilename))
                throw new ArgumentException("Provide a valid csr filename", nameof(csrFilename));

            if (string.IsNullOrEmpty(keyFilename))
                throw new ArgumentException("Provide a valid key filename", nameof(keyFilename));

            var procRSA = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + $"openssl genrsa -out ./natukaship/Convertion/{keyFilename}.key 2048" + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            procRSA.Start();
            procRSA.WaitForExit();

            Console.WriteLine(procRSA.StandardOutput.ReadToEnd());

            var procCSR = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + $"openssl req -new -key ./natukaship/Convertion/{keyFilename}.key -out ./natukaship/Convertion/{csrFilename}.certSigningRequest -subj '/emailAddress={account.User}/CN=Example/C=US'" + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            procCSR.Start();
            procCSR.WaitForExit();

            Console.WriteLine(procCSR.StandardOutput.ReadToEnd());

            return File.ReadAllText($"./natukaship/Convertion/{csrFilename}.certSigningRequest");
        }

        /// <summary>
        /// Revoke the certificate. You shouldn't use this method probably
        /// </summary>
        /// <param name="certificateId"></param>
        /// <param name="type"></param>
        public List<CertRequest> RevokeCertificate(string certificateId, string type)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(certificateId))
                    throw new ArgumentException("Provide a valid Certificate ID!", nameof(certificateId));

                if (string.IsNullOrEmpty(type))
                    throw new ArgumentException("Provide a valid Certificate Type!", nameof(type));


                account.EnsureCSRFToken = true;
                EnsureCSRFToken("Certificate");

                object data = new
                {
                    teamId,
                    type,
                    certificateId
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/certificate/revokeCertificate.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;
                account.EnsureCSRFToken = false;

                RevokeCertificateResponseObject responseObject;

                try
                {
                    responseObject = JObject.Parse(responseResult).ToObject<RevokeCertificateResponseObject>();
                }
                catch (JsonReaderException ex)
                {
                    throw new Exception("JsonReaderException: Received unexpected result - " + responseResult);
                }

                return responseObject.certRequests;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Download a certificate
        /// </summary>
        /// <param name="certificateId">ID of certificate</param>
        /// <param name="type">type of certificate to download</param>
        public string DownloadCertificate(string certificateId, string type)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(certificateId))
                    throw new ArgumentException("Provide a valid Certificate ID!", nameof(certificateId));

                if (string.IsNullOrEmpty(type))
                    throw new ArgumentException("Provide a valid Certificate Type!", nameof(type));

                var task = Task.Run(async () => await account.SendDownloadRequestAsync("GET", $"{Hostname}/account/ios/certificate/downloadCertificateContent.action?certificateId={certificateId}&teamId={teamId}&type={type}", "Certificates"));
                var response = task.Result;

                Console.WriteLine($"Certificate saved to - {response}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        // Default password for p12 is: `123456`
        public (string result, string p12Filename) ConvertCerToP12(string email, string certificateFilename, string p12Filename, string pemFilename, string keyFilename, string csrFilename, string password = "123456")
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(keyFilename))
                    throw new ArgumentException("Provide a valid key filename", nameof(keyFilename));

                if (string.IsNullOrEmpty(csrFilename))
                    throw new ArgumentException("Provide a valid csr filename", nameof(csrFilename));

                if (string.IsNullOrEmpty(email))
                    throw new ArgumentException("Provide a valid email", nameof(email));

                if (string.IsNullOrEmpty(certificateFilename))
                    throw new ArgumentException("Provide a valid certificate filename", nameof(certificateFilename));

                if (string.IsNullOrEmpty(p12Filename))
                    throw new ArgumentException("Provide a valid p12 filename", nameof(p12Filename));

                if (string.IsNullOrEmpty(pemFilename))
                    throw new ArgumentException("Provide a valid pem filename", nameof(pemFilename));

                string path = Directory.GetCurrentDirectory() + "/natukaship/";

                if (!Directory.Exists(path + "Convertion"))
                    Directory.CreateDirectory(path + "Convertion");

                path += "Convertion/";

                // Download Apple World Wide Developer Cert
                var requestClient = "https://developer.apple.com/certificationauthority/AppleWWDRCA.cer".WithClient(flurlClient);

                _ = Task.Run(async () => await requestClient.DownloadFileAsync(path)).Result;

                if (keyFilename.EndsWith(".key"))
                    keyFilename = keyFilename.Replace(".key", "");

                if (certificateFilename.EndsWith(".cer"))
                    certificateFilename = certificateFilename.Replace(".cer", "");

                if (pemFilename.EndsWith(".pem"))
                    pemFilename = pemFilename.Replace(".pem", "");

                if (p12Filename.EndsWith(".p12"))
                    p12Filename = p12Filename.Replace(".p12", "");

                var procPEM = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = "-c \"" + $"openssl x509 -inform der -in {certificateFilename}.cer -out ./natukaship/Convertion/{pemFilename}.pem" + "\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                procPEM.Start();
                procPEM.WaitForExit();

                Console.WriteLine(procPEM.StandardOutput.ReadToEnd());

                var procAppleWWDRCA = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = "-c \"" + "openssl x509 -in ./natukaship/Convertion/AppleWWDRCA.cer -inform DER -out ./natukaship/Convertion/AppleWWDRCA.pem -outform PEM" + "\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                procAppleWWDRCA.Start();
                procAppleWWDRCA.WaitForExit();

                Console.WriteLine(procAppleWWDRCA.StandardOutput.ReadToEnd());

                var procP12 = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "/bin/bash",
                        Arguments = "-c \"" + $"openssl pkcs12 -export -out ./natukaship/Convertion/{p12Filename}.p12 -inkey ./natukaship/Convertion/{keyFilename}.key -in ./natukaship/Convertion/{pemFilename}.pem -certfile ./natukaship/Convertion/AppleWWDRCA.pem -password {password}" + "\"",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };

                procP12.Start();
                procP12.WaitForExit();

                var result = procP12.StandardOutput.ReadToEnd();

                Console.WriteLine(result);

                return (result, p12Filename: $"{p12Filename}.p12");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public string ExecuteBashCommand(string command)
        {
            // according to: https://stackoverflow.com/a/15262019/637142
            // thanks to this we will pass everything as one command
            command = command.Replace("\"", "\"\"");

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = "-c \"" + command + "\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            proc.WaitForExit();

            return proc.StandardOutput.ReadToEnd();
        }

        //////////////////////////
        /// @!group Provisioning Profiles
        //////////////////////////

        /// <summary>
        /// Returns the provisioning profiles of the User
        /// </summary>
        /// <returns>List<ProvisioningProfile></returns>
        public List<ProvisioningProfile> ProvisioningProfiles()
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                object data = new
                {
                    teamId,
                    pageNumber = 1,
                    pageSize = 500,
                    sort = "name=asc",
                    includeInactiveProfiles = true,
                    onlyCountLists = true
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/profile/listProvisioningProfiles.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;

                var responseObject = JObject.Parse(responseResult).ToObject<ListProvisioningProfileResponseObject>();
                provisioningProfiles = responseObject.provisioningProfiles;

                return provisioningProfiles;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<ProvisioningProfile> { };
            }
        }

        /// <summary>
        /// Returns the detail of provisioning profile
        /// </summary>
        /// <param name="provisioningProfileId">ID of privisioning profile</param>
        /// <returns>ProvisioningProfile</returns>
        public ProvisioningProfile ProvisioningProfileDetails(string provisioningProfileId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(provisioningProfileId))
                    throw new ArgumentException("Provide a valid Provisioning Profile ID!");

                object data = new
                {
                    teamId,
                    provisioningProfileId
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/profile/getProvisioningProfile.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;

                var responseObject = JObject.Parse(responseResult).ToObject<GetProvisioningProfileResponseObject>();
                provisioningProfile = responseObject.provisioningProfile;

                return provisioningProfile;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new ProvisioningProfile { };
            }
        }

        /// <summary>
        /// Creates a new development provisioning profile
        /// </summary>
        /// <param name="name">Name of Provisioning Profile</param>
        /// <param name="appIdId">ID of App ID</param>
        /// <param name="certificateIds">IDs of Certificates</param>
        /// <param name="deviceIds">IDs of Devices (Optional)</param>
        public void CreateDevelopmentProvisioningProfile(string name, string appIdId, string certificateIds, string deviceIds = null)
        {
            CreateProvisioningProfile(name, "limited", appIdId, certificateIds);
        }

        /// <summary>
        /// Creates a new distribution provisioning profile
        /// </summary>
        /// <param name="name">Name of Provisioning Profile</param>
        /// <param name="appIdId">ID of App ID</param>
        /// <param name="certificateIds">IDs of Certificates</param>
        /// <param name="deviceIds">IDs of Devices (Optional)</param>
        public void CreateDistributionProvisioningProfile(string name, string appIdId, string certificateIds, string deviceIds = null)
        {
            CreateProvisioningProfile(name, "store", appIdId, certificateIds);
        }

        /// <summary>
        /// Create a new provisioning profile
        /// </summary>
        /// <param name="name">Name of Provisioning Profile</param>
        /// <param name="distributionType">Type of Provisioning Profile</param>
        /// <param name="appIdId">ID of App ID</param>
        /// <param name="certificateIds">IDs of Certificates</param>
        /// <param name="deviceIds">IDs of Devices (Optional)</param>
        public void CreateProvisioningProfile(string name, string distributionType, string appIdId, string certificateIds, string deviceIds = null)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("Provide a valid name", nameof(name));

                if (string.IsNullOrEmpty(distributionType))
                    throw new ArgumentException("Provide a valid distribution type", nameof(distributionType));

                if (string.IsNullOrEmpty(appIdId))
                    throw new ArgumentException("Provide a valid app id", nameof(appIdId));

                if (string.IsNullOrEmpty(certificateIds))
                    throw new ArgumentException("Provide a valid certificate ids", nameof(certificateIds));

                if (string.IsNullOrEmpty(deviceIds))
                    throw new ArgumentException("Provide a valid device ids", nameof(deviceIds));

                // Populate devices variable to be used
                if (devices == null)
                    Devices();

                account.EnsureCSRFToken = true;
                EnsureCSRFToken("Provisioning");

                // if no deviceIds were passed, the default would be all devices
                // listed to user
                if (deviceIds == null)
                {
                    List<string> allDeviceIds = new List<string> { };
                    foreach(var device in devices) { allDeviceIds.Add(device.deviceId); }

                    deviceIds = string.Join(",", allDeviceIds.ToArray());
                }

                object data = new
                {
                    appIdId,
                    teamId,
                    distributionType,
                    provisioningProfileName = name,
                    deviceIds,
                    certificateIds
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/profile/createProvisioningProfile.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;
                account.EnsureCSRFToken = false;

                try
                {
                    JObject.Parse(responseResult).ToObject<CreateProvisioningProfileResponseObject>();

                    Console.WriteLine("Provisioning profile successfully created!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Download the provisioning profile. This will store
        /// the provisioning profile on the file system.
        /// </summary>
        /// <param name="profileId">ID of provisioning profile</param>
        public void DownloadProvisioningProfile(string profileId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(profileId))
                    throw new ArgumentException("Provide a valid Provisioning Profile ID!");

                var task = Task.Run(async () => await account.SendDownloadRequestAsync("GET", $"{Hostname}/account/ios/profile/downloadProfileContent?provisioningProfileId={profileId}&teamId={teamId}", "ProvisioningProfiles"));
                var response = task.Result;

                Console.WriteLine($"Provisioning profile saved to - {response}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Delete the provisioning profile
        /// </summary>
        /// <param name="profileId">ID of provisioning profile</param>
        public void DeleteProvisioningProfile(string profileId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(profileId))
                    throw new ArgumentException("Provide a valid Provisioning Profile ID!");

                account.EnsureCSRFToken = true;
                EnsureCSRFToken("Provisioning");

                object data = new
                {
                    teamId,
                    provisioningProfileId = profileId
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/profile/deleteProvisioningProfile.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;
                account.EnsureCSRFToken = false;

                try
                {
                    JObject.Parse(responseResult).ToObject<DeleteProvisioningProfileResponseObject>();

                    Console.WriteLine("Provisioning profile successfully deleted!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw new TunesException(responseResult);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Repair an existing provisioning profile
        /// </summary>
        /// <param name="profileId">ID of Provisioning Profile</param>
        /// <param name="name">Name of Provisioning Profile</param>
        /// <param name="distributionType">Type of Provisioning Profile</param>
        /// <param name="appIdId">ID of App ID</param>
        /// <param name="certificateIds">IDs of Certificates</param>
        /// <param name="deviceIds">IDs of Devices (Optional)</param>
        public void RepairProvisioningProfile(string profileId, string name, string distributionType, string appIdId, string certificateIds, string deviceIds = null)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("Provide a valid name", nameof(name));

                if (string.IsNullOrEmpty(distributionType))
                    throw new ArgumentException("Provide a valid distribution type", nameof(distributionType));

                if (string.IsNullOrEmpty(appIdId))
                    throw new ArgumentException("Provide a valid app id", nameof(appIdId));

                if (string.IsNullOrEmpty(certificateIds))
                    throw new ArgumentException("Provide a valid certificate ids", nameof(certificateIds));

                if (string.IsNullOrEmpty(deviceIds))
                    throw new ArgumentException("Provide a valid device ids", nameof(deviceIds));

                // Populate devices variable to be used
                if (devices == null)
                    Devices();

                account.EnsureCSRFToken = true;
                EnsureCSRFToken("Provisioning");

                // if no deviceIds were passed, the default would be all devices
                // listed to user
                if (deviceIds == null)
                {
                    List<string> allDeviceIds = new List<string> { };
                    foreach (var device in devices) { allDeviceIds.Add(device.deviceId); }

                    deviceIds = string.Join(",", allDeviceIds.ToArray());
                }

                object data = new
                {
                    appIdId,
                    teamId,
                    distributionType,
                    provisioningProfileId = profileId,
                    provisioningProfileName = name,
                    deviceIds,
                    certificateIds
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/profile/regenProvisioningProfile.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;
                account.EnsureCSRFToken = false;

                try
                {
                    JObject.Parse(responseResult).ToObject<RegenProvisioningProfileResponseObject>();

                    Console.WriteLine("Provisioning profile successfully repaired!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        //////////////////////////
        /// @!group Devices
        //////////////////////////

        /// <summary>
        /// Returns all devices registered
        /// </summary>
        /// <param name="includeDisabled">bool if including removed devices (Default: false)</param>
        /// <returns>List<Device></returns>
        public List<Device> Devices(bool includeDisabled = false)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                object data = new
                {
                    teamId,
                    pageNumber = 1,
                    pageSize = 500,
                    sort = "name=asc",
                    includeRemovedDevices = includeDisabled
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/device/listDevices.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;

                var responseObject = JObject.Parse(responseResult).ToObject<ListDevicesResponseObject>();
                devices = responseObject.devices;

                if (devices.Count == 0)
                    throw new TunesException("No devices found!");

                return devices;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Device> { };
            }
        }

        /// <summary>
        /// Returns specific devices
        /// </summary>
        /// <param name="deviceClass">class of device (ex. 'iphone', 'ipod', 'ipad', etc.)</param>
        /// <param name="includeDisabled">bool if including removed devices (Default: false)</param>
        /// <returns>List<Device></returns>
        public List<Device> DevicesByClass(string deviceClass, bool includeDisabled = false)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(deviceClass))
                    throw new ArgumentException("Provide a valid device class", nameof(deviceClass));

                object data = new
                {
                    teamId,
                    pageNumber = 1,
                    pageSize = 500,
                    sort = "name=asc",
                    deviceClasses = deviceClass,
                    includeRemovedDevices = includeDisabled
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/device/listDevices.action", data, "application/x-www-form-urlencoded"));
                var (responseResult, responseCode) = task.Result;

                var responseObject = JObject.Parse(responseResult).ToObject<ListDevicesResponseObject>();
                devices = responseObject.devices;

                if (devices.Count == 0)
                    throw new TunesException($"No devices found for {deviceClass}.");

                return devices;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Device> { };
            }
        }

        /// <summary>
        /// Register a new device
        /// </summary>
        /// <param name="deviceName">The name of new device</param>
        /// <param name="deviceId">The UDID of the new device</param>
        public void CreateDevice(string deviceName, string deviceId)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(deviceName))
                    throw new ArgumentException("Provide a valid device name", nameof(deviceName));

                if (string.IsNullOrEmpty(deviceId))
                    throw new ArgumentException("Provide a valid device id", nameof(deviceId));

                object data = new
                {
                    teamId,
                    deviceClasses = "iphone",
                    deviceNumbers = deviceId,
                    deviceNames = deviceName,
                    register = "single"
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/device/addDevices.action", data));
                var (responseResult, responseCode) = task.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Disable a device. This will invalidate all provisioning profiles that use this device
        /// </summary>
        /// <param name="deviceId">ID of device</param>
        /// <param name="deviceUdid">UDID of the device</param>
        public void DisableDevice(string deviceId, string deviceUdid)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(deviceId))
                    throw new ArgumentException("Provide a valid device id", nameof(deviceId));

                if (string.IsNullOrEmpty(deviceUdid))
                    throw new ArgumentException("Provide a valid device udid", nameof(deviceUdid));

                object data = new
                {
                    teamId,
                    deviceNumbers = deviceId,
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/device/deleteDevice.action", data));
                var (responseResult, responseCode) = task.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Enable a device
        /// </summary>
        /// <param name="deviceId">ID of device</param>
        /// <param name="deviceUdid">UDID of the device</param>
        public void EnableDevice(string deviceId, string deviceUdid)
        {
            try
            {
                if (!IsLoggedIn)
                    throw new Exceptions.UnauthorizedAccessException("No user is logged in. Kindly login a user first.");

                if (string.IsNullOrEmpty(deviceId))
                    throw new ArgumentException("Provide a valid device id", nameof(deviceId));

                if (string.IsNullOrEmpty(deviceUdid))
                    throw new ArgumentException("Provide a valid device udid", nameof(deviceUdid));

                object data = new
                {
                    teamId,
                    deviceNumbers = deviceId,
                    deviceNumber = deviceUdid
                };

                var task = Task.Run(async () => await account.SendRequestAsync("POST", $"{Hostname}/account/ios/device/enableDevice.action", data));
                var (responseResult, responseCode) = task.Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
