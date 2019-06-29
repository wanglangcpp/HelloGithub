using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 霸体
    /// </summary>
    [Serializable]
    public abstract class BaseSteady
    {
        [SerializeField]
        protected float m_Steady;

        [SerializeField]
        private float m_MaxSteady;

        [SerializeField]
        private float m_SteadyRecoverSpeed;

        [SerializeField]
        protected bool m_IsSteadying = false;

        protected Character m_Owner = null;

        public BaseSteady()
        {

        }

        public bool SteadyStatus
        {
            set
            {
                m_IsSteadying = value;
                if (value)
                {
                    m_Steady = MaxSteady;
                }
                else
                {
                    m_Steady = 0;
                }
            }
        }

        public Character Owner
        {
            get
            {
                return m_Owner;
            }
            set
            {
                m_Owner = value;
            }
        }

        public virtual float Steady
        {
            set
            {
                m_Steady = value;
            }
            get
            {
                return m_Steady;
            }
        }

        public virtual float MaxSteady
        {
            set
            {
                m_MaxSteady = value;
            }
            get
            {
                return m_MaxSteady;
            }
        }

        public virtual float SteadyRecoverSpeed
        {
            set
            {
                m_SteadyRecoverSpeed = value;
            }
            get
            {
                return m_SteadyRecoverSpeed;
            }
        }

        public virtual bool IsSteadying
        {
            set
            {
                m_IsSteadying = value;
            }
            get
            {
                return m_IsSteadying;
            }
        }

        public float SteadyRatio
        {
            get
            {
                return MaxSteady > 0f ? Steady / MaxSteady : 0f;
            }
        }

        public virtual void UpdateSteady(float elapseSeconds = 0f)
        {

            if (m_Owner == null)
            {
                return;
            }

            if (MaxSteady <= 0f)
            {
                return;
            }

            if (IsSteadying)
            {
                if (Steady <= 0f)
                {
                    BreakSteady();
                }
            }
            else
            {
                Steady += SteadyRecoverSpeed * elapseSeconds;
                if (Steady >= MaxSteady)
                {
                    RecoverSteady();
                }
            }
        }

        private void RecoverSteady()
        {
            Steady = MaxSteady;
            IsSteadying = true;
            m_Owner.AddBuff(m_Owner.SteadyBuffId, m_Owner.Data, OfflineBuffPool.GetNextSerialId(), null);
            GameEntry.Event.Fire(this, new SteadyRecoveredEventArgs(m_Owner));
        }

        private void BreakSteady()
        {
            Steady = 0f;
            IsSteadying = false;
            m_Owner.RemoveBuffById(m_Owner.SteadyBuffId);
            GameEntry.Event.Fire(this, new SteadyBreakedEventArgs(m_Owner));
        }

        public virtual void AddSteady(float deltaSteady)
        {
            Steady += deltaSteady;
            if (Steady >= MaxSteady)
            {
                if (!IsSteadying)
                {
                    RecoverSteady();
                }
                else
                {
                    Steady = MaxSteady;
                }

                return;
            }
            
            if (Steady <= 0)
            {
                if (IsSteadying)
                {
                    BreakSteady();
                }
                else
                {
                    Steady = 0f;
                }
            }
        }

        public virtual void OnShow()
        {
        }

        public virtual void OnHide()
        {
        }
    }
}
