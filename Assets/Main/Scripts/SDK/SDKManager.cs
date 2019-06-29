using UnityEngine;
using Genesis.GameClient;
using System;
using GameFramework;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Text;
using System.Collections;

namespace Genesis.GameClient
{
    public class SDKManager
    {

        public const bool isDevelopMode = false;
#if UNITY_EDITOR
        private const bool isDevelopDebug = true;
#else
        private const bool isDevelopDebug = true;
#endif

        private static SDKManager instance;
        public static SDKManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SDKManager();
                }
                return instance;
            }

        }
        public static void CreateInstance()
        {
            if (instance == null)
            {
                instance = new SDKManager();
            }
        }
        public static void Debug(string msg)
        {
            if (isDevelopDebug)
            {
                UnityEngine.Debug.Log(msg);
            }
        }

        private SDKManager()
        {
            Log.Debug("SDKManager create");
            GameObject obj = new GameObject("[SdkManager]");
            UnityEngine.Object.DontDestroyOnLoad(obj);
            helper = obj.AddComponent<SdkManagerHelper>();
            HasConfig = false;
            HasSDK = false;
            isSDKLogin = false;
            SDKData = new PlatformData();
            TalkingData = new TalkingDataMgr();
        }

        public TalkingDataMgr TalkingData;

        public SdkManagerHelper helper = null;
        public bool isSDKLogin = false;
        public PlatformData SDKData;
        private bool isWaiting;
        public static bool HasSDK = false;
        public static bool HasExitWindow = false;

        private static bool mHasConfig = false;
        public static bool HasConfig
        {
            get { return mHasConfig; }
            private set { mHasConfig = value; }
        }

#if UNITY_ANDROID
        private AndroidCallAPI andoridCall = new AndroidCallAPI();
#elif UNITY_IOS || UNITY_IPHONE
        // DllImport这个方法相当于是告诉Unity，有一个unityToIOS函数在外部会实现。
        // 使用这个方法必须要导入System.Runtime.InteropServices;
        [DllImport("__Internal")]
        private static extern void unityToIOS(string str);
