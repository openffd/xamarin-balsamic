using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;

namespace Natukaship
{
    public class CookieManager
    {
        private string _username;
        private string file;

        public string ENVSpacheshipCookiePath { get => Environment.GetEnvironmentVariable("SPACESHIP_COOKIE_PATH"); }

        public CookieManager(string username)
        {
            _username = username;
            file = PersistentCookiePath();
        }

        private void CreateFile()
        {
            try
            {
                File.Create(file).Dispose();
            }
            catch (DirectoryNotFoundException)
            {
                file = file.Replace("/cookie", "");
                Directory.CreateDirectory(file);
                file += "/cookie";
                File.Create(file).Dispose();
            }
        }

        public void WriteCookiesToDisk(CookieContainer cookieJar)
        {
            try
            {
                if (!File.Exists(file))
                    CreateFile();

                using (Stream stream = File.Create(file))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, cookieJar);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Problem writing cookies to disk: " + e.GetType());
                File.Delete(file);
            }
        }

        public CookieContainer ReadCookiesFromDisk()
        {
            try
            {
                // prevent reading the file if its empty
                if (new FileInfo(file).Length == 0)
                    return new CookieContainer();

                using (Stream stream = File.Open(file, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return (CookieContainer)formatter.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                Console.Out.WriteLine("Problem reading cookies from disk: " + e.GetType());
                File.Delete(file);
                CreateFile();

                return new CookieContainer();
            }
        }

        // Returns preferred path for storing cookie
        // for two step verification.
        private string PersistentCookiePath()
        {
            string path = Directory.GetCurrentDirectory();
            path += $"/natukaship/{_username}/cookie";

            return path;
        }
    }
}
