using System;

namespace Genesis.GameClient
{
    [Serializable]
    public class VersionData
    {
        public bool ForceGameUpdate = false;
        public string LatestGameVersion = string.Empty;
        public int InternalApplicationVersion = 0;
        public int InternalResourceVersion = 0;
        public int VersionListLength = 0;
        public int VersionListHashCode = 0;
        public int VersionListZipLength = 0;
        public int VersionListZipHashCode = 0;
    }
}
