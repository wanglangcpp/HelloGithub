using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 锻造装备活动的数据类。
    /// </summary>
    [Serializable]
    public class GearFoundryData
    {
        [SerializeField]
        private int m_TeamId = -1;

        /// <summary>
        /// 队伍编号。-1 表示当前没有队伍。
        /// </summary>
        public int TeamId { get { return m_TeamId; } }

        [SerializeField]
        private GearFoundryProgressData m_Progress = new GearFoundryProgressData();

        /// <summary>
        /// 获取活动进度数据。
        /// </summary>
        public GearFoundryProgressData Progress { get { return m_Progress; } }

        [SerializeField]
        private GearFoundryPlayersData m_Players = new GearFoundryPlayersData();

        /// <summary>
        /// 获取全体玩家数据。
        /// </summary>
        public GearFoundryPlayersData Players { get { return m_Players; } }

        [SerializeField]
        private List<bool> m_RewardFlags = new List<bool>();

        [SerializeField]
        private GearFoundryInvitationsData m_Invitations = new GearFoundryInvitationsData();

        /// <summary>
        /// 获取邀请数据。
        /// </summary>
        public GearFoundryInvitationsData Invitations { get { return m_Invitations; } }

        [SerializeField]
        private DateTime m_NextFoundryTime = new DateTime();

        /// <summary>
        /// 获取下次可进行锻造操作的时间。
        /// </summary>
        public DateTime NextFoundryTime { get { return m_NextFoundryTime; } }

        /// <summary>
        /// 获取当前等级是否可以领奖。
        /// </summary>
        /// <param name="level">等级。</param>
        /// <returns></returns>
        public bool GetRewardFlagAtLevel(int level)
        {
            return m_RewardFlags[level];
        }

        /// <summary>
        /// 获取玩家是否在一个队伍中。
        /// </summary>
        public bool HasTeam
        {
            get
            {
                return m_TeamId >= 0;
            }
        }

        /// <summary>
        /// 当前玩家是否为队长。
        /// </summary>
        public bool AmILeader
        {
            get
            {
                if (m_Players.Data.Count <= 0)
                {
                    return false;
                }

                return GameEntry.Data.Player.Id == m_Players.Data[0].Player.Id;
            }
        }

        public void OnUpdate()
        {
            m_Invitations.OnUpdate();
        }

        public void UpdateData(PBGearFoundryInfo pbGearFoundry)
        {
            m_TeamId = pbGearFoundry.TeamId;
            UpdateData(pbGearFoundry.Players);
            if (pbGearFoundry.Progress != null)
            {
                UpdateData(pbGearFoundry.Progress);
            }
            UpdateData(pbGearFoundry.RewardFlags);
            UpdateData(pbGearFoundry.NextFoundryTimeInTicks);
        }

        public void UpdateData(long nextFoundryTimeInTicks)
        {
            m_NextFoundryTime = new DateTime(nextFoundryTimeInTicks, DateTimeKind.Utc);
        }

        public void UpdateDataAsCreator(int teamId)
        {
            m_TeamId = teamId;

            m_Players.ClearAndAddData(new List<PBGearFoundryPlayerInfo>
            {
                new PBGearFoundryPlayerInfo
                {
                    Player = new PBPlayerInfo
                    {
                        Id = GameEntry.Data.Player.Id,
                        Name = GameEntry.Data.Player.Name,
                        Level = GameEntry.Data.Player.Level,
                        VipLevel = GameEntry.Data.Player.VipLevel,
                        PortraitType = GameEntry.Data.Player.PortraitType,
                    },
                    FoundryCount = 0,
                }
            });

            m_Progress = new GearFoundryProgressData();

            int levelCount = 3; // GameEntry.ServerConfig.GetInt(Constant.ServerConfig.GearFoundryLevelCount, 3);
            m_RewardFlags = new List<bool>(levelCount);
            for (int i = 0; i < levelCount; ++i)
            {
                m_RewardFlags.Add(false);
            }
        }

        public void UpdateData(List<PBGearFoundryPlayerInfo> pbPlayers)
        {
            m_Players.ClearAndAddData(pbPlayers);
        }

        public void UpdateData(PBGearFoundryProgressInfo pbProgress)
        {
            m_Progress.UpdateData(pbProgress);
        }

        public void UpdateData(IList<bool> rewardFlags)
        {
            m_RewardFlags.Clear();
            m_RewardFlags.AddRange(rewardFlags);
        }

        public void LeaveTeam()
        {
            m_TeamId = -1;
            m_Players.ClearData();
            m_Progress = new GearFoundryProgressData();
            m_RewardFlags.Clear();
            m_Invitations.ClearData();
        }
    }
}
