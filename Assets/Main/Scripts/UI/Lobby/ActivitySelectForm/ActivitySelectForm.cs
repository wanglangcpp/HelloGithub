using GameFramework;
using GameFramework.Event;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ActivitySelectForm : NGUIForm
    {
        [SerializeField]
        private ScrollViewCache m_ActivityScrollViewCache = null;

        private Dictionary<ActivityType, ActionSet> m_ActionSets = new Dictionary<ActivityType, ActionSet>();

        #region NGUIForm

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            ResetActionSets();
            RefreshActivities();
            GameEntry.Event.Subscribe(EventId.InstancesForResourceDataChanged, OnRefreshActivities);
            GameEntry.Event.Subscribe(EventId.OfflineArenaDataChanged, OnRefreshActivities);
        }

        protected override void OnClose(object userData)
        {
            GameEntry.Event.Unsubscribe(EventId.InstancesForResourceDataChanged, OnRefreshActivities);
            GameEntry.Event.Unsubscribe(EventId.OfflineArenaDataChanged, OnRefreshActivities);
            base.OnClose(userData);
        }

        #endregion NGUIForm

        public void SelectActivity(int activityId)
        {
            var drActivity = GameEntry.DataTable.GetDataTable<DRActivity>().GetDataRow(activityId);
            if (drActivity == null)
            {
                Log.Warning("Activity '{0}' not found in data table.", activityId.ToString());
                return;
            }

            ActionSet actionSet;
            if (!m_ActionSets.TryGetValue((ActivityType)drActivity.Id, out actionSet))
            {
                Log.Warning("No action set for activity type '{0}'.", ((ActivityType)drActivity.Id).ToString());
                return;
            }

            actionSet.OnSelectActivity();
        }

        private void ResetActionSets()
        {
            m_ActionSets.Clear();
            m_ActionSets.Add(ActivityType.OfflineArena, new ActionSet().Init(UIUtility.GetPlayCount_OfflineArena, EnterOfflineArena));
            m_ActionSets.Add(ActivityType.InstanceForResource_Coin, new ActionSet().Init(UIUtility.GetPlayCount_InstanceForCoinResource, () => { EnterInstanceForResource(InstanceForResourceType.Coin); }));
            m_ActionSets.Add(ActivityType.InstanceForResource_Exp, new ActionSet().Init(UIUtility.GetPlayCount_InstanceForExpResource, () => { EnterInstanceForResource(InstanceForResourceType.Exp); }));
            m_ActionSets.Add(ActivityType.MimicMeleeInstance, new ActionSet().Init(UIUtility.GetPlayCount_MimicMelee, () => { EnterMimicMeleeInstance(); }));
            m_ActionSets.Add(ActivityType.SinglePVP, new ActionSet().Init(UIUtility.GetPlayCount_InstanceForExpResource, () => { EnterInstanceForResource(InstanceForResourceType.Exp); }));
            m_ActionSets.Add(ActivityType.BossChallenge, new ActionSet().Init(UIUtility.GetPlayCount_BossChallenge, EnterBossChallenge));

            m_ActionSets.Add(ActivityType.StormTower, new ActionSet().Init(UIUtility.GetPlayCount_StormTower, EnterStormTower));
        }
        private void EnterBossChallenge()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ChallengeBossForm);
        }
        private void EnterStormTower()
        {
            GameEntry.UI.OpenUIForm(UIFormId.StormTowerForm);
        }
        private void EnterOfflineArena()
        {
            GameEntry.UI.OpenUIForm(UIFormId.ActivityOfflineArenaForm);
        }

        private void EnterInstanceForResource(InstanceForResourceType instanceForResourceType)
        {
            var allInstanceForResourceDataRows = GameEntry.DataTable.GetDataTable<DRInstanceForResource>().GetAllDataRows(dr => dr.InstanceResourceType == instanceForResourceType);
            for (int i = 0; i < allInstanceForResourceDataRows.Length; i++)
            {
                var drInstanceForResource = allInstanceForResourceDataRows[i];
                int levelRangeId = drInstanceForResource.LevelRangeId;
                var drLevelRange = GameEntry.DataTable.GetDataTable<DRLevelRange>().GetDataRow(levelRangeId);
                if (drLevelRange == null)
                {
                    Log.Warning("Level range '{0}' not found in data table.", levelRangeId.ToString());
                    continue;
                }

                if (GameEntry.Data.Player.Level >= drLevelRange.MinLevel && GameEntry.Data.Player.Level <= drLevelRange.MaxLevel)
                {
                    GameEntry.LobbyLogic.EnterInstanceForResource(drInstanceForResource.Id);
                    return;
                }
            }

            Log.Error("No instances for resource of type '{0}' match the player level '{1}'.", instanceForResourceType.ToString(), GameEntry.Data.Player.Level.ToString());
        }

        private void OnRefreshActivities(object sender, GameEventArgs e)
        {
            RefreshActivities();
        }

        private void RefreshActivities()
        {
            var allActivityDataRows = new List<DRActivity>(GameEntry.DataTable.GetDataTable<DRActivity>().GetAllDataRows(dr => dr.ShouldDisplay));
            allActivityDataRows.Sort(CompareActivityDataRows);

            for (int i = 0; i < allActivityDataRows.Count; i++)
            {
                var drActivity = allActivityDataRows[i];
                var activityItem = m_ActivityScrollViewCache.GetOrCreateItem(i, (go) => { go.name = drActivity.Id.ToString(); }, (go) => { go.name = drActivity.Id.ToString(); });

                ActionSet actionSet;
                if (!m_ActionSets.TryGetValue((ActivityType)drActivity.Id, out actionSet))
                {
                    Log.Error("No action set for activity type '{0}'.", ((ActivityType)drActivity.Id).ToString());
                    return;
                }

                activityItem.RefreshData(drActivity, actionSet.GetPlayCountDelegate);
            }
            m_ActivityScrollViewCache.ResetPosition();
            m_ActivityScrollViewCache.RecycleItemsAtAndAfter(allActivityDataRows.Count);
        }

        private int CompareActivityDataRows(DRActivity x, DRActivity y)
        {
            if (x.OrderPrioprity != y.OrderPrioprity)
            {
                return x.OrderPrioprity.CompareTo(y.OrderPrioprity);
            }

            return x.Id.CompareTo(y.Id);
        }

        private void EnterMimicMeleeInstance()
        {
            var allMimicMeleeInstanceDataRows = GameEntry.DataTable.GetDataTable<DRMimicMeleeInstance>().GetAllDataRows();
            for (int i = 0; i < allMimicMeleeInstanceDataRows.Length; i++)
            {
                var drMimicMeleeInstance = allMimicMeleeInstanceDataRows[i];
                int levelRangeId = drMimicMeleeInstance.LevelRangeId;
                var drLevelRange = GameEntry.DataTable.GetDataTable<DRLevelRange>().GetDataRow(levelRangeId);
                if (drLevelRange == null)
                {
                    Log.Warning("Level range '{0}' not found in data table.", levelRangeId.ToString());
                    continue;
                }

                if (GameEntry.Data.Player.Level >= drLevelRange.MinLevel && GameEntry.Data.Player.Level <= drLevelRange.MaxLevel)
                {
                    GameEntry.LobbyLogic.EnterMimicMeleeInstance(drMimicMeleeInstance.Id);
                    return;
                }
            }

            Log.Error("No mimic melee instance matches the player level '{0}'.", GameEntry.Data.Player.Level.ToString());
        }

        [Serializable]
        private class ScrollViewCache : UIScrollViewCache<ActivityItem>
        {

        }

        private class ActionSet
        {
            ActivityItem.GetPlayCountDelegate m_GetPlayCountDelegate;
            GameFrameworkAction m_OnSelectActivity;

            public ActionSet Init(ActivityItem.GetPlayCountDelegate getPlayCountDelegate, GameFrameworkAction onSelectActivity)
            {
                m_GetPlayCountDelegate = getPlayCountDelegate;
                m_OnSelectActivity = onSelectActivity;
                return this;
            }

            public ActivityItem.GetPlayCountDelegate GetPlayCountDelegate
            {
                get { return m_GetPlayCountDelegate; }
            }

            public GameFrameworkAction OnSelectActivity
            {
                get { return m_OnSelectActivity; }
            }
        }
    }
}
