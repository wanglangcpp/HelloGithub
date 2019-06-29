using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 序列图动画组件。扩充 NGUI 原有的相似组件。
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent(typeof(UISprite))]
    public class UISpriteAnimationEx : UISpriteAnimation
    {
        /// <summary>
        /// 播放模式枚举。
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// 前向。
            /// </summary>
            Forward,

            /// <summary>
            /// 后向。
            /// </summary>
            Backward,

            /// <summary>
            /// 来回。
            /// </summary>
            Pingpong,
        }

        [HideInInspector]
        [SerializeField]
        private Mode m_PlayMode = Mode.Forward;

        /// <summary>
        /// 播放模式。
        /// </summary>
        public Mode PlayMode
        {
            get
            {
                return m_PlayMode;
            }

            set
            {
                m_PlayMode = value;
            }
        }

        private bool m_PingpongIsBackward = false;

        /// <summary>
        /// 将动画重置到相应播放模式的起始帧，并激活之。
        /// </summary>
        public void Reset()
        {
            if (m_PlayMode == Mode.Backward)
            {
                ResetToEnd();
            }
            else
            {
                ResetToBeginning();
            }
        }

        /// <summary>
        /// 将动画重置到末尾帧，并激活之。
        /// </summary>
        public void ResetToEnd()
        {
            mActive = true;
            frameIndex = mSpriteNames.Count - 1;

            if (mSprite != null && mSpriteNames.Count > 0)
            {
                mSprite.spriteName = mSpriteNames[frameIndex];
                if (mSnap) mSprite.MakePixelPerfect();
            }
        }

        protected override void Update()
        {
            switch (PlayMode)
            {
                case Mode.Backward:
                    UpdateBackward();
                    break;
                case Mode.Pingpong:
                    UpdatePingpong();
                    break;
                default:
                    UpdateForward();
                    break;
            }
        }

        private void UpdateForward()
        {
            base.Update();
        }

        private void UpdateBackward()
        {
            if (!CanUpdate) return;

            mDelta += RealTime.deltaTime;
            float rate = 1f / mFPS;

            if (rate >= mDelta) return;

            mDelta = (rate > 0f) ? mDelta - rate : 0f;

            if (--frameIndex < 0)
            {
                frameIndex = mSpriteNames.Count - 1;
                mActive = mLoop;
            }

            UpdateCommon();
        }

        private void UpdatePingpong()
        {
            if (!CanUpdate) return;

            mDelta += RealTime.deltaTime;
            float rate = 1f / mFPS;

            if (rate >= mDelta) return;

            mDelta = (rate > 0f) ? mDelta - rate : 0f;

            if (!m_PingpongIsBackward)
            {
                if (++frameIndex >= mSpriteNames.Count)
                {
                    m_PingpongIsBackward = true;
                    frameIndex = mSpriteNames.Count - 2;
                }
            }
            else
            {
                if (--frameIndex < 0)
                {
                    m_PingpongIsBackward = false;

                    if (mLoop)
                    {
                        frameIndex = 1;
                        mActive = true;
                    }
                    else
                    {
                        frameIndex = 0;
                        mActive = false;
                    }
                }
            }

            UpdateCommon();
        }

        private void UpdateCommon()
        {
            if (mActive)
            {
                mSprite.spriteName = mSpriteNames[frameIndex];
                if (mSnap) mSprite.MakePixelPerfect();
            }
        }

        private bool CanUpdate
        {
            get
            {
                return mActive && mSpriteNames.Count > 1 && Application.isPlaying && mFPS > 0;
            }
        }
    }
}
