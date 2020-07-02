using System.Collections.Generic;

namespace Natukaship
{
    public class AppVersionStatesHistory
    {
        public string stateKey { get; set; }
        public string userName { get; set; }
        public string userEmail { get; set; }
        public long date { get; set; }
    }

    public class AppVersionHistory
    {
        public string versionString { get; set; }
        public int versionId { get; set; }
        public List<AppVersionStatesHistory> items { get; set; }

        public Application application { get; set; }

        public List<AppVersionStatesHistory> Items()
        {
            if (items == null || items.Count < 1)
                items = FetchItems();

            return items;
        }

        public List<AppVersionStatesHistory> FetchItems()
        {
            var itms = Globals.TunesClient.VersionStatesHistory(application.appleId, versionId).items;

            return itms;
        }
    }

    public class VersionsHistory
    {
        public List<string> sectionErrorKeys { get; set; }
        public List<string> sectionInfoKeys { get; set; }
        public List<string> sectionWarningKeys { get; set; }
        public object value { get; set; }
        public List<AppVersionHistory> versions { get; set; }
    }

    public class VersionsHistoryResponseObject
    {
        public VersionsHistory data { get; set; }
        public Messages messages { get; set; }
        public string statusCode { get; set; }
    }
}
