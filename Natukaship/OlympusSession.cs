using System;
using System.Collections.Generic;

namespace Natukaship
{
    public class User
    {
        public string fullName { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailAddress { get; set; }
        public string prsId { get; set; }
    }

    public class Provider
    {
        public int providerId { get; set; }
        public string name { get; set; }
        public List<string> contentTypes { get; set; }
        public string subType { get; set; }
    }

    public class AvailableProvider
    {
        public int providerId { get; set; }
        public string name { get; set; }
        public List<string> contentTypes { get; set; }
        public string subType { get; set; }
    }

    public class Module
    {
        public string key { get; set; }
        public string name { get; set; }
        public string localizedName { get; set; }
        public string url { get; set; }
        public string iconUrl { get; set; }
        public bool down { get; set; }
        public bool visible { get; set; }
        public bool hasNotifications { get; set; }
    }

    public class HelpLink
    {
        public string key { get; set; }
        public string url { get; set; }
        public string localizedText { get; set; }
    }

    public class UserProfile
    {
        public string key { get; set; }
        public string url { get; set; }
        public string localizedText { get; set; }
    }

    public class OlympusSession
    {
        public User user { get; set; }
        public Provider provider { get; set; }
        public string theme { get; set; }
        public List<AvailableProvider> availableProviders { get; set; }
        public string backingType { get; set; }
        public List<string> backingTypes { get; set; }
        public List<string> roles { get; set; }
        public List<object> unverifiedRoles { get; set; }
        public List<string> featureFlags { get; set; }
        public bool agreeToTerms { get; set; }
        public List<string> termsSignatures { get; set; }
        public List<Module> modules { get; set; }
        public List<HelpLink> helpLinks { get; set; }
        public List<UserProfile> userProfile { get; set; }
        public object pccDto { get; set; }
        public string publicUserId { get; set; }
    }
}
