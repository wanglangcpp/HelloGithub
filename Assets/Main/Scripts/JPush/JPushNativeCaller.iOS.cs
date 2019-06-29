#if !UNITY_EDITOR && UNITY_IOS

using System.Runtime.InteropServices;

namespace Genesis.GameClient
{
	public class JPushNativeCaller
	{
		[DllImport("__Internal")]
		private static extern void JPush_SetAlias(string alias);

        public static void SetAlias(string alias)
        {
            JPush_SetAlias(alias);
        }
	}
}

#endif
