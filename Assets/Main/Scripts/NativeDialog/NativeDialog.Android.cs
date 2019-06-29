#if !UNITY_EDITOR && UNITY_ANDROID

using UnityEngine;

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

                using (AndroidJavaClass nativeClass = new AndroidJavaClass(Application.bundleIdentifier + ".NativeDialog"))
                {
                    nativeClass.CallStatic("open", mode, title, message, pauseGame, confirmText, cancelText, otherText, confirmCallbackId, cancelCallbackId, otherCallbackId, userData);
                }
            }
        }
    }
}

#endif
