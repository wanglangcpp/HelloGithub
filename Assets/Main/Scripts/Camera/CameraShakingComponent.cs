using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 摄像机震动组件，为跟随角色的摄像机提供震动所需的数据，便于配置。
    /// </summary>
    public partial class CameraShakingComponent : MonoBehaviour
    {
        private const int ShakingConfigLayerCount = 8;

        [SerializeField]
        private CameraShakingConfig m_Config = null;

        private Vector3 m_CurrentOffset = Vector3.zero;
        private RunningConfigInfo[] m_RunningConfigs = new RunningConfigInfo[ShakingConfigLayerCount];

        private class RunningConfigInfo
        {
            public int Index { get; set; }
            public float StartTime { get; set; }
        }

        public void PerformShaking(int configIndex)
        {
            if (configIndex < 0 || configIndex >= m_Config.Count)
            {
                Log.Warning("[CameraShakingComponent PerformShaking] The requested config index {0} is invalid.", configIndex);
                return;
            }

            var newConfig = m_Config[configIndex];
            var layer = newConfig.Layer;

            if (m_RunningConfigs[layer] != null)
            {
                //Log.Debug("[CameraShakingComponent PerformShaking] Layer {0} is occupied. The requested config with index {1} won't be played", layer, configIndex);
                return;
            }

            m_RunningConfigs[layer] = new RunningConfigInfo { Index = configIndex, StartTime = Time.unscaledTime };
        }

        public Vector3 CurrentOffset
        {
            get
            {
                return m_CurrentOffset;
            }
        }

        public void Reset()
        {
            for (int i = 0; i < m_RunningConfigs.Length; ++i)
            {
                m_RunningConfigs[i] = null;
            }

            m_CurrentOffset = Vector3.zero;
        }

        #region MonoBehaviour

        private void Update()
        {
            m_CurrentOffset = Vector3.zero;
            var currentTime = Time.unscaledTime;
            for (int i = 0; i < m_RunningConfigs.Length; ++i)
            {
                var configInfo = m_RunningConfigs[i];

                if (configInfo == null)
                {
                    continue;
                }

                var config = m_Config[configInfo.Index];
                var playedTime = currentTime - configInfo.StartTime;
                if (playedTime > config.Duration)
                {
                    m_RunningConfigs[i] = null;
                    continue;
                }

                var toAdd = new Vector3(config.OffsetXCurve.Evaluate(playedTime),
                    config.OffsetYCurve.Evaluate(playedTime),
                    config.OffsetZCurve.Evaluate(playedTime));

                m_CurrentOffset += toAdd;
            }
        }

        #endregion MonoBehaviour
    }
}
