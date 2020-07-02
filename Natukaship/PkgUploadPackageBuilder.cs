using System;
using System.IO;
using System.Xml;

namespace Natukaship
{
    // Builds a package for the pkg ready to be uploaded with the iTunes Transporter
    public class PkgUploadPackageBuilder
    {
        public string PackagePath { get; set; }

        public string Generate(string appId, string pkgPath = null, string packagePath = null, string platform = "osx")
        {
            PackagePath = Path.Join(packagePath, $"{appId}.itmsp");

            if (Directory.Exists(PackagePath))
                Directory.Delete(PackagePath, true);

            Directory.CreateDirectory(PackagePath);

            pkgPath = CopyPkg(pkgPath);

            var data = new
            {
                appleId = appId,
                fileSize = new FileInfo(pkgPath).Length,
                ipaPath = Path.GetFileName(pkgPath), // this is only the base name as the ipa is inside the package
                md5 = Utilities.Md5Digest(pkgPath),
                archiveType = "product-archive",
                platform = platform // pass "appletvos" for Apple TV's IPA
            };

            XmlDocument doc = new XmlDocument();
            string xmlPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Natukaship", "Assets/XMLTemplate.xml"));

            doc.Load(xmlPath);
            XmlNode root = doc.DocumentElement;
            root.ChildNodes[0].Attributes["apple_id"].Value = data.appleId;
            root.ChildNodes[0].Attributes["app_platform"].Value = data.platform;
            root.ChildNodes[0].ChildNodes[0].Attributes["type"].Value = data.archiveType;
            root.ChildNodes[0].ChildNodes[0].ChildNodes[0]["size"].Value = data.fileSize.ToString();
            root.ChildNodes[0].ChildNodes[0].ChildNodes[0]["file_name"].Value = data.ipaPath;
            root.ChildNodes[0].ChildNodes[0].ChildNodes[0]["checksum"].Value = data.md5;
            doc.Save(Path.Combine(packagePath + "metadata.xml"));

            return packagePath;
        }

        public string UniquepkgPath(string pkgPath)
        {
            return $"{Utilities.SHA256Digest(pkgPath)}.ipa";
        }

        private string CopyPkg(string pkgPath)
        {
            string ipaFileName = UniquepkgPath(pkgPath);
            string resultingPath = Path.Join(PackagePath, ipaFileName);
            File.Copy(pkgPath, resultingPath);

            return resultingPath;
        }
    }
}
