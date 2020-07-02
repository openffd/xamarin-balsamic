
using System;
using System.Collections.Generic;

namespace Natukaship
{
    public class ExtendedTeamAttributes
    {
    }

    public class Privileges
    {
    }

    public class CurrentTeamMember
    {
        public string lastName { get; set; }
        public List<string> roles { get; set; }
        public string firstName { get; set; }
        public bool cipsAccess { get; set; }
        public string teamMemberId { get; set; }
        public string email { get; set; }
        public string developerStatus { get; set; }
        public Privileges privileges { get; set; }
        public long personId { get; set; }
    }

    public class Membership
    {
        public object purchaseType { get; set; }
        public object inMacDeviceResetWindow { get; set; }
        public string membershipProductId { get; set; }
        public bool inRenewalWindow { get; set; }
        public object statusDisplayValue { get; set; }
        public bool inIosDeviceResetWindow { get; set; }
        public string platform { get; set; }
        public string dateStart { get; set; }
        public string dateExpire { get; set; }
        public string membershipId { get; set; }
        public string status { get; set; }
        public string name { get; set; }
    }

    public class TeamAgent
    {
        public string lastName { get; set; }
        public string teamMemberId { get; set; }
        public string firstName { get; set; }
        public string email { get; set; }
        public string developerStatus { get; set; }
        public bool cipsAccess { get; set; }
        public long personId { get; set; }
    }

    public class Team
    {
        public object enrollment { get; set; }
        public object typeLabel { get; set; }
        public bool xcodeFreeOnly { get; set; }
        public ExtendedTeamAttributes extendedTeamAttributes { get; set; }
        public string dateCreated { get; set; }
        public string teamId { get; set; }
        public CurrentTeamMember currentTeamMember { get; set; }
        public string type { get; set; }
        public List<Membership> memberships { get; set; }
        public object statusLabel { get; set; }
        public TeamAgent teamAgent { get; set; }
        public string status { get; set; }
        public string name { get; set; }
    }

    public class ListTeamResponseObject
    {
        public string responseId { get; set; }
        public string protocolVersion { get; set; }
        public string requestUrl { get; set; }
        public List<Team> teams { get; set; }
        public string userLocale { get; set; }
        public List<object> teamAddresses { get; set; }
        public int resultCode { get; set; }
        public List<object> developers { get; set; }
        public List<object> devices { get; set; }
        public object agentChangeInitiated { get; set; }
        public DateTime creationTimestamp { get; set; }
    }
}
