using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CameraShakingConfig : ScriptableObject
    {
        [Serializable]
        public class Config
        {
            [SerializeField]
            [Range(0, CameraShakingConfig.MaxLayerCount - 1)]
            private int m_Layer = 0;

            [SerializeField]
            private AnimationCurve m_OffsetXCurve = new AnimationCurve();

            [SerializeField]
            private AnimationCurve m_OffsetYCurve = new AnimationCurve();

            [SerializeField]
            private AnimationCurve m_OffsetZCurve = new AnimationCurve();

            public AnimationCurve OffsetXCurve
            {
                get
                {
                    return m_OffsetXCurve;
                }
            }

            public AnimationCurve OffsetYCurve
            {
                get
                {
                    return m_OffsetYCurve;
                }
            }

            public AnimationCurve OffsetZCurve
            {
                get
                {
                    return m_OffsetZCurve;
                }
            }

            public int Layer
            {
                get
                {
                    m_Layer = Mathf.Clamp(m_Layer, 0, CameraShakingConfig.MaxLayerCount);
                    return m_Layer;
                }
            }

            private static float GetDuration(AnimationCurve curve)
            {
                return curve == null ? 0f : curve.keys[curve.keys.Length - 1].time;
            }

            public float Duration
            {
                get
                {
                    return Mathf.Max(GetDuration(m_OffsetXCurve), GetDuration(m_OffsetYCurve), GetDuration(m_OffsetZCurve));
                }
            }
        }
    }
}
