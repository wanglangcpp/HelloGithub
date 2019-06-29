namespace Genesis.GameClient
{
    public static partial class NativeDialog
    {
        private static INativeCaller s_NativeCallerInstance = null;

        private static INativeCaller NativeCallerInstance
        {
            get
            {
                if (s_NativeCallerInstance == null)
                {
                    s_NativeCallerInstance = new NativeCaller();
                }

                return s_NativeCallerInstance;
            }
        }

        public static void Open(int mode = 1, string title = null, string message = null, bool pauseGame = false,
            string confirmText = null, string cancelText = null, string otherText = null,
            int confirmCallbackId = -1, int cancelCallbackId = -1, int otherCallbackId = -1,
            string userData = null)
        {
            NativeCallerInstance.Open(mode, title ?? string.Empty, message ?? string.Empty, pauseGame,
                confirmText ?? string.Empty, cancelText ?? string.Empty, otherText ?? string.Empty,
                confirmCallbackId, cancelCallbackId, otherCallbackId,
                userData ?? string.Empty);
        }

        private interface INativeCaller
        {
            void Open(int mode, string title, string message, bool pauseGame,
                string confirmText, string cancelText, string otherText,
                int confirmCallbackId, int cancelCallbackId, int otherCallbackId,
                string userData);
        }
    }
}
