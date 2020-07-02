using System;
namespace Natukaship
{
    public class NatukashipSetting
    {
        /// <summary>
        /// Boolean to check if going to use a proxy
        /// </summary>
        public bool UseProxy { get; set; } = false;

        /// <summary>
        /// IP Address of Proxy Server
        /// </summary>
        public string ProxyAddress { get; set; }

        /// <summary>
        /// Apple Developer Password
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Apple Developer Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// User-Agent to be used for sending a request
        /// </summary>
        public string UserAgent { get; set; } = "Spaceship 2.135.2";
    }
}
