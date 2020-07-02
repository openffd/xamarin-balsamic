using System.Collections.Generic;

namespace Natukaship
{
    public class TrustedPhoneNumber
    {
        public string numberWithDialCode { get; set; }
        public string pushMode { get; set; }
        public string obfuscatedNumber { get; set; }
        public int id { get; set; }
    }

    public class SecurityCode
    {
        public int length { get; set; }
        public string code { get; set; }
        public bool tooManyCodesSent { get; set; }
        public bool tooManyCodesValidated { get; set; }
        public bool securityCodeLocked { get; set; }
        public bool securityCodeCooldown { get; set; }
        public bool valid { get; set; }
    }

    public class PhoneNumber
    {
        public string numberWithDialCode { get; set; }
        public string pushMode { get; set; }
        public string obfuscatedNumber { get; set; }
        public int id { get; set; }
    }

    public class ServiceError
    {
        public string message { get; set; }
        public string code{ get; set; }
    }

    public class ValidationError
    {
    }

    public class VerifyPhoneResponseObject
    {
        public List<TrustedPhoneNumber> trustedPhoneNumbers { get; set; }
        public List<ServiceError> serviceErrors { get; set; }
        public List<ValidationError> validationErrors { get; set; }
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public PhoneNumber phoneNumber { get; set; }
        public SecurityCode securityCode { get; set; }
        public string authenticationType { get; set; }
        public string mode { get; set; }
        public string type { get; set; }
        public string recoveryUrl { get; set; }
        public string cantUsePhoneNumberUrl { get; set; }
        public string recoveryWebUrl { get; set; }
        public string repairPhoneNumberUrl { get; set; }
        public string repairPhoneNumberWebUrl { get; set; }
        public string aboutTwoFactorAuthenticationUrl { get; set; }
        public bool autoVerified { get; set; }
        public bool showAutoVerificationUI { get; set; }
        public TrustedPhoneNumber trustedPhoneNumber { get; set; }
        public bool hsa2Account { get; set; }
        public bool restrictedAccount { get; set; }
        public bool supportsRecovery { get; set; }
        public bool managedAccount { get; set; }
    }
}
