using System.Collections.Generic;

namespace Natukaship
{
    public class Notifications : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class Artists : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class CurrentPassword : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class NewPassword : CommonSharedValues
    {
        public object value { get; set; }
        public int maxLength { get; set; }
        public int minLength { get; set; }
    }

    public class ConfirmPassword : CommonSharedValues
    {
        public object value { get; set; }
        public int maxLength { get; set; }
        public int minLength { get; set; }
    }

    public class IsInternalTester : CommonSharedValues
    {
        public object value { get; set; }
    }

    public class MemberEmailAddress : CommonSharedValues
    {
        public object value { get; set; }
        public int maxLength { get; set; }
        public int minLength { get; set; }
    }

    public class MemberFirstName : CommonSharedValues
    {
        public object value { get; set; }
        public int maxLength { get; set; }
        public int minLength { get; set; }
    }

    public class MemberLastName : CommonSharedValues
    {
        public object value { get; set; }
        public int maxLength { get; set; }
        public int minLength { get; set; }
    }

    public class CreateUser : UserMember
    {
        public object access { get; set; }
        public Notifications notifications { get; set; }
        public Artists artists { get; set; }
        public CurrentPassword currentPassword { get; set; }
        public NewPassword newPassword { get; set; }
        public ConfirmPassword confirmPassword { get; set; }
        public IsInternalTester isInternalTester { get; set; }
        public object testerId { get; set; }
    }

    public class ActionsCommonSharedValues
    {
        public string marketing { get; set; }
        public string reports { get; set; }
        public string legal { get; set; }
        public string customersupport { get; set; }
        public string admin { get; set; }
        public string developer { get; set; }
        public string appmanager { get; set; }
        public string finance { get; set; }
        public string sales { get; set; }
    }

    public class Actions
    {
        public ActionsCommonSharedValues manageusers { get; set; }
        public ActionsCommonSharedValues managetestusers { get; set; }
        public ActionsCommonSharedValues manageyourapplications { get; set; }
        public ActionsCommonSharedValues Salesandtrends { get; set; }
        public ActionsCommonSharedValues appanalytics { get; set; }
        public ActionsCommonSharedValues taxandbanking { get; set; }
        public ActionsCommonSharedValues contracts { get; set; }
        public ActionsCommonSharedValues paymentsandfinancialreports { get; set; }
        public ActionsCommonSharedValues marketing { get; set; }
    }

    public class NotificationTypes
    {
        public string contract { get; set; }
        public string payment { get; set; }
        public string financialReport { get; set; }
        public string appStatus { get; set; }
    }

    public class CreateMemberData
    {
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public object value { get; set; }
        public CreateUser user { get; set; }
        public List<UserRole> roles { get; set; }
        public Actions actions { get; set; }
        public List<string> countries { get; set; }
        public NotificationTypes notificationTypes { get; set; }
        public List<string> providerFeatures { get; set; }
        public List<string> appFeatures { get; set; }
    }

    public class GetCreateMemberResponseObject
    {
        public CreateMemberData data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
