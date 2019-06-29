using GameFramework;
using GameFramework.Fsm;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ReceiveGearForm : NGUIForm
    {
        [SerializeField]
        private UISprite m_GearInfoIcon = null;

        [SerializeField]
        private UILabel m_GearNameText = null;

        [SerializeField]
        private UILabel m_GearTypeText = null;

        [SerializeField]
        private UILabel m_GearCurrentLevel = null;

        [SerializeField]
        private UISprite[] m_GearStars = null;

        [SerializeField]
        private Animation[] m_OpenAnimations = null;

        [SerializeField]
        private UIGrid m_GearAttributeList = null;

        [SerializeField]
        private GameObject m_GearAttribute = null;

        [SerializeField]
        private float m_MinDisplayTime = 0.3f;

        [SerializeField]
        private float m_MaxDisplayTime = 3f;

        private List<KeyValuePair<int, int>> m_ShuffledGoods = new List<KeyValuePair<int, int>>();
        private int m_CurrentIndex = 0;
        private IFsm<ReceiveGearForm> m_Fsm = null;
        private float m_CurrentDisplayTime = 0f;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            SetOpenAnimVisible(false);
            var data = userData as ReceiveData;
            if (data == null)
            {
                Log.Error("User data is invalid.");
                return;
            }

            m_ShuffledGoods = data.Datas;
            ClearAttributes();

            m_Fsm = GameEntry.Fsm.CreateFsm(this,
                new StateInit(),
                new StateLoading(),
                new StateAnimating(),
                new StateIdle());
            m_Fsm.Start<StateInit>();
        }

        protected override void OnClose(object userData)
        {
            m_ShuffledGoods.Clear();
            GameEntry.Fsm.DestroyFsm(m_Fsm);
            base.OnClose(userData);
        }

        public void OnclickWholeScreenBtn()
        {
            (m_Fsm.CurrentState as StateBase).OnClickWholeScreenButton(m_Fsm);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        private void ShuffleGoods()
        {
            AIUtility.Shuffle(m_ShuffledGoods);
        }

        private void ClearAttributes()
        {
            var childList = m_GearAttributeList.GetChildList();
            for (int i = 0; i < childList.Count; i++)
            {
                Destroy(childList[i].gameObject);
            }
        }

        private void SetOpenAnimVisible(bool isVisible)
        {
            for (int i = 0; i < m_OpenAnimations.Length; i++)
            {
                m_OpenAnimations[i].gameObject.SetActive(isVisible);
            }
        }

        private void PlayOpenAnim()
        {
            for (int i = 0; i < m_OpenAnimations.Length; i++)
            {
                m_OpenAnimations[i][m_OpenAnimations[i].clip.name].time = 0;
                m_OpenAnimations[i].Sample();
                m_OpenAnimations[i].Play();
            }
        }

        private bool IsOpenAnimPlaying()
        {
            bool isPlaying = false;
            for (int i = 0; i < m_OpenAnimations.Length; i++)
            {
                if (m_OpenAnimations[i].isPlaying)
                {
                    isPlaying = true;
                    break;
                }
            }
            return isPlaying;
        }

        private class StateBase : FsmState<ReceiveGearForm>
        {
            protected IFsm<ReceiveGearForm> m_Fsm = null;
            protected ReceiveGearForm m_Owner = null;
            private bool m_CloseSelf = false;

            protected override void OnInit(IFsm<ReceiveGearForm> fsm)
            {
                base.OnInit(fsm);
                m_Fsm = fsm;
                m_Owner = fsm.Owner;
                m_CloseSelf = false;
            }

            public virtual void OnClickWholeScreenButton(IFsm<ReceiveGearForm> fsm)
            {

            }

            protected void Next(IFsm<ReceiveGearForm> fsm)
            {
                if (m_CloseSelf)
                {
                    return;
                }

                if (m_Owner.m_CurrentIndex + 1 >= m_Owner.m_ShuffledGoods.Count)
                {
                    m_CloseSelf = true;
                    m_Owner.CloseSelf();
                    return;
                }
                m_Owner.ClearAttributes();
                m_Owner.m_CurrentIndex++;
                ChangeState<StateLoading>(fsm);
            }
        }

        private class StateInit : StateBase
        {
            protected override void OnEnter(IFsm<ReceiveGearForm> fsm)
            {
                base.OnEnter(fsm);
                m_Owner.ShuffleGoods();
                m_Owner.SetOpenAnimVisible(false);
                m_Owner.m_CurrentIndex = 0;
                m_Owner.m_CurrentDisplayTime = 0f;
                ChangeState<StateLoading>(fsm);
            }
        }

        private class StateLoading : StateBase
        {
            private DRGear m_GearData = null;

            protected override void OnEnter(IFsm<ReceiveGearForm> fsm)
            {
                base.OnEnter(fsm);
                m_Owner.m_EffectsController.Pause();
                fsm.Owner.SetOpenAnimVisible(true);
                fsm.Owner.PlayOpenAnim();

                m_Owner.m_CurrentDisplayTime = 0f;
                int typeId = m_Owner.m_ShuffledGoods[m_Owner.m_CurrentIndex].Key;
                var dataTable = GameEntry.DataTable.GetDataTable<DRGear>();
                m_GearData = dataTable.GetDataRow(typeId);
                if (m_GearData == null)
                {
                    Log.Error("Cannot found gear with type '{0}'.", typeId);
                    return;
                }
                ShowAttributes();
                SetGearInfo();
                fsm.Owner.m_GearAttributeList.repositionNow = true;
            }

            private void ShowAttributes()
            {
                var gearData = new GearData();
                gearData.UpdateData(new PBGearInfo
                {
                    Id = 1, // No use.
                    Type = m_GearData.Id,
                    Level = 1,
                    StrengthenLevel = 0,
                });

                var displayer = new GearAttributeDisplayer<GearInfoAttributeItem>(gearData, GearAttributeNewValueMask.Default);
                displayer.SetNameAndCurrentValueDelegate += SetAttributeNameAndValue;
                displayer.GetItemDelegate += CreateAttributeItem;
                displayer.Run();
            }

            private void InitAttribute(string key, string value)
            {
                var script = CreateAttributeItem(0);
                SetAttributeNameAndValue(script, GameEntry.Localization.GetString(key), value);
            }

            private GearInfoAttributeItem CreateAttributeItem(int index)
            {
                var go = NGUITools.AddChild(m_Fsm.Owner.m_GearAttributeList.gameObject, m_Fsm.Owner.m_GearAttribute);
                var script = go.GetComponent<GearInfoAttributeItem>();
                return script;
            }

            private void SetAttributeNameAndValue(GearInfoAttributeItem script, string name, string value)
            {
                script.Name = name;
                script.Value = value;
            }

            private void SetGearInfo()
            {
                m_Fsm.Owner.m_GearInfoIcon.LoadAsync(m_GearData.IconId);
                m_Fsm.Owner.m_GearNameText.color = ColorUtility.GetColorForQuality(m_GearData.Quality);
                m_Fsm.Owner.m_GearNameText.text = GameEntry.Localization.GetString(m_GearData.Name);
                m_Fsm.Owner.m_GearTypeText.text = GameEntry.Localization.GetString(Constant.Gear.GearTypeNameDics[m_GearData.Type]);
                m_Fsm.Owner.m_GearCurrentLevel.text = GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", 1);
                UIUtility.SetStarLevel(m_Fsm.Owner.m_GearStars, 0);
                m_Fsm.Owner.m_GearAttributeList.Reposition();
                ChangeState<StateAnimating>(m_Fsm);
            }
        }

        private class StateAnimating : StateBase
        {
            protected override void OnEnter(IFsm<ReceiveGearForm> fsm)
            {
                base.OnEnter(fsm);
            }

            protected override void OnUpdate(IFsm<ReceiveGearForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                m_Owner.m_CurrentDisplayTime += realElapseSeconds;
                m_Owner.m_EffectsController.Resume();
                if (!m_Owner.IsOpenAnimPlaying())
                {
                    ChangeState<StateIdle>(fsm);
                }
            }

            public override void OnClickWholeScreenButton(IFsm<ReceiveGearForm> fsm)
            {
                if (m_Owner.IsOpenAnimPlaying())
                {
                    return;
                }
                if (m_Owner.m_CurrentDisplayTime <= m_Owner.m_MinDisplayTime)
                {
                    return;
                }

                Next(fsm);
            }
        }

        private class StateIdle : StateBase
        {
            protected override void OnUpdate(IFsm<ReceiveGearForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                m_Owner.m_CurrentDisplayTime += realElapseSeconds;

                if (m_Owner.m_CurrentDisplayTime > m_Owner.m_MaxDisplayTime)
                {
                    Next(fsm);
                }
            }

            public override void OnClickWholeScreenButton(IFsm<ReceiveGearForm> fsm)
            {
                if (m_Owner.m_CurrentDisplayTime <= m_Owner.m_MinDisplayTime)
                {
                    return;
                }

                Next(fsm);
            }
        }
    }
}
