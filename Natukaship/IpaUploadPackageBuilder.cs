using System;
using System.IO;
using System.Xml;

namespace Natukaship
{
    // Builds a package for the binary ready to be uploaded with the iTunes Transporter
    public class IpaUploadPackageBuilder
    {
        public string PackagePath { get; set; }
        public string IpaPath { get; set; }

        public string Generate(string appId, string ipaPath = null, string packagePath = null, string platform = "ios")
        {
            PackagePath = Path.Join(packagePath, $"{appId}.itmsp");

            if (Directory.Exists(PackagePath))
                Directory.Delete(PackagePath, true);

            Directory.CreateDirectory(PackagePath);

            IpaPath = CopyIpa(ipaPath);

            var data = new
            {
                appleId = appId,
                fileSize = new FileInfo(IpaPath).Length,
                ipaPath = Path.GetFileName(IpaPath),
                md5 = Utilities.Md5Digest(IpaPath),
                archiveType = "bundle",
                platform = platform // pass "appletvos" for Apple TV's IPA
            };

            XmlDocument doc = new XmlDocument();
            string xmlPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Natukaship", "Assets/XMLTemplate.xml"));

            doc.Load(xmlPath);
            XmlNode root = doc.DocumentElement;
            root.ChildNodes[0].Attributes["apple_id"].Value = data.appleId;
            root.ChildNodes[0].Attributes["app_platform"].Value = data.platform;
            root.ChildNodes[0].ChildNodes[0].Attributes["type"].Value = data.archiveType;
            root.ChildNodes[0].ChildNodes[0].ChildNodes[0]["size"].InnerText = data.fileSize.ToString();
            root.ChildNodes[0].ChildNodes[0].ChildNodes[0]["file_name"].InnerText = data.ipaPath;
            root.ChildNodes[0].ChildNodes[0].ChildNodes[0]["checksum"].InnerText = data.md5;
            doc.Save(Path.Combine(PackagePath + "/metadata.xml"));

            return packagePath;
        }

        public string UniqueIpaPath(string ipaPath)
        {
            return $"{Utilities.SHA256Digest(ipaPath)}.ipa";
        }

        private string CopyIpa(string ipaPath)
        {
            string ipaFileName = UniqueIpaPath(ipaPath);
            string resultingPath = Path.Join(PackagePath, ipaFileName);
            File.Copy(ipaPath, resultingPath);

            return resultingPath;
        }
    }
}
