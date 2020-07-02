namespace Natukaship
{
    public class AppRatingsData
    {
        public int reviewCount { get; set; }
        public int ratingCount { get; set; }
        public int ratingOneCount { get; set; }
        public int ratingTwoCount { get; set; }
        public int ratingThreeCount { get; set; }
        public int ratingFourCount { get; set; }
        public int ratingFiveCount { get; set; }
        public int averageRating { get; set; }
        public object ratingsReset { get; set; }
    }

    public class AppRatingsResponseObject
    {
        public AppRatingsData data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
