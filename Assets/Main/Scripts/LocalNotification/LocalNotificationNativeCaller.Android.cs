#if !UNITY_EDITOR && UNITY_ANDROID

using System.Runtime.InteropServices;
using UnityEngine;

namespace Genesis.GameClient
{
	public class LocalNotificationNativeCaller
	{
		public static void LocalNotification_Register(string dateStr, string alertBody, string key)
        {
            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                activityContext.Call("registerNotification", alertBody, dateStr);
            }
        }

		public static void LocalNotification_Cancel()
        {
            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                activityContext.Call("cancelNotification");
            }
        }
	}
}

#endif
