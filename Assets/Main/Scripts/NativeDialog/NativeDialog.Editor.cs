#if UNITY_EDITOR

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

                int dialogResult = 0;
                switch (mode)
                {
                    case 1:
                        UnityEditor.EditorUtility.DisplayDialog(title, message, confirmText);
                        dialogResult = 0;
                        break;
                    case 2:
                        dialogResult = UnityEditor.EditorUtility.DisplayDialog(title, message, confirmText, cancelText) ? 0 : 1;
                        break;
                    case 3:
                        dialogResult = UnityEditor.EditorUtility.DisplayDialogComplex(title, message, confirmText, cancelText, otherText);
                        break;
                    default:
                        throw new System.NotSupportedException(string.Format("Invalid dialog mode '{0}'.", mode.ToString()));
                }

                int callbackId = dialogResult == 0 ? confirmCallbackId : dialogResult == 1 ? cancelCallbackId : otherCallbackId;

                GameEntry.NativeCallback.OnAskedToCallPreregisteredCallback(callbackId.ToString());

                if (pauseGame)
                {
                    GameEntry.TimeScale.ResumeGame();
                }
            }
        }
    }
}

#endif
