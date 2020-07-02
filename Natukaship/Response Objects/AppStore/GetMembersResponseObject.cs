using System.Collections.Generic;

namespace Natukaship
{
    public class GetMembersResponseObject
    {
        public string statusCode { get; set; }
        public MembersData data { get; set; }
        public Messages messages { get; set; }
    }

    public class MembersData
    {
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public object value { get; set; }
        public List<string> roles { get; set; }
        public List<UserMember> users { get; set; }
    }

    public class UserMember
    {
        public MemberEmailAddress emailAddress { get; set; }
        public MemberFirstName firstName { get; set; }
        public MemberLastName lastName { get; set; }
        public string dsId { get; set; }
        public string userName { get; set; }
        public string userId { get; set; }
        public List<UserRole> roles { get; set; }
        public bool canBeDeleted { get; set; }
        public PreferredCurrency preferredCurrency { get; set; }
        public object preferredCountry { get; set; }
        public object activationExpiry { get; set; }
        public bool isActive { get; set; }
        public UserSoftwares userSoftwares { get; set; }
        public bool isSiloable { get; set; }
    }

    public class Activities
    {
        public List<string> EDIT { get; set; }
        public List<object> REPORT { get; set; }
        public List<string> VIEW { get; set; }
    }

    public class RoleValue
    {
        public string name { get; set; }
        public List<string> incompatibleRoles { get; set; }
        public Activities activities { get; set; }
        public bool canBeSiloed { get; set; }
    }

    public class UserRole : CommonSharedValues
    {
        public RoleValue value { get; set; }
    }

    public class PreferredCurrencyValue
    {
        public string name { get; set; }
        public string currencyCode { get; set; }
        public string countryName { get; set; }
        public string countryCode { get; set; }
        public string countryCode2d { get; set; }
    }

    public class PreferredCurrency : CommonSharedValues
    {
        public PreferredCurrencyValue value { get; set; }
    }

    public class UserSoftwaresValue
    {
        public bool grantAllSoftware { get; set; }
        public List<string> grantedSoftwareAdamIds { get; set; }
    }

    public class UserSoftwares : CommonSharedValues
    {
        public UserSoftwaresValue value { get; set; }
    }
}
