using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Natukaship
{
    public class UploadFile
    {
        public string filePath { get; set; }
        public string fileName { get; set; }
        public long fileSize { get; set; }
        public string contentType { get; set; }
        public byte[] bytes { get; set; }

        public static UploadFile FromPath(string path)
        {
            if (!File.Exists(path))
                throw new Exception($"Image must exists at path: {path}");

            // md5 from original. keeping track of md5s allows to skip previously uploaded in deliver
            var contentMd5 = Utilities.Md5Digest(path);

            if (Path.GetExtension(path.ToLower()).Contains(".png"))
                path = RemoveAlphaChannel(path);

            string contentType = Utilities.ContentType(path);
            var mi = Utilities.GetMediaInfo(path);

            return new UploadFile
            {
                filePath = path,
                fileName = "ftl_" + contentMd5 + "_" + Path.GetFileName(path),
                fileSize = mi.Size,
                contentType = contentType,
                bytes = File.ReadAllBytes(path)
            };
        }

        // As things like screenshots and app icon shouldn't contain the alpha channel
        // This will copy the image into /tmp to remove the alpha channel there
        // That's done to not edit the original image
        public static string RemoveAlphaChannel(string originalPath)
        {
            string md5Path = "";
            using (var md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(originalPath);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                md5Path = sb.ToString();
            }

            string path = $"/tmp/{md5Path}.png";

            try
            {
                File.Copy(originalPath, path);
            }
            catch (IOException ex)
            {
                if (ex.Message.Contains("already exists"))
                    File.Delete(path);

                File.Copy(originalPath, path);
            }

            if (IsMacOS())
            {
                File.Create($"/tmp/{md5Path}.command").Close();

                string[] lines = { "#! /bin/bash", "sips -s format bmp '$3' &> /dev/null", "sips -s format png '$3'" };
                File.WriteAllLines($"/tmp/{md5Path}.command", lines);

                new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = $"chmod",
                        Arguments = $"u+x /tmp/{md5Path}.command {path}",
                    }
                }.Start();

                File.Delete($"/tmp/{md5Path}.command");
            }

            return path;
        }

        public static bool IsMacOS()
        {
            return Regex.Matches(System.Runtime.InteropServices.RuntimeInformation.OSDescription, @"Darwin").Count > 0;
        }
    }
}
