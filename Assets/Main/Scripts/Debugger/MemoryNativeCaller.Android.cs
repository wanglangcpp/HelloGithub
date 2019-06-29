#if !UNITY_EDITOR && UNITY_ANDROID

namespace Genesis.GameClient
{
    public class MemoryNativeCaller
    {
        public long UsedMemory
        {
            get
            {
                return 0;
            }
        }

        public long FreeMemory
        {
            get
            {
                return 0;
            }
        }
    }
}

#endif
