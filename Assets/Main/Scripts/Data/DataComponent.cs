using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 数据组件。
    /// </summary>
    public class DataComponent : MonoBehaviour
    {
        [SerializeField]
        private AccountData m_Account = new AccountData();

        [SerializeField]
        private PlayerData m_Player = new PlayerData();

        [SerializeField]
        private RoomData m_Room = new RoomData();

        [SerializeField]
        private LobbyHeroesData m_LobbyHeros = new LobbyHeroesData();

        [SerializeField]
        private HeroTeamsData m_HeroTeams = new HeroTeamsData();

        [SerializeField]
        private ItemsData m_Items = new ItemsData();

        [SerializeField]
        private ItemsData m_HeroQualityItems = new ItemsData();

        [SerializeField]
        private ItemsData m_SkillBadgeItems = new ItemsData();

        [SerializeField]
        private GearsData m_Gears = new GearsData();

        [SerializeField]
        private SoulsData m_Souls = new SoulsData();

        [SerializeField]
        private InstanceProgressesData m_InstanceProgresses = new InstanceProgressesData();

        [SerializeField]
        private InstanceGoodsData m_InstanceGoods = new InstanceGoodsData();

        [SerializeField]
        private InstanceDropsData m_InstanceDrops = new InstanceDropsData();

        [SerializeField]
        private ChessBattleMeData m_ChessBattleMeData = new ChessBattleMeData();

        [SerializeField]
        private ChessBattleEnemyData m_ChessBattleEnemyData = new ChessBattleEnemyData();

        [SerializeField]
        private MeridianData m_Meridian = new MeridianData();

        [SerializeField]
        private EpigraphsData m_Epigraphs = new EpigraphsData();

        [SerializeField]
        private EpigraphSlotsData m_EpigraphSlots = new EpigraphSlotsData();

        [SerializeField]
        private MailsData m_Mails = new MailsData();

        [SerializeField]
        private ChatsData m_ChatsData = new ChatsData();

        [SerializeField]
        private FriendsData m_Friends = new FriendsData();

        [SerializeField]
        private NearbyPlayersData m_NearbyPlayers = new NearbyPlayersData();

        [SerializeField]
        private FriendRequestsData m_FriendRequests = new FriendRequestsData();

        [SerializeField]
        private GearFoundryData m_GearFoundry = new GearFoundryData();

        [SerializeField]
        private ChancesData m_Chances = new ChancesData();

        [SerializeField]
        private OfflineArenaData m_OfflineArena = new OfflineArenaData();

        [SerializeField]
        private OfflineArenaBattleReportsData m_OfflineArenaBattleReports = new OfflineArenaBattleReportsData();

        [SerializeField]
        private OfflineArenaPlayerData m_OfflineArenaOpponent = new OfflineArenaPlayerData();

        [SerializeField]
        private CleanOutsData m_CleanOuts = new CleanOutsData();

        [SerializeField]
        private InstanceGroupsData m_InstanceGroups = new InstanceGroupsData();

        [SerializeField]
        private CosmosCrackData m_CosmosCrackData = new CosmosCrackData();

        [SerializeField]
        private PvpArenaData m_PvpArena = new PvpArenaData();

        [SerializeField]
        private PvpArenaPlayerAndTeamData m_PvpArenaOpponent = new PvpArenaPlayerAndTeamData();

        [SerializeField]
        private SingleMatchData m_SingleMatchData = new SingleMatchData();

        [SerializeField]
        private AchievementsData m_Achievements = new AchievementsData();

        [SerializeField]
        private DailyQuestsData m_DailyQuests = new DailyQuestsData();

        [SerializeField]
        private DailyLoginData m_DailyLogin = new DailyLoginData();

        [SerializeField]
        private ServerNamesData m_ServerNames = new ServerNamesData();

        [SerializeField]
        private ShopData m_ShopData = new ShopData();

        [SerializeField]
        private VipInfosData m_VipsData = new VipInfosData();

        [SerializeField]
        private InstanceForCoinResourceData m_InstanceForCoinResourceData = new InstanceForCoinResourceData();

        [SerializeField]
        private InstanceForExpResourceData m_InstanceForExpResourceData = new InstanceForExpResourceData();

        [SerializeField]
        private ChargeStatusData m_ChargeStatusData = new ChargeStatusData();

        [SerializeField]
        private TaskStepData m_TaskStepData = new TaskStepData();

        [SerializeField]
        private InstanceForBossData m_InstanceForBossData = new InstanceForBossData();

        [SerializeField]
        private StromTowerData m_StromTowerData = new StromTowerData();

        [SerializeField]
        private EveryDaySignInData m_EveryDaySignInData = new EveryDaySignInData();

        [SerializeField]
        private OnlineRewardsData m_OnlineRewardsData = new OnlineRewardsData();

        [SerializeField]
        private SevenDayLoginData m_SevenDayLoginData = new SevenDayLoginData();

        private IDictionary<int, EntityData> m_EntityDatas = new Dictionary<int, EntityData>();
        private IDictionary<string, object> m_TempData = new Dictionary<string, object>();

        /// <summary>
        /// 获取当前账户数据。
        /// </summary>
        public AccountData Account { get { return m_Account; } }

        /// <summary>
        /// 获取当前玩家数据。
        /// </summary>
        public PlayerData Player { get { return m_Player; } }

        /// <summary>
        /// 获取当前房间数据。
        /// </summary>
        public RoomData Room { get { return m_Room; } }

        /// <summary>
        /// 获取大厅英雄数据。
        /// </summary>
        public LobbyHeroesData LobbyHeros { get { return m_LobbyHeros; } }

        /// <summary>
        /// 获取英雄阵容数据。
        /// </summary>
        public HeroTeamsData HeroTeams { get { return m_HeroTeams; } }

        /// <summary>
        /// 获取道具数据（不含英雄升品道具）。
        /// </summary>
        public ItemsData Items { get { return m_Items; } }

        /// <summary>
        /// 获取英雄生品道具数据。
        /// </summary>
        public ItemsData HeroQualityItems { get { return m_HeroQualityItems; } }

        /// <summary>
        /// 获取技能徽章道具数据
        /// </summary>
        public ItemsData SkillBadgeItems { get { return m_SkillBadgeItems; } }

        /// <summary>
        /// 获取装备数据。
        /// </summary>
        public GearsData Gears { get { return m_Gears; } }

        /// <summary>
        /// 获取战魂数据。
        /// </summary>
        public SoulsData Souls { get { return m_Souls; } }

        /// <summary>
        /// 获取副本进度数据。
        /// </summary>
        public InstanceProgressesData InstanceProgresses { get { return m_InstanceProgresses; } }

        /// <summary>
        /// 获取副本奖励物品数据。
        /// </summary>
        public InstanceGoodsData InstanceGoods { get { return m_InstanceGoods; } }

        /// <summary>
        /// 获取副本掉落数据。
        /// </summary>
        public InstanceDropsData InstanceDrops { get { return m_InstanceDrops; } }

        /// <summary>
        /// 获取翻翻棋副本中本玩家的数据。
        /// </summary>
        public ChessBattleMeData ChessBattleMe { get { return m_ChessBattleMeData; } }

        /// <summary>
        /// 获取翻翻棋副本中对方的数据。
        /// </summary>
        public ChessBattleEnemyData ChessBattleEnemy { get { return m_ChessBattleEnemyData; } }

        /// <summary>
        /// 获取星图数据。
        /// </summary>
        public MeridianData Meridian { get { return m_Meridian; } }

        /// <summary>
        /// 获取铭文数据。
        /// </summary>
        public EpigraphsData Epigraphs { get { return m_Epigraphs; } }

        /// <summary>
        /// 获取铭文槽位数据。
        /// </summary>
        public EpigraphSlotsData EpigraphSlots { get { return m_EpigraphSlots; } }

        /// <summary>
        /// 获取邮件数据。
        /// </summary>
        public MailsData Mails { get { return m_Mails; } }

        /// <summary>
        /// 获取聊天数据。
        /// </summary>
        public ChatsData Chat { get { return m_ChatsData; } }

        /// <summary>
        /// 获取好友数据。
        /// </summary>
        public FriendsData Friends { get { return m_Friends; } }

        /// <summary>
        /// 获取附近玩家数据。
        /// </summary>
        public NearbyPlayersData NearbyPlayers { get { return m_NearbyPlayers; } }

        /// <summary>
        /// 获取待处理好友请求数据。
        /// </summary>
        public FriendRequestsData FriendRequests { get { return m_FriendRequests; } }

        /// <summary>
        /// 获取锻造装备活动数据。
        /// </summary>
        public GearFoundryData GearFoundry { get { return m_GearFoundry; } }

        /// <summary>
        /// 获取抽奖数据。
        /// </summary>
        public ChancesData Chances { get { return m_Chances; } }

        /// <summary>
        /// 获取离线竞技数据。
        /// </summary>
        public OfflineArenaData OfflineArena { get { return m_OfflineArena; } }

        /// <summary>
        /// 获取离线竞技战报数据。
        /// </summary>
        public OfflineArenaBattleReportsData OfflineArenaBattleReports { get { return m_OfflineArenaBattleReports; } }

        /// <summary>
        /// 获取离线竞技对手数据。
        /// </summary>
        public OfflineArenaPlayerData OfflineArenaOpponent { get { return m_OfflineArenaOpponent; } }

        /// <summary>
        /// 获取扫荡数据。
        /// </summary>
        public CleanOutsData CleanOuts { get { return m_CleanOuts; } }

        /// <summary>
        /// 获取副本组数据
        /// 目前这个数据结构保存了配置文件的数据以及服务器下发的副本进度数据，游戏中用的副本数据请用此结构
        /// </summary>
        public InstanceGroupsData InstanceGroups { get { return m_InstanceGroups; } }

        /// <summary>
        /// 获取副本组数据
        /// </summary>
        public PvpArenaData PvpArena { get { return m_PvpArena; } }

        /// <summary>
        /// 获取副本组数据,1v1敌方数据也在这里
        /// </summary>
        public PvpArenaPlayerAndTeamData PvpArenaOpponent { get { return m_PvpArenaOpponent; } }

        /// <summary>
        /// 获取服务器名字列表数据
        /// </summary>
        public ServerNamesData ServerNames { get { return m_ServerNames; } }

        public int InstanceStoryResidueDegree { get; set; }

        /// <summary>
        /// 获取时空裂缝数据。
        /// </summary>
        public CosmosCrackData CosmosCrackData { get { return m_CosmosCrackData; } }

        /// <summary>
        /// 获取成就数据。
        /// </summary>
        public AchievementsData Achievements { get { return m_Achievements; } }

        /// <summary>
        /// 每日任务数据。
        /// </summary>
        public DailyQuestsData DailyQuests { get { return m_DailyQuests; } }

        /// <summary>
        /// 每日签到数据
        /// </summary>
        public DailyLoginData DailyLogin { get { return m_DailyLogin; } }

        /// <summary>
        /// 商店数据。
        /// </summary>
        public ShopData ShopsData { get { return m_ShopData; } }

        /// <summary>
        /// Vip数据。
        /// </summary>
        public VipInfosData VipsData { get { return m_VipsData; } }

        /// <summary>
        /// 金币资源副本数据。
        /// </summary>
        public InstanceForCoinResourceData InstanceForCoinResourceData { get { return m_InstanceForCoinResourceData; } }

        /// <summary>
        /// 经验资源副本数据。
        /// </summary>
        public InstanceForExpResourceData InstanceForExpResourceData { get { return m_InstanceForExpResourceData; } }
        /// <summary>
        /// 单人1v1匹配本方数据
        /// </summary>
        public SingleMatchData SingleMatchData { get { return m_SingleMatchData; } set { m_SingleMatchData = value; } }

        /// <summary>
        /// 充值状态
        /// </summary>
        public ChargeStatusData ChargeStatusData { get { return m_ChargeStatusData; } }

        /// <summary>
        /// 当前进行中的所有任务的任务进度
        /// </summary>
        public TaskStepData TaskStepData { get { return m_TaskStepData; } }

        public InstanceForBossData InstanceForBossData { get { return m_InstanceForBossData; } }

        /// <summary>
        /// 风暴之塔数据
        /// </summary>
        public StromTowerData StromTowerData { get { return m_StromTowerData; } }

        /// <summary>
        /// 每日签到数据
        /// </summary>
        public EveryDaySignInData EveryDaySignInData { get { return m_EveryDaySignInData; } }
        /// <summary>
        /// 在线奖励数据
        /// </summary>
        public OnlineRewardsData OnlineRewardsData { get { return m_OnlineRewardsData; } }
        /// <summary>
        /// 七日登录数据
        /// </summary>
        public SevenDayLoginData SevenDayLoginData { get { return m_SevenDayLoginData; } }
        /// <summary>
        /// 充值表
        /// </summary>
        public List<ChargeInfo> ChargeTable { get; set; }

        private void Awake()
        {
            Log.Info("Data component has been initialized.");
        }

        private void Update()
        {
            GearFoundry.OnUpdate();
        }

        public EntityData GetEntityData(int entityId)
        {
            EntityData entityData = null;
            if (!m_EntityDatas.TryGetValue(entityId, out entityData))
            {
                return null;
            }

            return entityData;
        }

        public T GetEntityData<T>(int entityId) where T : EntityData
        {
            return GetEntityData(entityId) as T;
        }

        public void RegisterEntityData(EntityData entityData)
        {
            int entityId = entityData.Id;
            EntityData data = GetEntityData(entityId);
            if (data != null && data.Entity != null)
            {
                Log.Warning("Duplicate entity data '{0}'.", entityId.ToString());
            }

            m_EntityDatas[entityId] = entityData;
        }

        public void ClearAllEntityDatas()
        {
            m_EntityDatas.Clear();
        }

        /// <summary>
        /// 是否存在临时数据。
        /// </summary>
        /// <param name="key">关键字。</param>
        /// <returns>存在与否。</returns>
        public bool HasTempData(string key)
        {
            return m_TempData.ContainsKey(key);
        }

        /// <summary>
        /// 获取临时数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="key">关键字。</param>
        /// <returns>数据的值。</returns>
        public T GetTempData<T>(string key)
        {
            return (T)(m_TempData[key]);
        }

        /// <summary>
        /// 获取并删除临时数据。
        /// </summary>
        /// <typeparam name="T">数据类型。</typeparam>
        /// <param name="key">关键字。</param>
        /// <returns>数据的值。</returns>
        public T GetAndRemoveTempData<T>(string key)
        {
            T tempData = GetTempData<T>(key);
            RemoveTempData(key);
            return tempData;
        }

        /// <summary>
        /// 添加或更新临时数据。
        /// </summary>
        /// <param name="key">关键字。</param>
        /// <param name="data">数据的值。</param>
        public void AddOrUpdateTempData(string key, object data)
        {
            m_TempData[key] = data;
        }

        /// <summary>
        /// 删除临时数据。
        /// </summary>
        /// <param name="key">关键字。</param>
        /// <returns>是否删除成功。</returns>
        public bool RemoveTempData(string key)
        {
            return m_TempData.Remove(key);
        }
    }
}
