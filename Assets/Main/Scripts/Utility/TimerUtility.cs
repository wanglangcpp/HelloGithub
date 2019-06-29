using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 计时器工具。
    /// </summary>
    public static class TimerUtility
    {
        /// <summary>
        /// 等待固定帧数执行回调。
        /// </summary>
        /// <param name="frameCount">帧数。</param>
        /// <param name="callback">回调函数。</param>
        /// <param name="userData">用户数据。</param>
        /// <returns>等待辅助器。</returns>
        public static ITimerUtilityHelper WaitFrames(int frameCount, GameFrameworkAction<object> callback, object userData = null)
        {
            GameObject obj = GameObject.Find("Timer Utility Wait Frames Helper");
            if (null != obj)
                return obj.GetComponent<TimerUtilityWaitFramesHelper>();
            return new GameObject("Timer Utility Wait Frames Helper").AddComponent<TimerUtilityWaitFramesHelper>().Init(frameCount, callback, userData);
        }

        /// <summary>
        /// 等待到当前帧结束执行回调。
        /// </summary>
        /// <param name="callback">回调函数。</param>
        /// <param name="userData">用户数据。</param>
        /// <returns>等待辅助器。</returns>
        public static ITimerUtilityHelper WaitEndOfFrame(GameFrameworkAction<object> callback, object userData = null)
        {
            GameObject obj = GameObject.Find("Timer Utility Wait End Of Frame Helper");
            if (null != obj)
                return obj.GetComponent<TimerUtilityWaitEndOfFrameHelper>();
            return new GameObject("Timer Utility Wait End Of Frame Helper").AddComponent<TimerUtilityWaitEndOfFrameHelper>().Init(callback, userData);
        }

        /// <summary>
        /// 等待特定秒数。
        /// </summary>
        /// <param name="seconds">描述。</param>
        /// <param name="callback">回调函数。</param>
        /// <param name="userData">用户数据。</param>
        /// <returns>等待辅助器。</returns>
        public static ITimerUtilityHelper WaitSeconds(float seconds, GameFrameworkAction<object> callback, object userData = null)
        {
            GameObject obj = GameObject.Find("Timer Utility Wait Seconds Helper");
            if (null != obj)
                return obj.GetComponent<TimerUtilityWaitSecondsHelper>();
            return new GameObject("Timer Utility Wait Seconds Helper").AddComponent<TimerUtilityWaitSecondsHelper>().Init(seconds, callback, userData);
        }
    }
}