#endif
        //游戏业务 例如登录，退出
        public class SdkManagerHelper : MonoBehaviour
        {
#region Use by Client
            public void LoginPlatformServer()
            {
                CLPlatformLoginServer requestData = new CLPlatformLoginServer();
                requestData.ChannelCode = Instance.SDKData.ChannelCode;
                requestData.AccountName = Instance.SDKData.AccountName;
                requestData.Token = Instance.SDKData.Token;
                requestData.UserId = Convert.ToInt64(Instance.SDKData.Uid);
                //requestData.UserId = 0;
                GameEntry.Network.Send(requestData);
                Log.Debug("SDKManager.RequestLoginPlatformServer");
            }

            private void Reset()
            {
                HasConfig = false;
                HasSDK = false;
                Instance.isSDKLogin = false;
                Instance.SDKData = new PlatformData();
                Instance.TalkingData = new TalkingDataMgr();
            }
            public void Logout()
            {
                if (Instance.isSDKLogin == false)
                {
                    return;
                }
                SDKBoxData data = new SDKBoxData();
                data.Behavior = "logout";
                Send(data);
            }
            public void ExitGame()
            {
                SDKBoxData data = new SDKBoxData();
                data.Behavior = "exitGameBySDK";
                Send(data);
            }
            //显示游戏正在登陆
            public void StartWaiting(string key)
            {
                if (!Instance.isWaiting)
                {
                    Instance.isWaiting = true;
                    GameEntry.Waiting.StartWaiting(WaitingType.Default, key);
                }
            }

            public void StopWaiting(string key)
            {
                if (Instance.isWaiting)
                {
                    Instance.isWaiting = false;
                    GameEntry.Waiting.StopWaiting(WaitingType.Default, key);
                }
            }

#endregion

#region Platform Callback
            //分成两部分传过来，要不传过来的字符串太长了，会出现丢失的情况
            public void CallbackInitConfig_part1(string data)
            {
                SDKManager.Debug("SDKManager.a-u.CallbackInitConfig_part1:" + data);
                PlatformDataPart1 revData = JsonUtility.FromJson<PlatformDataPart1>(data);
                if (null == Instance.SDKData)
                    Instance.SDKData = new PlatformData();
                Instance.SDKData.SetFieldValue(revData);
                SDKManager.Debug("===============>>CallbackInitConfig_part1:" + (null == revData));
            }
            //分成两部分传过来，要不传过来的字符串太长了，会出现丢失的情况
            public void CallbackInitConfig_part2(string data)
            {
                SDKManager.Debug("SDKManager.a-u.CallbackInitConfig_part2:" + data);
                PlatformDataPart2 revData = JsonUtility.FromJson<PlatformDataPart2>(data);
                if (null == Instance.SDKData)
                    Instance.SDKData = new PlatformData();
                Instance.SDKData.SetFieldValue(revData);
                HasSDK = Instance.SDKData.HadSDK();
                HasExitWindow = Instance.SDKData.HadExitWindow();
                Instance.TalkingData.InitSDK(Instance.SDKData.ChannelCode);
                HasConfig = Instance.SDKData.HasConfig.Equals("1");
                SDKManager.Debug("===============>>CallbackInitConfig_part2:" + (null == revData));
            }

            public void SetAmount(string data)
            {
                StopWaiting("showWebLogining");
                SDKManager.Debug("SDKManager.a-u.SetAmount.data:" + data);
                PlatformData revData = JsonUtility.FromJson<PlatformData>(data);

                Instance.SDKData.AccountName = revData.AccountName;
                Instance.SDKData.Uid = revData.Uid;
                Instance.SDKData.Token = revData.Token;
                Instance.SDKData.Data = revData.Data;//修饰服务器用的
                Instance.isSDKLogin = true;
                Instance.TalkingData.SetAccount(revData.AccountName);
                bool needLogin = string.IsNullOrEmpty(Instance.SDKData.AccountName) || string.IsNullOrEmpty(Instance.SDKData.Token);
                GameEntry.Data.AddOrUpdateTempData(Constant.TempData.NeedShowAccountAgreement, needLogin);
                ProcedureLogin procedureLogin = GameEntry.Procedure.CurrentProcedure as ProcedureLogin;
                procedureLogin.ChangeLoginOK(true);
                SDKManager.Debug("SDKManager.a-u.SetAmount.data.procedureLogin:" + procedureLogin);
            }

            public void SdkLogoutCallback(string data)
            {
                if (Instance.isSDKLogin == false)
                {
                    return;
                }
                Instance.isSDKLogin = false;
                Instance.SDKData = new PlatformData();
                Instance.TalkingData = new TalkingDataMgr();
                GameEntry.Restart();
            }
            public void SdkExitCallback(string data)
            {
                //GameEntry.ExitGame();
                GameEntry.Shutdown();
                Application.Quit();
            }
#endregion

#region Send to Platform Command
            private void Send(object dataObj)
            {
                string data = Utility.Json.ToJson(dataObj);
                SDKManager.Debug("SDKManager.u-a.send:" + data);
#if UNITY_ANDROID
                instance.andoridCall.CallAndroidFunc(data);
#elif UNITY_IOS || UNITY_IPHONE
                SDKManager.unityToIOS(data);
#endif
            }

            private void InitSDK()
            {
                Log.Debug("SDKManager.u-a.InitSDK");
                SDKBoxData data = new SDKBoxData();
                data.Behavior = "initSDK";
                Send(data);
            }

            public void LoginFail(string data)
            {
                StopWaiting("showWebLogining");
                //结束转圈圈效果
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 1,
                    Title = GameEntry.Localization.GetString("UI_TEXT_PROMPT"),
                    Message = GameEntry.Localization.GetString("UI_TEXT_WEBLOGIN_FAIL_TIP"),
                    ConfirmText = GameEntry.Localization.GetString("UI_BUTTON_TEXT_RETRY"),
                    OnClickConfirm = o => { RecConnectWebLogin(); },
                });
            }

            public void InitConfig(bool isPublisher)
            {
                Reset();
                Log.Debug("SDKManager.u-a.InitConfig");
                SDKBoxData data = new SDKBoxData();
                data.Behavior = "initConfig";
                data.Data = isPublisher ? (-1 + "") : (1 + "");
                Send(data);
            }

            public void LoginSDK()
            {
                Log.Debug("SDKManager.u-a.LoginSDK");
                SDKBoxData data = new SDKBoxData();
                data.Behavior = "login";
                Send(data);
            }

            public void RecConnectWebLogin()
            {
                StartWaiting("showWebLogining");
                Log.Debug("SDKManager.u-a.RecConnectWebLogin");
                SDKBoxData data = new SDKBoxData();
                data.Behavior = "reConnectWebLogin";
                Send(data);
            }

            public void LaunchAppDetail()
            {
                SDKBoxData data = new SDKBoxData();
                data.Behavior = "LaunchAppDetail";
                data.Data = "";
                Send(data);
            }

