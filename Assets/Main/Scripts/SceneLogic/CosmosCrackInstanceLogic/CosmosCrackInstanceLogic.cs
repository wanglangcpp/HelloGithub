namespace Genesis.GameClient
{
    /// <summary>
    /// 时空裂缝活动副本逻辑。
    /// </summary>
    public partial class CosmosCrackInstanceLogic : BaseSinglePlayerInstanceLogic
    {
        /// <summary>
        /// 初始化使用的用户数据。
        /// </summary>
        public class InitUserData
        {
            public int OverwritingNpcLevel;
        }

        private DRInstanceCosmosCrack m_DRInstance = null;
        private int m_OverwritingNpcLevel = 1;

        public override InstanceLogicType Type
        {
            get
            {
                return InstanceLogicType.CosmosCrack;
            }
        }

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
            m_OverwritingNpcLevel = (userData as InitUserData).OverwritingNpcLevel;
            m_DRInstance = GetInstanceDataRow<DRInstanceCosmosCrack>(m_InstanceId);
            m_Requests = new Requests();
            InitInstanceDataBefore(m_DRInstance);
            InitGuidePoints(m_DRInstance);
        }

        public override void RequestStartInstance()
        {
            base.RequestStartInstance();
            InitNpcDataTable(m_DRInstance);
            InitTimer(m_DRInstance);
        }

        /// <summary>
        /// 获取覆盖 NPC 和建筑的等级。
        /// </summary>
        /// <param name="originalLevel">原始等级。</param>
        /// <returns>覆盖后等级。</returns>
        /// <remarks>直接覆盖原有等级。</remarks>
        protected override int GetOverwrittenNpcLevel(int originalLevel)
        {
            return m_OverwritingNpcLevel;
        }
    }
}
