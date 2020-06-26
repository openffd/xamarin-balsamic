using System;
using System.Net;
using System.Net.Http;
using static System.Net.DecompressionMethods;

namespace BalsamicConnect
{
    public class TunesClient
    {
        private HttpClientHandler HttpClientHandler { get; set; }

        #region Initialization

        public TunesClient()
        {
            HttpClientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = Deflate | GZip,
                CookieContainer = new CookieContainer(),
                ServerCertificateCustomValidationCallback = (message, certificate, chain, errors) => true,
                UseCookies = true,
                UseDefaultCredentials = false,
            };
        }

        #endregion
    }
}
