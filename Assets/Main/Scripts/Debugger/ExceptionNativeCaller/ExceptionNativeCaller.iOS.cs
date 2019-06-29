#if !UNITY_EDITOR && UNITY_IOS

using System.Runtime.InteropServices;

namespace Genesis.GameClient
{
    public class ExceptionNativeCaller
    {
        [DllImport("__Internal")]
        private static extern void ExceptionNativeCaller_Trigger();

        public void Trigger()
        {
            ExceptionNativeCaller_Trigger();
        }
    }
}

#endif
