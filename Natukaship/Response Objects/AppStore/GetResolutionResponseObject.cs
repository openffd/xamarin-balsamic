using System.Collections.Generic;

namespace Natukaship
{
    public class ReplyConstraints
    {
        public int minLength { get; set; }
        public int maxLength { get; set; }
    }

    public class QcRejectionReason
    {
        public string section { get; set; }
        public string description { get; set; }
    }

    public class Message
    {
        public string from { get; set; }
        public long date { get; set; }
        public string body { get; set; }
        public bool appleMsg { get; set; }
        public List<object> tokens { get; set; }
        public bool hasObjectionableContent { get; set; }
        public List<QcRejectionReason> qcRejectionReason { get; set; }
    }

    public class Thread
    {
        public string id { get; set; }
        public string versionId { get; set; }
        public string version { get; set; }
        public List<Message> messages { get; set; }
        public bool active { get; set; }
        public bool canDeveloperAddNote { get; set; }
        public bool metadataRejected { get; set; }
        public bool binaryRejected { get; set; }
    }

    public class AppNotes
    {
        public List<Thread> threads { get; set; }
    }

    public class BetaNotes
    {
        public List<object> threads { get; set; }
    }

    public class AppMessages
    {
        public List<object> threads { get; set; }
    }

    public class ResolutionData
    {
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public object value { get; set; }
        public ReplyConstraints replyConstraints { get; set; }
        public AppNotes appNotes { get; set; }
        public BetaNotes betaNotes { get; set; }
        public AppMessages appMessages { get; set; }
    }

    public class GetResolutionResponseObject
    {
        public ResolutionData data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
