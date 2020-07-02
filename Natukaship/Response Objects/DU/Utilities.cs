using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Enums;
using Xabe.FFmpeg.Streams;

namespace Natukaship
{
    public class Utilities
    {
        public static string ContentType(string path)
        {
            // Identifies the content_type of a file based on its file name extension.
            // Supports all formats required by DU-UTC right now (video, images and json)
            var supportedFileTypes = new Dictionary<string, string>
            {
                { ".jpg", "image/jpeg" },
                { ".jpeg", "image/jpeg" },
                { ".png", "image/png" },
                { ".geojson", "application/json" },
                { ".mov", "video/quicktime" },
                { ".m4v", "video/mp4" },
                { ".mp4", "video/mp4" },
                { ".txt", "text/plain" },
                { ".pdf", "application/pdf" },
                { ".doc", "application/msword" },
                { ".docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
                { ".rtf", "application/rtf" },
                { ".pages", "application/x-iwork-pages-sffpages" },
                { ".xls", "application/vnd.ms-excel" },
                { ".xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
                { ".numbers", "application/x-iwork-numbers-sffnumbers" },
                { ".rar", "application/x-rar-compressed" },
                { ".plist", "application/xml" },
                { ".crash", "text/x-apport" },
                { ".avi", "video/x-msvideo" },
                { ".zip", "application/zip" }
            };

            var fileExt = Path.GetExtension(path.ToLower());
            if (supportedFileTypes.ContainsKey(fileExt))
                return supportedFileTypes[fileExt];

            throw new Exception($"Unknown content-type for file {path}");
        }

        // Identifies the resolution of a video or an image.
        // Supports all video and images required by DU-UTC right now
        // @param path (String) the path to the file
        public static int[] Resolution(string path)
        {
            if (ContentType(path).StartsWith("image"))
            {
                // return resolution of image
                IMediaInfo mediaInfo = Task.Run(async () => await MediaInfo.Get(path)).Result;
                IVideoStream stream = mediaInfo.VideoStreams.FirstOrDefault();

                return new int[] { stream.Width, stream.Height };
            }

            if (ContentType(path).StartsWith("video"))
            {
                // return resolution of video
                return VideoResolution(path);
            }

            throw new Exception($"Cannot find resolution of file {path}");
        }

        // @return (String) md5 checksum of given file
        public static string Md5Digest(string path)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        // @return (String) md5 checksum of given file
        public static string SHA256Digest(string path)
        {
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }

        public static IMediaInfo GetMediaInfo(string path)
        {
            return Task.Run(async () => await MediaInfo.Get(path)).Result;
        }

        // Is the video or image in portrait mode ?
        // Supports all video and images required by DU-UTC right now
        // @param path (String) the path to the file
        public static bool IsPortrait(string path)
        {
            var resolution = Resolution(path);
            return resolution[0] < resolution[1];
        }

        // Grabs a screenshot from the specified video at the specified timestamp using `ffmpeg`
        // @param videoPath (String) the path to the video file
        // @param timestamp (String) the `ffmpeg` timestamp format (e.g. 00.00)
        // @param dimensions (Array) the dimension of the screenshot to generate
        // @return the TempFile containing the generated screenshot
        public static FileInfo GrabVideoPreview(string videoPath, string timestamp, int targetWidth = 0, int targetHeight = 0)
        {
            IMediaInfo mediaInfo = Task.Run(async () => await MediaInfo.Get(videoPath)).Result;
            IVideoStream videoStream = mediaInfo.VideoStreams.FirstOrDefault();
            string output = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + FileExtensions.Jpg);

            var timestampDouble = double.Parse(timestamp, System.Globalization.CultureInfo.InvariantCulture);
            videoStream = videoStream.SetOutputFramesCount(1).SetSeek(TimeSpan.FromSeconds(timestampDouble));

            if (targetWidth != 0 && targetHeight != 0)
                videoStream = videoStream.SetSize(new VideoSize(targetWidth, targetHeight));

            Conversion.New().AddStream(videoStream).SetOutput(output);

            return new FileInfo(output);
        }

        public static int EpochTime
        {
            get
            {
                TimeSpan t = DateTime.UtcNow - new DateTime(1970, 1, 1);
                int secondsSinceEpoch = (int)t.TotalSeconds;
                return secondsSinceEpoch;
            }
        }

        // identifies the resolution of a video using `ffmpeg`
        // @param video_path (String) the path to the video file
        // @return [Array] the resolution of the video
        public static int[] VideoResolution(string videoPath)
        {
            IMediaInfo mediaInfo = Task.Run(async () => await MediaInfo.Get(videoPath)).Result;
            IVideoStream videoStream = mediaInfo.VideoStreams.FirstOrDefault();

            return new int[] { videoStream.Width, videoStream.Height };
        }
    }
}
