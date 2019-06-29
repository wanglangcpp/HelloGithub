using System;

namespace Genesis.GameClient
{
    [Serializable]
    public class BuildInfo
    {
        public int InternalVersion = 0;
        public string GitHash = string.Empty;
        public string GitBranch = string.Empty;
        public string Publisher = string.Empty;
        public string CheckVersionUri = string.Empty;
        public string UpdateResourceUri = string.Empty;
        public string CheckServerListUri = string.Empty;
        public string LoginUri = string.Empty;
        public string IosAppUrl = string.Empty;
        public string AndroidAppUrl = string.Empty;
        public bool InnerPublisherEnabled = false;
        public string ChargeTableUrl = string.Empty;
    }
}
