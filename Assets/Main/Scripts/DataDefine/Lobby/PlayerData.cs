using GameFramework;
using GameFramework.DataTable;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class PlayerData : IGenericData<PlayerData, PBPlayerInfo>
    {
        [SerializeField]
        private int m_Id;

        [SerializeField]
        private string m_Name;

        [SerializeField]
        private bool m_IsFemale;

        [SerializeField]
        private int m_PortraitType;

        [SerializeField]
        private int m_Level;

        [SerializeField]
        private int m_Exp;

        [SerializeField]
        private int m_Money;

        [SerializeField]
        private int m_Coin;

        [SerializeField]
        private int m_Energy;

        [SerializeField]
        private int m_VipLevel;

        [SerializeField]
        private int m_VipExp;

        [SerializeField]
        private int m_MeridianEnergy;

        [SerializeField]
        private int m_ArenaToken;

        [SerializeField]
        private int m_PvpToken;

        [SerializeField]
        private int m_Spirit;

        [SerializeField]
        private int m_DragonStripeToken;

        [SerializeField]
        private DateTime m_NextEnergyRecoveryTime;

        [SerializeField]
        private bool m_IsOnline;

        [SerializeField]
        private int m_TeamMight;

        [SerializeField]
        private Vector2 m_LoginPostion;

        [SerializeField]
        private int m_ActivenessToken;
        [SerializeField]
        private int m_GuidanceGroupId;
        [SerializeField]
        private bool m_IsLevelUp = false;
        public bool IsLevelUp { set { m_IsLevelUp = value; } get { return m_IsLevelUp; } }

        public int Key { get { return m_Id; } }
        public int Id { get { return m_Id; } set { m_Id = value; } }
        public string Name { get { return m_Name; } }
        public bool IsFemale { get { return m_IsFemale; } }
        public int PortraitType { get { return m_PortraitType; } }

        public int Level
        {
            get
            {
                return m_Level;
            }
        }

        public int Exp
        {
            get
            {
                return m_Exp;
            }
        }

        public const int IdToDisplayIdExtraValue = 1000000;

        public float ExpRatio
        {
            get
            {
                IDataTable<DRPlayer> dt = GameEntry.DataTable.GetDataTable<DRPlayer>();
                DRPlayer dr = dt.GetDataRow(m_Level);
                if (dr == null)
                {
                    return 0f;
                }

                return dr.LevelUpExp > 0 ? (float)m_Exp / dr.LevelUpExp : 0f;
            }
        }

        public int TeamMight
        {
            get
            {
                int might = 0;
                for (int i = 0; i < GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).HeroType.Count; i++)
                {
                    LobbyHeroData data = GameEntry.Data.LobbyHeros.GetData(GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Default).HeroType[i]);
                    if (data != null)
                    {
                        might += data.Might;
                    }
                }

                return m_TeamMight > 0 ? m_TeamMight : might;
            }
        }

        /// <summary>
        /// 获取钻石数量。
        /// </summary>
        public int Money { get { return m_Money; } }

        /// <summary>
        /// 获取金币数量。
        /// </summary>
        public int Coin { get { return m_Coin; } }

        /// <summary>
        /// 获取体力。
        /// </summary>
        public int Energy { get { return m_Energy; } }

        /// <summary>
        /// 获取 VIP 等级。
        /// </summary>
        public int VipLevel { get { return m_VipLevel; } }

        /// <summary>
        /// 获取 VIP 经验。
        /// </summary>
        public int VipExp { get { return m_VipExp; } }

        /// <summary>
        /// 获取星图能量。
        /// </summary>
        public int MeridianEnergy { get { return m_MeridianEnergy; } }

        /// <summary>
        /// 获取离线竞技代币。
        /// </summary>
        public int ArenaToken { get { return m_ArenaToken; } }

        /// <summary>
        /// PVP竞技代币。
        /// </summary>
        public int PvpToken { get { return m_PvpToken; } }

        /// <summary>
        /// 获取魂魄数量。
        /// </summary>
        public int Spirit { get { return m_Spirit; } }

        /// <summary>
        /// 活跃度。
        /// </summary>
        public int ActivenessToken { set { m_ActivenessToken = value; } get { return m_ActivenessToken; } }

        /// <summary>
        /// 龙纹币数量。
        /// </summary>
        public int DragonStripeToken { get { return m_DragonStripeToken; } }

        /// <summary>
        /// 显示用身份号码。
        /// </summary>
        public int DisplayId { get { return m_Id + IdToDisplayIdExtraValue; } }

        /// <summary>
        /// 下一次恢复体力的时间。
        /// </summary>
        public DateTime NextEnergyRecoveryTime { get { return m_NextEnergyRecoveryTime; } }

        /// <summary>
        /// Player是否在线。
        /// </summary>
        public bool IsOnline { get { return m_IsOnline; } }

        /// <summary>
        /// 登陆主城的位置。
        /// </summary>
        public Vector2 LoginPostion { get { return m_LoginPostion; } set { m_LoginPostion = value; } }
        /// <summary>
        /// 下一个新手引导GroupID
        /// </summary>
        public int GuidanceGroupId { get { return m_GuidanceGroupId; } set { m_GuidanceGroupId = value; } }

        /// <summary>
        /// 获取某种代币的数量。
        /// </summary>
        /// <param name="currencyType">代币类型。</param>
        /// <returns></returns>
        public int GetCurrencyAmount(CurrencyType currencyType)
        {
            switch (currencyType)
            {
                case CurrencyType.Coin:
                    return Coin;
                case CurrencyType.Energy:
                    return Energy;
                case CurrencyType.MeridianToken:
                    return MeridianEnergy;
                case CurrencyType.Money:
                    return Money;
                case CurrencyType.SpiritToken:
                    return Spirit;
                case CurrencyType.DragonStripeToken:
                    return DragonStripeToken;
                case CurrencyType.ArenaToken:
                    return ArenaToken;
                default:
                    Log.Warning("CurrencyType '{0}' is unsupported.", currencyType);
                    return 0;
            }
        }

        public void UpdateData(PBPlayerInfo data)
        {
            m_Id = data.Id;
            if (data.HasMight)
            {
                m_TeamMight = data.Might;
            }

            if (data.HasName)
            {
                m_Name = data.Name;
            }

            if (data.HasIsFemale)
            {
                m_IsFemale = data.IsFemale;
            }

            if (data.HasPortraitType)
            {
                m_PortraitType = data.PortraitType;
            }

            if (data.HasLevel)
            {
                Log.Debug("palyer old level:{0}...", m_Level);
                Log.Debug("player current  level: {0}...", data.Level);
                if (data.Level > m_Level && m_Level != 0)
                {
                    m_IsLevelUp = true;
                    Log.Debug("player level up...");
                    m_Level = data.Level;
                    SDKManager.Instance.helper.UploadData("LevelUp");
                }
                m_Level = data.Level;
            }

            if (data.HasExp)
            {
                m_Exp = data.Exp;
            }

            if (data.HasMoney)
            {
                m_Money = data.Money;
            }

            if (data.HasCoin)
            {
                m_Coin = data.Coin;
            }

            if (data.HasEnergy)
            {
                m_Energy = data.Energy;
            }

            if (data.HasVipLevel)
            {
                m_VipLevel = data.VipLevel;
            }

            if (data.HasVipExp)
            {
                m_VipExp = data.VipExp;
            }

            if (data.HasMeridianEnergy)
            {
                m_MeridianEnergy = data.MeridianEnergy;
            }

            if (data.HasArenaToken)
            {
                m_ArenaToken = data.ArenaToken;
            }

            if (data.HasPvpToken)
            {
                m_PvpToken = data.PvpToken;
            }

            if (data.HasSpirit)
            {
                m_Spirit = data.Spirit;
            }

            if (data.HasDragonStripeToken)
            {
                m_DragonStripeToken = data.DragonStripeToken;
            }

            if (data.NextEnergyRecoveryTime != null)
            {
                m_NextEnergyRecoveryTime = TimeComponent.IsInvalidTime(data.NextEnergyRecoveryTime) ? TimeComponent.InvalidDateTime : new DateTime(data.NextEnergyRecoveryTime, DateTimeKind.Utc);
            }

            if (data.HasIsOnline)
            {
                m_IsOnline = data.IsOnline;
            }

            if (data.Position != null)
            {
                m_LoginPostion = data.Position;
            }

            if (data.HasActivenessToken)
            {
                m_ActivenessToken = data.ActivenessToken;
            }

            if (data.HasGuidanceGroupId)
            {
                m_GuidanceGroupId = data.GuidanceGroupId;
            }
        }
    }
}
