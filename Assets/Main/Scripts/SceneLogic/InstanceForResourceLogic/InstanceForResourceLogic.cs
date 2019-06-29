using UnityEngine;
using System.Collections;
using GameFramework;

namespace Genesis.GameClient
{
    /// <summary>
    /// 资源副本逻辑。
    /// </summary>
    public partial class InstanceForResourceLogic : BaseSinglePlayerInstanceLogic
    {
        public override InstanceLogicType Type
        {
            get
            {
                return InstanceLogicType.ForResource;
            }
        }

        private DRInstanceForResource m_DRInstance;

        protected override DRInstance DRInstance
        {
            get
            {
                return m_DRInstance;
            }
        }

        private int m_RewardLevel = 0;

        public override void Init(int instanceOrSceneId, object userData)
        {
            base.Init(instanceOrSceneId, userData);
            m_DRInstance = GetInstanceDataRow<DRInstanceForResource>(m_InstanceId);
            InitInstanceDataBefore(m_DRInstance);
        }

        public override void RequestStartInstance()
        {
            base.RequestStartInstance();
            InitNpcDataTable(m_DRInstance);
            InitBuildingDataTable(m_DRInstance);
            InitTimer(m_DRInstance);
        }

        public void SetRewardLevel(int rewardLevel)
        {
            m_RewardLevel = rewardLevel;
        }

        protected override void OnInstanceTimeOut()
        {
            SetInstanceSuccess(InstanceSuccessReason.TimeOut);
        }

        protected override void PrepareSuccessData()
        {
            // Empty.
        }

        protected override void OnAllHeroesDead()
        {
            GameEntry.TimeScale.PauseGame();
            SetInstanceSuccess(InstanceSuccessReason.HasBeenBeaten);
        }
    }
}