#endregion

            #region Pay

            public void Pay(PayInfos payInfo)
            {
                if (!SDKManager.HasSDK || !SDKManager.Instance.isSDKLogin)
                {
                    Log.Debug("Pay.hasSDK:" + SDKManager.HasConfig + ",isSDKLogin:" + SDKManager.Instance.isSDKLogin);
                    return;
                }
                Log.Debug("SDKManager.u-a.Pay");
                payInfo.gameServerId = GameEntry.Data.Account.ServerData.Id.ToString();
                payInfo.gameServerName = GameEntry.Data.Account.ServerData.Name;
                payInfo.roleId = GameEntry.Data.Player.Id.ToString();
                payInfo.roleName = GameEntry.Data.Player.Name;
                payInfo.userId = GameEntry.Data.Player.Id.ToString();//目前userId与roleId一致
                payInfo.amount = 1;
                payInfo.playerLv = GameEntry.Data.Player.Level.ToString();
                payInfo.vipLv = GameEntry.Data.Player.VipLevel.ToString();
                payInfo.balance = GameEntry.Data.Player.Money.ToString();
                StartWaiting("orderID");
                StartCoroutine(RequestOrderId(payInfo));
            }

            public IEnumerator RequestOrderId(PayInfos payInfo)
            {
                OrderRequestData data = new OrderRequestData();
#if UNITY_ANDROID
                data.d = (int)Platform.Android;
#elif UNITY_IOS || UNITY_IPHONE
                data.d = (int)Platform.IOS;
#endif
                data.app = Instance.SDKData.AppCode;
                data.iId = payInfo.itemId;
                data.a = 1;
                data.CC = Instance.SDKData.ChannelCode;
                data.gsId = payInfo.gameServerId;
                data.p = payInfo.price;
                data.pp = payInfo.price;
                data.uId = payInfo.userId;//目前userId与roleId一致
                data.an = Instance.SDKData.AccountName;

                string jsonData = Utility.Json.ToJson(data);
                SDKManager.Debug("requestOrderIdJsonData:" + jsonData);
                //string enData = AESMgrTool.Encrypt(jsonData, "dlgame2017112223");
                string enData = AESMgrTool.Encrypt(jsonData, Instance.SDKData.AesKey);
                bool isNeiwang = string.IsNullOrEmpty(GameEntry.Data.GetTempData<string>(Constant.TempData.Publisher));
                string requestURL = isNeiwang ? Instance.SDKData.RequestOrderURL : Instance.SDKData.RequestOrderURLOut;
                //string rspStr = HttpPost("http://192.168.1.116:8888/game/sdk/DL/pay", enData);

                byte[] byteArray = Encoding.UTF8.GetBytes("BF=" + enData + "&");
                WWW postData = new WWW(requestURL, byteArray);
                yield return postData;
                StopWaiting("orderID");
                string ss = postData.text;
                if (postData.error != null)
                {
                    Log.Debug(postData.error);
                    Log.Debug("Request OrderId Fail");
                    GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_WEBLOGIN_FAIL_TIP") });
                }
                else if (postData.text != null && postData.text.Length > 0)
                {
                    SDKManager.Debug("sdkMgr.RequestOrderId.postData.len:" + ss.Length + "data:" + ss);
                    string deStrJson = AESMgrTool.Decrypt(postData.text, Instance.SDKData.AesKey);
                    SDKManager.Debug("deStrJson:" + deStrJson);
                    OrderRspData rspData = JsonUtility.FromJson<OrderRspData>(deStrJson);
                    if (rspData.retCode == 0 && !string.IsNullOrEmpty(rspData.data.orderId))
                    {
                        payInfo.orderId = rspData.data.orderId;
                        Instance.TalkingData.OnChargeRequest(payInfo);
                        SDKBoxData notifyPay = new SDKBoxData();
                        notifyPay.Behavior = "pay";
                        notifyPay.Data = Utility.Json.ToJson(payInfo);
                        Send(notifyPay);
                    }
                    else
                    {
                        Log.Debug("Rsponse OrderId Fail");
                        GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_WEBLOGIN_FAIL_TIP") });
                    }
                }
            }

            public void PayCallback(string data)
            {
                Debug("SDKManager.a-u.PayCallback:" + data);
                bool isPaySuccess = Convert.ToInt32(data) == 1;
                Instance.TalkingData.OnChargeResult(isPaySuccess);
            }

#endregion

#region statistics
            public void Record(string tag, string datas)
            {
                Log.Debug("SDKManager.u-a.RecordLogin");
                SDKBoxData data = new SDKBoxData();
                data.Behavior = "record";
                data.Data = tag + "|||" + datas;
                Send(data);
            }

            public void UploadData(string tag)
            {
                PlayerData playerData = GameEntry.Data.Player;
                ServerData serverData = GameEntry.Data.Account.ServerData;
                FriendsData friendsData = GameEntry.Data.Friends;
                if (GameEntry.Data.HasTempData(Constant.TempData.CreatPlayer) && tag == "Loading")
                {
                    GameEntry.Data.RemoveTempData(Constant.TempData.CreatPlayer);
                    tag = "CreatePlayer";
                }
                RecordMgr recordMgr = new RecordMgr();
                recordMgr.playerData = playerData;
                recordMgr.serverData = serverData;
                foreach (var item in friendsData.Data)
                {
                    recordMgr.Friends.Add(item.Player);
                }
                string recordData = JsonUtility.ToJson(recordMgr);
                //RecordMgr mgr = JsonUtility.FromJson<RecordMgr>(recordData);
                Record(tag, recordData);
            }
#endregion
        }

        private enum Platform : int
        {
            Android = 1,
            IOS = 2,
        }



    }
}