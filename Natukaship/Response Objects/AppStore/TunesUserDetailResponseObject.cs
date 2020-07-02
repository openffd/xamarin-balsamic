using System.Collections.Generic;

namespace Natukaship
{
    public class PermittedActivities
    {
        public List<string> VIEW { get; set; }
        public List<string> EDIT { get; set; }
    }

    public class ContentProvider
    {
        public List<string> contentProviderTypes { get; set; }
        public int contentProviderId { get; set; }
        public string contentProviderPublicId { get; set; }
        public string name { get; set; }
    }

    public class AssociatedAccount
    {
        public ContentProvider contentProvider { get; set; }
        public List<string> roles { get; set; }
        public object lastLogin { get; set; }
    }

    public class SessionToken
    {
        public string dsId { get; set; }
        public int contentProviderId { get; set; }
        public object ipAddress { get; set; }
    }

    public class UserData
    {
        public List<AssociatedAccount> associatedAccounts { get; set; }
        public SessionToken sessionToken { get; set; }
        public PermittedActivities permittedActivities { get; set; }
        public string preferredCurrencyCode { get; set; }
        public object preferredCountryCode { get; set; }
        public string countryOfOrigin { get; set; }
        public List<string> contentProviderFeatures { get; set; }
        public List<object> userFeatures { get; set; }
        public bool isLocaleNameReversed { get; set; }
        public object feldsparToken { get; set; }
        public object feldsparChannelName { get; set; }
        public string lastname { get; set; }
        public bool visibility { get; set; }
        public bool isEmailInvalid { get; set; }
        public bool hasContractInfo { get; set; }
        public bool DYCVisibility { get; set; }
        public bool canEditITCUsersAndRoles { get; set; }
        public bool canViewITCUsersAndRoles { get; set; }
        public bool canEditIAPUsersAndRoles { get; set; }
        public bool transporterEnabled { get; set; }
        public bool hasPendingFeldsparBindingRequest { get; set; }
        public bool isLegalUser { get; set; }
        public bool isPreOrderBlacklisted { get; set; }
        public string contentProviderPublicId { get; set; }
        public string contentProvider { get; set; }
        public string firstname { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
        public string contentProviderType { get; set; }
        public string contentProviderId { get; set; }
        public string displayName { get; set; }
    }

    public class TunesUserDetailResponseObject
    {
        public string statusCode { get; set; }
        public UserData data { get; set; }
        public Messages messages { get; set; }
    }
}
