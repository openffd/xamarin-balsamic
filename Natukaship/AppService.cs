using System;
namespace Natukaship
{
    public class AppService
    {
        public string ServiceId { get; set; }
        public string ServiceUri { get; set; }
        public bool Value { get; set; }
    }

    public class HealthKitService : AppService
    {
        public HealthKitService()
        {
            ServiceId = "HK421J6T7P";
            ServiceUri = "account/ios/identifiers/updateService.action";
            Value = false;
        }
    }

    public class PushNotificationService : AppService
    {
        public PushNotificationService()
        {
            ServiceId = "push";
            ServiceUri = "account/ios/identifiers/updatePushService.action";
            Value = false;
        }
    }

    public class SiriKitService : AppService
    {
        public SiriKitService()
        {
            ServiceId = "SI015DKUHP";
            ServiceUri = "account/ios/identifiers/updateService.action";
            Value = false;
        }
    }
}
