using System.Collections.Generic;

namespace Natukaship
{
    public class DeveloperResponse
    {
        public bool isHidden { get; set; }
        public long lastModified { get; set; }
        public string pendingState { get; set; }
        public string response { get; set; }
        public int responseId { get; set; }
    }

    public class ReviewValue
    {
        public int id { get; set; }
        public int rating { get; set; }
        public string title { get; set; }
        public string review { get; set; }
        public double lastModified { get; set; }
        public string nickname { get; set; }
        public string storeFront { get; set; }
        public DeveloperResponse developerResponse { get; set; }
    }

    public class Review
    {
        public ReviewValue value { get; set; }
    }

    public class AppReviewsData
    {
        public int reviewCount { get; set; }
        public List<Review> reviews { get; set; }
    }

    public class AppReviewsResponseObject
    {
        public AppReviewsData data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
