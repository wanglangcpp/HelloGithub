using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class UIAnimation : MonoBehaviour
    {
        [SerializeField]
        private List<UIFormAnimation> m_OpenAnimations = null;

        [SerializeField]
        private List<UIFormAnimation> m_CloseAnimations = null;

        private float m_OpenAnimationDuration = 0f;
        private float m_CloseAnimationDuration = 0f;

        public void AddOpenAnimation(Animation animation, bool isReverse = false)
        {
            m_OpenAnimations.Add(new UIFormAnimation(animation, isReverse));
        }

        public void AddCloseAnimation(Animation animation, bool isReverse = false)
        {
            m_CloseAnimations.Add(new UIFormAnimation(animation, isReverse));
        }

        public void PlayOpenAnimations(GameFrameworkAction<object> onComplete = null)
        {
            List<Animation> listAnim = new List<Animation>();
            for (int i = 0; i < m_OpenAnimations.Count; i++)
            {
                Animation animation = m_OpenAnimations[i].Animation;
                if (animation == null)
                {
                    continue;
                }

                if (animation.clip == null)
                {
                    Log.Warning("Animation clip is invalid.");
                    continue;
                }

                var animationState = animation[animation.clip.name];
                if (m_OpenAnimations[i].IsReverse)
                {
                    animationState.time = animation.clip.length;
                    animationState.speed = -1f;
                }
                else
                {
                    animationState.time = 0f;
                    animationState.speed = 1f;
                }

                listAnim.Add(animation);
            }

            StartCoroutine(AnimationUtility.PlayAnimations(listAnim, onComplete));
        }

        public void SkipToOpenAnimationsEnd()
        {
            for (int i = 0; i < m_OpenAnimations.Count; i++)
            {
                Animation animation = m_OpenAnimations[i].Animation;
                if (animation == null)
                {
                    continue;
                }

                var animationState = animation[animation.clip.name];
                float sampleTime = m_OpenAnimations[i].IsReverse ? 0f : animation.clip.length;
                animationState.clip.SampleAnimation(animation.gameObject, sampleTime);
            }
        }

        public void PlayCloseAnimations(GameFrameworkAction<object> onComplete = null)
        {
            List<Animation> listAnim = new List<Animation>();
            for (int i = 0; i < m_CloseAnimations.Count; i++)
            {
                Animation animation = m_CloseAnimations[i].Animation;
                if (animation == null)
                {
                    continue;
                }

                var animationState = animation[animation.clip.name];
                if (m_CloseAnimations[i].IsReverse)
                {
                    animationState.time = animation.clip.length;
                    animationState.speed = -1f;
                }
                else
                {
                    animationState.time = 0f;
                    animationState.speed = 1f;
                }
                listAnim.Add(animation);
            }
            StartCoroutine(AnimationUtility.PlayAnimations(listAnim, onComplete));
        }

        private void Start()
        {
            CalcOpenAnimationDuration();
            CalcCloseAnimationDuration();
        }

        private void CalcOpenAnimationDuration()
        {
            for (int i = 0; i < m_OpenAnimations.Count; i++)
            {
                Animation animation = m_OpenAnimations[i].Animation;
                if (animation == null)
                {
                    continue;
                }

                animation.playAutomatically = false;
                animation.cullingType = AnimationCullingType.AlwaysAnimate;
                if (animation.clip.length > m_OpenAnimationDuration)
                {
                    m_OpenAnimationDuration = animation.clip.length;
                }
            }
        }

        private void CalcCloseAnimationDuration()
        {
            for (int i = 0; i < m_CloseAnimations.Count; i++)
            {
                Animation animation = m_CloseAnimations[i].Animation;
                if (animation == null)
                {
                    continue;
                }

                animation.playAutomatically = false;
                animation.cullingType = AnimationCullingType.AlwaysAnimate;
                if (animation.clip.length > m_CloseAnimationDuration)
                {
                    m_CloseAnimationDuration = animation.clip.length;
                }
            }
        }

        [Serializable]
        private class UIFormAnimation
        {
            [SerializeField]
            private Animation m_Animation = null;

            [SerializeField]
            private bool m_IsReverse = false;

            public UIFormAnimation()
            {

            }

            public UIFormAnimation(Animation animation, bool isReverse)
            {
                m_Animation = animation;
                m_IsReverse = isReverse;
            }

            public Animation Animation
            {
                get
                {
                    return m_Animation;
                }
            }

            public bool IsReverse
            {
                get
                {
                    return m_IsReverse;
                }
            }
        }
    }
}
