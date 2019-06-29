#if !UNITY_EDITOR && UNITY_IOS

using System.Runtime.InteropServices;

namespace Genesis.GameClient
{
	public class LocalNotificationNativeCaller
	{
		[DllImport("__Internal")]
		public static extern void LocalNotification_Register(string dateStr, string alertBody, string key);

        [DllImport("__Internal")]
		public static extern void LocalNotification_Cancel();
	}
}

#endif
