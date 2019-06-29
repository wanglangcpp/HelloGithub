using GameFramework;
using GameFramework.DataTable;
using GameFramework.Fsm;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ReceiveHeroForm : NGUIForm
    {
        [SerializeField]
        private Transform m_PlatformRoot = null;

        [SerializeField]
        private Camera m_SecondaryCamera = null;

        [SerializeField]
        private ReceiveHeroInfo m_HeroInfo = null;

        [SerializeField]
        private UILabel m_CongrContent = null;

        [SerializeField]
        private Animation m_CongrAnimation = null;

        private Vector3 m_PlatformDefaultPosition;

        private FakeCharacter m_Character = null;

        private int m_IndexInAllHeroes = -1;

        private List<DRHero> m_AllHeroes = new List<DRHero>();

        private List<int> m_HeroChips = new List<int>();

        private IFsm<ReceiveHeroForm> m_Fsm = null;

        private static int s_SerialId = 0;

        private DRHero HeroData
        {
            get
            {
                return m_AllHeroes[m_IndexInAllHeroes];
            }
        }

        private int HeroChipCount
        {
            get
            {
                return m_HeroChips[m_IndexInAllHeroes];
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_PlatformDefaultPosition = m_PlatformRoot.localPosition;
        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            GameEntry.Impact.IncreaseHidingNameBoardCount();
            m_AllHeroes.Clear();
            m_HeroChips.Clear();
            IDataTable<DRHero> dtHero = GameEntry.DataTable.GetDataTable<DRHero>();
            var userDataDict = userData as ReceiveData;
            for (int i = 0; i < userDataDict.Datas.Count; i++)
            {
                DRHero dr = dtHero.GetDataRow(userDataDict.Datas[i].Key);
                if (dr == null)
                {
                    Log.Error("ReceiveHeroForm OnOpen get DRHero is null!! Id is '{0}'", userDataDict.Datas[i].Key);
                }
                m_AllHeroes.Add(dr);
                m_HeroChips.Add(userDataDict.Datas[i].Value);
            }
            m_IndexInAllHeroes = 0;
            m_Fsm = GameEntry.Fsm.CreateFsm(GetType().Name + (s_SerialId++).ToString(), this,
                        new InitState(),
                        new DebutState(),
                        new DefaultState(),
                        new EndState());
            m_Fsm.Start<InitState>();
        }

        protected override void OnResume()
        {
            base.OnResume();
            m_SecondaryCamera.enabled = true;
            m_PlatformRoot.localPosition = m_PlatformDefaultPosition;
        }

        protected override void OnPause()
        {
            if (!GameEntry.IsAvailable) return;

            m_SecondaryCamera.enabled = false;
            base.OnPause();
        }

        protected override void OnClose(object userData)
        {
            GameEntry.Impact.DecreaseHidingNameBoardCount();
            if (!GameEntry.IsAvailable) return;

            m_PlatformRoot.localPosition = m_PlatformDefaultPosition;
            GameEntry.Fsm.DestroyFsm(m_Fsm);
            m_Fsm = null;
            base.OnClose(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        public void OnClickNextStep()
        {
            (m_Fsm.CurrentState as StateBase).SkipAnimation(m_Fsm);
        }

        private void ShowNextHero()
        {
            m_IndexInAllHeroes++;
            if (m_IndexInAllHeroes < m_AllHeroes.Count)
            {
                (m_Fsm.CurrentState as StateBase).StartInit(m_Fsm);
            }
            else
            {
                CloseSelf();
            }
        }
    }
}
