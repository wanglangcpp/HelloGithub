#if !UNITY_EDITOR && UNITY_STANDALONE

namespace Genesis.GameClient
{
    public static partial class NativeDialog
    {
        private class NativeCaller : INativeCaller
        {
            public void Open(int mode, string title, string message, bool pauseGame,
                string confirmText, string cancelText, string otherText,
                int confirmCallbackId, int cancelCallbackId, int otherCallbackId,
                string userData)
            {

            }
        }
    }
}

#endif
