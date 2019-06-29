using GameFramework.ObjectPool;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public abstract class BaseBubble
    {
        [SerializeField]
        private TargetableObject m_Owner = null;

        protected Bubble m_Bubble = null;

        private string m_Content = string.Empty;

        private float m_CurrentTime = 0;

        public float KeepTime
        {
            get;
            set;
        }

        public TargetableObject Owner
        {
            get
            {
                return m_Owner;
            }
            set
            {
                m_Owner = value;
                m_Bubble.Owner = value;
            }
        }

        public void Init(Bubble bubble)
        {
            m_Bubble = bubble;
        }

        public void DestroyBubble(IObjectPool<BubbleObject> bubbleObjects)
        {
            Owner = null;
            if (m_Bubble != null && m_Bubble.gameObject != null)
            {
                m_Bubble.gameObject.SetActive(false);
                bubbleObjects.Unspawn(m_Bubble);
                m_Bubble = null;
            }
        }

        public virtual void OnUpdate()
        {
            if (IsAvailable)
            {
                return;
            }
            m_CurrentTime += Time.deltaTime;
            if (m_CurrentTime >= StartShowTime)
            {
                IsAvailable = true;
                m_Bubble.gameObject.SetActive(true);
                StartTime = Time.time;
                m_Bubble.SetContent(GameEntry.Localization.GetString(m_Content));
            }
        }

        public float StartTime
        {
            get
            {
                return m_Bubble.StartTime;
            }
            set
            {
                m_Bubble.StartTime = value;
            }
        }

        public float StartShowTime
        {
            get;
            set;
        }

        public bool IsAvailable
        {
            get;
            set;
        }

        public float Height
        {
            get
            {
                return m_Bubble.Height;
            }
            set
            {
                m_Bubble.Height = value;
            }
        }

        public virtual void RefreshBubble(TargetableObject targetableObject, string content, float startTime, float keepTime)
        {
            if (m_Bubble != null)
            {
                Owner = targetableObject;
                Height = targetableObject.ModelHeight * m_Bubble.HeightFactor;
                StartShowTime = startTime;
                KeepTime = keepTime;
                m_Content = content;
                m_CurrentTime = 0;
                IsAvailable = false;
                m_Bubble.RefreshPosition();
            }
        }
    }
}
