namespace Natukaship
{
    public class AppTrailer
    {
        public string videoUrl { get; set; }
        public string previewImageUrl { get; set; }
        public string fullSizedPreviewImageUrl { get; set; }
        public string deviceType { get; set; }
        public string language { get; set; }

        public string videoAssetToken { get; set; }
        public string pictureAssetToken { get; set; }
        public string descriptionXML { get; set; }
        public string previewFrameTimeCode { get; set; }
        public string contentType { get; set; }
        public int? sortPosition { get; set; }
        public int? size { get; set; }
        public int? width { get; set; }
        public int? height { get; set; }
        public string checksum { get; set; }
        public bool isPortrait { get; set; }
    }

    public class TrailerValue
    {
        public AppTrailer value { get; set; }
    }
}
