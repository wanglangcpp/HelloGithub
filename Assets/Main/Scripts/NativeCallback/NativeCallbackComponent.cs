using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 本地代码回调组件
    /// </summary>
    public class NativeCallbackComponent : MonoBehaviour
    {
        private Dictionary<int, GameFrameworkAction> m_PreregisteredCallbacks = new Dictionary<int, GameFrameworkAction>();

        private static int s_CurrentPreregisteredCallbackId = 0;

        /// <summary>
        /// 增加预注册的回调函数。
        /// </summary>
        /// <param name="callback">回调函数。</param>
        /// <returns>回调函数编号。</returns>
        public int AddPreregisteredCallback(GameFrameworkAction callback)
        {
            m_PreregisteredCallbacks.Add(s_CurrentPreregisteredCallbackId, callback);
            return s_CurrentPreregisteredCallbackId++;
        }

        /// <summary>
        /// 内存低回调。仅供从 Native 代码调用。
        /// </summary>
        /// <param name="msg">本地消息字串。</param>
        public void OnLowMemory(string msg)
        {
            Log.Info("Received native OnLowMemory message.");
            GameEntry.ObjectPool.ReleaseAllUnused();
            GameEntry.Resource.ForceUnloadUnusedAssets(true);
            SendReportPacket();
        }

        /// <summary>
        /// 本地对话框关闭回调。仅供从 Native 代码调用。
        /// </summary>
        /// <param name="msg">本地消息字串。</param>
        public void OnNativeDialogClose(string msg)
        {
            CallPreregisteredCallback(int.Parse(msg));
        }

        /// <summary>
        /// 预定义回调的调用回调。仅供从 Native 代码调用。
        /// </summary>
        /// <param name="msg">本地消息字串。</param>
        public void OnAskedToCallPreregisteredCallback(string msg)
        {
            CallPreregisteredCallback(int.Parse(msg));
        }

        /// <summary>
        /// 尝试从暂停状态恢复游戏。仅供从 Native 代码调用。
        /// </summary>
        /// <param name="msg"></param>
        public void OnTryToResumeGame(string msg)
        {
            GameEntry.TimeScale.ResumeGame();
        }

        private void CallPreregisteredCallback(int id)
        {
            if (!m_PreregisteredCallbacks.ContainsKey(id))
            {
                return;
            }

            var callback = m_PreregisteredCallbacks[id];
            m_PreregisteredCallbacks.Remove(id);
            if (callback != null)
            {
                callback();
            }
        }

        private static void SendReportPacket()
        {
            if (GameEntry.Data.Player.Id <= 0)
            {
                // 未登录。
                return;
            }

            CLLogReport packet = new CLLogReport()
            {
                Type = (int)LogReportType.LowMemory,
            };

            if (PacketUtility.FillLogReport(packet))
            {
                GameEntry.Network.Send(packet);
            }
        }
    }
}
