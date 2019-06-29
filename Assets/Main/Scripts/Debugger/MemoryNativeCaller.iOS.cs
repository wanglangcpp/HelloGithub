#if !UNITY_EDITOR && UNITY_IOS

using System.Runtime.InteropServices;

namespace Genesis.GameClient
{
    public class MemoryNativeCaller
    {
        [DllImport("__Internal")]
        private static extern long GetUsedMemory();
        public long UsedMemory
        {
            get
            {
                return GetUsedMemory();
            }
        }

        [DllImport("__Internal")]
        private static extern long GetFreeMemory();
        public long FreeMemory
        {
            get
            {
                return GetFreeMemory();
            }
        }
    }
}

#endif
