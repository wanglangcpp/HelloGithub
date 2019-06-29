using GameFramework;
using GameFramework.Event;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public partial class SinglePlayerInstanceLogic : BaseSinglePlayerInstanceLogic
    {
        public override InstanceLogicType Type
        {
            get
            {
                return InstanceLogicType.SinglePlayer;
            }
        }

        private DRInstance m_DRInstance = null;

        protected override DRInstance DRInstance
        {
            get
            {
                return m_DRInstance;
            }
        }

        public override void Init(int instanceOrSceneId, object userData)
        {
            base.Init(instanceOrSceneId, userData);
            m_DRInstance = GetInstanceDataRow<DRInstance>(m_InstanceId);
            m_Requests = new Requests();
            InitInstanceDataBefore(m_DRInstance);
            InitGuidePoints(m_DRInstance);
        }

        public override void RequestStartInstance()
        {
            base.RequestStartInstance();
            InitNpcDataTable(m_DRInstance);
            InitBuildingDataTable(m_DRInstance);
            InitTimer(m_DRInstance);
            InitDropGoodsList();
        }


        private void SendLeaveInstanceRequest()
        {
            var MonsterCount = GameEntry.TaskComponent.MonsterCount;
            if (m_DRInstance.InstanceType == 0 || m_DRInstance.InstanceType == 1)
            {
                //推图副本
                CLLeaveInstance request = new CLLeaveInstance();
                request.InstanceType = m_DRInstance.Id;
                request.Win = (InstanceResultType == InstanceResultType.Win);
                request.StarCount = RequestCompleteCount;
                request.DropCoins = m_DeadDropCoins;
                foreach (var item in MonsterCount)
                {
                    PBMonster monster = new PBMonster();
                    monster.MonsterId = item.Key;
                    monster.MonsterCount = item.Value;
                    request.KillMonsters.Add(monster);
                }
                GameEntry.Network.Send(request);
            }
            else if (m_DRInstance.InstanceType == 2 || m_DRInstance.InstanceType == 3)
            {
                //boss副本
                CLLeaveInstanceForGroupBoss requestForBoss = new CLLeaveInstanceForGroupBoss();
                requestForBoss.Win = (InstanceResultType == InstanceResultType.Win);
                requestForBoss.InstanceType = m_DRInstance.Id;
                requestForBoss.StarCount = RequestCompleteCount;
                requestForBoss.DropCoins = m_DeadDropCoins;
                foreach (var item in MonsterCount)
                {
                    PBMonster monster = new PBMonster();
                    monster.MonsterId = item.Key;
                    monster.MonsterCount = item.Value;
                    requestForBoss.KillMonsters.Add(monster);
                }
                GameEntry.Network.Send(requestForBoss);
            }
            else if (m_DRInstance.InstanceType == 4)
            {
                //爬塔副本
                CLLeaveInstanceForTower requestForTower = new CLLeaveInstanceForTower();
                requestForTower.InstanceId = m_DRInstance.Id;
                requestForTower.Win = (InstanceResultType == InstanceResultType.Win);
           
                GameEntry.Network.Send(requestForTower);

            }

            GameEntry.TaskComponent.MonsterCount.Clear();
        }
        public void OnLevelInstance()
        {
            switch (m_DRInstance.InstanceType)
            {
                case 0:
                case 1:
                    GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenInstanceSelectForm, true);
                    break;
                case 2:
                case 3:
                    GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenActivityBossForm, true);
                    break;
                case 4:
                    GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenActivityTowerForm, true);
                    break;
                default:
                    break;
            }
        }

    }
}
