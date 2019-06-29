using System;
using System.Reflection;
using UnityEngine;

namespace Genesis.GameClient
{
#if UNITY_ANDROID
    public class AndroidCallAPI
    {
        //#if UNITY_ANDROID
        private readonly string FuncName = "onReceive";


        public void CallAndroidFunc(string datas = null)
        {
#if !UNITY_EDITOR
            if (!SDKManager.isDevelopMode) {
                using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        if (datas != null)
                        {
                            jo.Call(FuncName, datas);
                        }
                        else
                        {
                            jo.Call(FuncName);
                        }
                    }
                }
            }
#endif
        }


        //[Obsolete]
        //public void OnReceiveAndroidDatas(string datas = null)
        //{
        //    SDKCallbackBase baseCallback = JsonUtility.FromJson<SDKCallbackBase>(datas);
        //    Type t = typeof(SDKManager.SdkManagerHelper);
        //    MethodInfo mt = t.GetMethod(baseCallback.CallBackMethodName);
        //    if (mt != null)
        //    {
        //        mt.Invoke(SDKManager.Instance.helper, new String[] { datas });
        //    }
        //    else
        //    {
        //        Debug.LogError("OnReceiveAndroidDatas.MethodInfo" + baseCallback.CallBackMethodName + " is null");
        //    }
        //}
        //#endif
    }
#endif
        }

