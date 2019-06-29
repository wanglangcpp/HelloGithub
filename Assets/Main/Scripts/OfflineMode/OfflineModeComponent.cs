using GameFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线模式组件。
    /// </summary>
    public class OfflineModeComponent : MonoBehaviour
    {
        [SerializeField]
        private bool m_OfflineModeEnabled = false;

        //[SerializeField]
        //private bool m_ForceUseLocalTexts = true;

        [SerializeField]
        private OfflineModeConfig m_Config = null;

        private List<int> m_PreObtainedHeroTypeIds = null;

        /// <summary>
        /// 获取是否使用离线模式。
        /// </summary>
        public bool OfflineModeEnabled
        {
            get
            {
                return m_OfflineModeEnabled;
            }
        }

        /// <summary>
        /// 获取是否强制使用本地表和字典。
        /// </summary>
        public bool ForceUseLocalTexts
        {
            get
            {
                return true; //m_ForceUseLocalTexts;
            }
        }

        /// <summary>
        /// 获取或设置离线模式使用的场景。
        /// </summary>
        public int OfflineSceneId
        {
            get
            {
                return m_Config.OfflineSceneId;
            }
        }

        private void Awake()
        {
            m_OfflineModeEnabled &= Application.isEditor;
            //m_ForceUseLocalTexts &= Application.isEditor;
        }

        private void MockDataField<T>(T target, string fieldName, object value) where T : class
        {
            FieldInfo fieldInfo = typeof(T).GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo == null)
            {
                Log.Warning("GetField '{0}' failed.", fieldName);
                return;
            }

            fieldInfo.SetValue(target, value);
        }

        private void MockDataProperty<T>(T target, string propertyName, object value) where T : class
        {
            PropertyInfo propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            if (propertyInfo == null)
            {
                Log.Warning("GetProperty '{0}' failed.", propertyName);
                return;
            }

            propertyInfo.SetValue(target, value, null);
        }

        public void PrepareData()
        {
            if (!m_OfflineModeEnabled)
            {
                return;
            }

            if (m_Config == null)
            {
                Log.Error("Offline mode config is invalid.");
                return;
            }

            PreparePlayerData();
            PrepareLobbyHerosData();
            PrepareHeroTeamData();
            PrepareFriendsData();
            PreparePendingFriendRequestsData();
            PrepareItemsData();
            PrepareDailyQuestData();
            PrepareInstanceData();
            PrepareMeridianData();
            PrepareVipData();
        }

        private void PrepareVipData()
        {
            List<PBVipPrivilegeInfo> pb = new List<PBVipPrivilegeInfo>();
            for (int i = 0; i < (int)VipPrivilegeType.VipPrivilegeTypeCount; i++)
            {
                pb.Add(new PBVipPrivilegeInfo { VipPrivilegeType = i, UsedVipPrivilegeCount = 0, });
            }

            GameEntry.Data.VipsData.ClearAndAddData(pb);
        }

        private void PrepareItemsData()
        {
            for (int i = 0; i < m_Config.ItemDatas.Count; ++i)
            {
                var itemData = m_Config.ItemDatas[i];
                GameEntry.Data.Items.AddData(new PBItemInfo { Type = itemData.Type, Count = itemData.Count });
            }
        }

        private void PreparePlayerData()
        {
            MockDataField(GameEntry.Data.Player, "m_Name", m_Config.PlayerName);
            MockDataField(GameEntry.Data.Player, "m_Level", m_Config.PlayerLevel);
            MockDataField(GameEntry.Data.Player, "m_NextEnergyRecoveryTime", DateTime.UtcNow.AddMinutes(1.0));
            MockDataField(GameEntry.Data.Player, "m_Energy", 100);
        }

        private void PrepareLobbyHerosData()
        {
            m_PreObtainedHeroTypeIds = new List<int>(m_Config.PreObtainedHeroTypeIds).Distinct().ToList();
            m_PreObtainedHeroTypeIds.Sort();

            if (m_PreObtainedHeroTypeIds.Count < 0)
            {
                m_PreObtainedHeroTypeIds.Add(1);
            }

            var gearIndexCount = 3;
            foreach (int heroTypeId in m_PreObtainedHeroTypeIds)
            {
                LobbyHeroData lobbyHeroData = new LobbyHeroData();
                MockDataProperty(lobbyHeroData, "Type", heroTypeId);
                MockDataProperty(lobbyHeroData, "Level", m_Config.HeroLevel);
                MockDataProperty(lobbyHeroData, "StarLevel", m_Config.HeroStarLevel);
                MockDataProperty(lobbyHeroData, "SkillLevels", m_Config.SkillLevels);
                MockDataProperty(lobbyHeroData, "TotalQualityLevel", UnityEngine.Random.Range(1, 16));
                MockDataField(lobbyHeroData, "m_QualityItemSlotStates", new bool[] { false, false, false, false, false, false });
                var skillsBadges = new List<SkillBadgesData>();
                var pbHeroSkillBadgesInfo = new PBHeroSkillBadgesInfo { SpecificBadgeId = -1, HasGenericBadgeIds = true };

                for (int i = 0; i < Constant.Hero.MaxBadgeSlotCountPerSkill; ++i)
                {
                    pbHeroSkillBadgesInfo.GenericBadgeIds.Add(-1);
                }

                for (int i = 0; i < m_Config.SkillLevels.Count; ++i)
                {
                    var skillBadges = new SkillBadgesData();
                    skillBadges.UpdateData(pbHeroSkillBadgesInfo);
                    skillsBadges.Add(skillBadges);
                }
                MockDataField(lobbyHeroData, "m_SkillBadges", skillsBadges);

                lobbyHeroData.NewGears.ClearData();

                for (int i = 0; i < gearIndexCount; ++i)
                {
                    lobbyHeroData.NewGears.AddData(new PBNewGearInfo
                    {
                        TypeId = heroTypeId * 100 + i,
                        StrengthenLevel = 1,
                        TotalQualityLevel = 1,
                    });
                }

                PBLobbyHeroInfo pbLobbyHeroInfo = m_Config.DefaultHeroAttributes;
                pbLobbyHeroInfo.Type = heroTypeId;
                lobbyHeroData.UpdateData(pbLobbyHeroInfo);
                GameEntry.Data.LobbyHeros.Data.Add(lobbyHeroData);
            }
        }

        private void PrepareHeroTeamData()
        {
            var battleHeroTypeIds = new List<int>(m_Config.BattleHeroIds).Distinct().ToList();
            battleHeroTypeIds.RemoveAll(i => !m_PreObtainedHeroTypeIds.Contains(i));

            if (battleHeroTypeIds.Count <= 0)
            {
                battleHeroTypeIds.Add(m_PreObtainedHeroTypeIds[0]);
            }

            List<PBHeroTeamInfo> heroTeamInfo = new List<PBHeroTeamInfo>();
            for (int i = 0; i < (int)HeroTeamType.HeroTeamTypeCount; i++)
            {
                PBHeroTeamInfo heroTeam = new PBHeroTeamInfo();
                heroTeam.Type = i;
                heroTeamInfo.Add(heroTeam);
            }
            foreach (int heroId in m_Config.BattleHeroIds)
            {
                heroTeamInfo[(int)HeroTeamType.Default].HeroType.Add(heroId);
                if (heroTeamInfo[(int)HeroTeamType.Default].HeroType.Count >= Constant.MaxBattleHeroCount)
                {
                    break;
                }
            }

            GameEntry.Data.HeroTeams.ClearAndAddData(heroTeamInfo);
        }

        private void PrepareMeridianData()
        {
            var meridian = GameEntry.Data.Meridian;
            PBMeridianInfo meridianInfo = new PBMeridianInfo();
            meridianInfo.MeridianProgress = 360;
            meridian.UpdateData(meridianInfo);
        }

        private void PrepareFriendsData()
        {
            //var friends = GameEntry.Data.Friends;
            //for (int i = 0; i < 30; ++i)
            //{
            //    friends.AddFriend(new PBFriendInfo
            //    {
            //        PlayerInfo = new PBPlayerInfo
            //        {
            //            Id = i + 1,
            //            Level = i + 1,
            //            VipLevel = 3,
            //            Name = "Fake friend " + i,
            //        },
            //        LastClaimEnergyTime = DateTime.UtcNow.AddDays(-1).Ticks,
            //        LastGiveEnergyTime = DateTime.UtcNow.AddDays(-1).Ticks,
            //        LastReceiveEnergyTime = DateTime.UtcNow.AddDays(-1).Ticks
            //    });
            //}
        }

        private void PreparePendingFriendRequestsData()
        {
            //var pendingRequests = GameEntry.Data.FriendRequests;
            //for (int i = 0; i < 10; ++i)
            //{
            //    pendingRequests.AddRequest(new PBPlayerInfo
            //    {
            //        Id = i + 1,
            //        Level = i + 1,
            //        VipLevel = 3,
            //        Name = "Fake request " + i,
            //    });
            //}
        }

        private void PrepareDailyQuestData()
        {
            //             var data = GameEntry.Data.DailyQuests;
            // 
            //             var pb = new PBDailyQuestInfo();
            //             for (int i = 0; i < 4; ++i)
            //             {
            //                 pb.TrackingDailyQuests.Add(new PBTrackingDailyQuest { QuestId = i + 1, ProgressCount = i + 2 });
            //             }
            // 
            //             for (int i = 4; i < 8; ++i)
            //             {
            //                 pb.CompletedDailyQuests.Add(i + 1);
            //             }
            // 
            //             data.ClearAndAddData(pb);
        }

        private void PrepareInstanceData()
        {
            var instanceTable = GameEntry.DataTable.GetDataTable<DRInstance>().GetAllDataRows();
            for (int j = 0; j < instanceTable.Length; j++)
            {
                PBInstanceProgressInfo data = new PBInstanceProgressInfo();
                data.Id = instanceTable[j].Id;
                data.StarCount = 3;
                GameEntry.Data.InstanceProgresses.AddData(data);
            }
        }
    }
}
