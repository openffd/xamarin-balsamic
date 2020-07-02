namespace Natukaship
{
    public class UserDetail
    {
        public string contentProviderId { get; set; }
        public SessionToken sessionToken { get; set; }

        public string dsId => sessionToken.dsId;
    }
}
