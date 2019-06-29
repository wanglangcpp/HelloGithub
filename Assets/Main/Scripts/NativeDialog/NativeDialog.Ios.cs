#if !UNITY_EDITOR && UNITY_IOS

using System.Runtime.InteropServices;

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
                if (!GameEntry.IsAvailable)
                {
                    return;
                }

                if (pauseGame)
                {
                    GameEntry.TimeScale.PauseGame();
                }

                NativeDialog_Open(mode, title, message, pauseGame, confirmText, cancelText, otherText, confirmCallbackId, cancelCallbackId, otherCallbackId, userData);
            }

            [DllImport("__Internal")]
            private extern static void NativeDialog_Open(int mode, string title, string message, bool pauseGame,
                string confirmText, string cancelText, string otherText,
                int confirmCallbackId, int cancelCallbackId, int otherCallbackId,
                string userData);
        }

    }
}

#endif
