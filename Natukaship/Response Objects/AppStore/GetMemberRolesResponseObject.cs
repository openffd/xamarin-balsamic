using System.Collections.Generic;

namespace Natukaship
{
    public class MemberRolesData
    {
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public object value { get; set; }
        public UserMember user { get; set; }
        public List<UserRole> roles { get; set; }
        public Actions actions { get; set; }
        public List<string> providerFeatures { get; set; }
        public List<string> appFeatures { get; set; }
    }

    public class GetMemberRolesResponseObject
    {
        public MemberRolesData data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
