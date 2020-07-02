namespace Natukaship
{
    public class AppVersionRef
    {
        public string ssoTokenForVideo { get; set; }
        public string ssoTokenForImage { get; set; }
    }

    public class AppVersionRefResponseObject
    {
        public AppVersionRef data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
