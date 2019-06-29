#if !UNITY_EDITOR && UNITY_STANDALONE

using System.Runtime.InteropServices;

namespace Genesis.GameClient
{
	public class LocalNotificationNativeCaller
	{
		public static void LocalNotification_Register(string dateStr, string alertBody, string key)
        {
        }

		public static void LocalNotification_Cancel()
        {
        }
	}
}

#endif
