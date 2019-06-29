#if !UNITY_EDITOR && UNITY_ANDROID

using System.Runtime.InteropServices;
using UnityEngine;

namespace Genesis.GameClient
{
	public class JPushNativeCaller
	{
		public static void SetAlias(string alias)
        {
            using(AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                activityContext.Call("jPushSetAlias", alias);
            }
        }
	}
}

#endif
