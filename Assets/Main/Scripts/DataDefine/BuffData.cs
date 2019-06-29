using GameFramework;

namespace Genesis.GameClient
{
    public class BuffData
    {
        private DRBuff m_DTBuff;

        public DRBuff DRBuff
        {
            get
            {
                return m_DTBuff;
            }
        }

        public long SerialId
        {
            get;
            private set;
        }

        public EntityData OwnerData
        {
            get;
            private set;
        }

        public bool HasOwner
        {
            get
            {
                return OwnerData != null;
            }
        }

        public float StartTime
        {
            get;
            private set;
        }

        private float m_Duration;

        public float Duration
        {
            get
            {
                return m_Duration;
            }
        }

        public float EndTime
        {
            get
            {
                return StartTime + Duration;
            }
        }

        public int BuffId
        {
            get
            {
                return m_DTBuff.Id;
            }
        }

        public BuffType BuffType
        {
            get
            {
                return (BuffType)(m_DTBuff.BuffType);
            }
        }

        public int ExcludingGroup
        {
            get
            {
                return m_DTBuff.ExcludingGroup;
            }
        }

        public int ExcludingLevel
        {
            get
            {
                return m_DTBuff.ExcludingLevel;
            }
        }

        public float HeartBeat
        {
            get
            {
                return m_DTBuff.HeartBeat;
            }
        }

        private float[] m_Params;

        public float[] Params
        {
            get
            {
                return m_Params;
            }
        }

        public int DefaultEffectId
        {
            get
            {
                return m_DTBuff.DefaultEffectId;
            }
        }

        public int BlendEffectId
        {
            get
            {
                return m_DTBuff.BlendEffectId;
            }
        }

        /// <summary>
        /// 记录上次心跳的时间。
        /// </summary>
        public float LastHeartBeatTime
        {
            get;
            set;
        }

        public int[] BuffEffectEntityIds
        {
            get;
            set;
        }

        public int CurrentBuffEffectId
        {
            get;
            set;
        }

        /// <summary>
        /// 是否传递。
        /// </summary>
        public bool TransferOnHeroSwitch
        {
            get
            {
                return m_DTBuff.TransferOnHeroSwitch;
            }
        }

        /// <summary>
        /// 是否在进入假死状态时保留。
        /// </summary>
        public bool KeepOnFakeDeath
        {
            get
            {
                return m_DTBuff.KeepOnFakeDeath;
            }
        }

        /// <summary>
        /// 变色编号。
        /// </summary>
        public int ColorChangeId
        {
            get
            {
                return m_DTBuff.ColorChangeId;
            }
        }

        /// <summary>
        /// 元素编号。
        /// </summary>
        public int ElementId
        {
            get
            {
                return m_DTBuff.ElementId;
            }
        }

        /// <summary>
        /// 空中处理。
        /// </summary>
        public GoToAirBuffActionType GoToAir
        {
            get
            {
                return (GoToAirBuffActionType)m_DTBuff.GoToAir;
            }
        }

        /// <summary>
        /// 增益(1)或减益(-1)。
        /// </summary>
        public int GoodOrBad
        {
            get
            {
                return m_DTBuff.GoodOrBad;
            }
        }

        public object UserData
        {
            get;
            private set;
        }

        public BuffData(DRBuff drBuff, EntityData ownerData, long buffSerialId, float startTime, int colorChangeId, object userData = null)
        {
            if (drBuff == null)
            {
                Log.Fatal("DRBuff is invalid");
                return;
            }

            m_DTBuff = drBuff;
            OwnerData = ownerData;
            SerialId = buffSerialId;
            StartTime = startTime;
            LastHeartBeatTime = startTime;
            BuffEffectEntityIds = new int[Constant.MaxCharacterBuffEffectCount];
            UserData = userData;
            m_Duration = m_DTBuff.Duration;

            m_Params = new float[drBuff.BuffParams.Length];
            System.Array.Copy(drBuff.BuffParams, m_Params, m_Params.Length);
        }

        public void IncreaseDuration(float durationDelta)
        {
            m_Duration += durationDelta;
        }
    }
}
