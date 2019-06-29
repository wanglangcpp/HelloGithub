using GameFramework;
using GameFramework.Fsm;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 获得奖励界面。
    /// </summary>
    public class ReceiveItemForm : NGUIForm
    {
        [SerializeField]
        private Animation m_GeneralItemAnimation = null;

        [SerializeField]
        private Transform m_Animated = null;

        [SerializeField]
        private GeneralItemView m_Item = null;

        [SerializeField]
        private UILabel m_Text = null;

        [SerializeField]
        private float m_MinDisplayTime = 0.3f;

        private List<KeyValuePair<int, int>> m_ShuffledGoods = new List<KeyValuePair<int, int>>();
        private int m_CurrentIndex = 0;
        private float m_CurrentDisplayTime = 0f;
        private IFsm<ReceiveItemForm> m_Fsm = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            var data = userData as ReceiveData;
            if (data == null)
            {
                Log.Error("User data is invalid.");
                return;
            }

            m_ShuffledGoods = data.Datas;

            m_Fsm = GameEntry.Fsm.CreateFsm(this,
                new StateInit(),
                new StateLoading(),
                new StateAnimating(),
                new StateIdle());
            m_Fsm.Start<StateInit>();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnClose(object userData)
        {
            GameEntry.Fsm.DestroyFsm(m_Fsm);
            m_ShuffledGoods.Clear();
            m_Animated.gameObject.SetActive(false);
            base.OnClose(userData);
        }

        // Called by NGUI via reflection.
        public void OnClickWholeScreenButton()
        {
            (m_Fsm.CurrentState as StateBase).OnClickWholeScreenButton(m_Fsm);
        }

        protected override bool BackButtonIsAvailable
        {
            get
            {
                return false;
            }
        }

        private void ShuffleGoods()
        {
            var goodsDataCount = m_ShuffledGoods.Count;
            for (int i = 0; i < goodsDataCount - 1; ++i)
            {
                int j = Random.Range(i + 1, m_ShuffledGoods.Count);
                var tmp = m_ShuffledGoods[i];
                m_ShuffledGoods[i] = m_ShuffledGoods[j];
                m_ShuffledGoods[j] = tmp;
            }
        }

        private void PreloadNextAtlas()
        {
            int nextIndex = m_CurrentIndex + 1;

            if (nextIndex >= m_ShuffledGoods.Count)
            {
                return;
            }

            int typeId = m_ShuffledGoods[nextIndex].Key;
            int iconId = GeneralItemUtility.GetGeneralItemIconId(typeId);

            string atlasName, spriteName;
            if (!UIUtility.TryGetAtlasAndSpriteName(iconId, out atlasName, out spriteName))
            {
                return;
            }

            GameEntry.NGUIAtlas.LoadAtlas(atlasName);
        }

        private class StateBase : FsmState<ReceiveItemForm>
        {
            protected IFsm<ReceiveItemForm> m_Fsm = null;
            protected ReceiveItemForm m_Owner = null;

            protected override void OnInit(IFsm<ReceiveItemForm> fsm)
            {
                base.OnInit(fsm);
                m_Fsm = fsm;
                m_Owner = fsm.Owner;
            }

            public virtual void OnClickWholeScreenButton(IFsm<ReceiveItemForm> fsm)
            {

            }

            protected void Next(IFsm<ReceiveItemForm> fsm)
            {
                if (m_Owner.m_CurrentIndex + 1 >= m_Owner.m_ShuffledGoods.Count)
                {
                    m_Owner.CloseSelf();
                    return;
                }
                m_Owner.m_CurrentIndex++;
                ChangeState<StateLoading>(fsm);
            }
        }

        private class StateInit : StateBase
        {
            protected override void OnEnter(IFsm<ReceiveItemForm> fsm)
            {
                base.OnEnter(fsm);
                m_Owner.ShuffleGoods();
                m_Owner.m_Animated.gameObject.SetActive(false);
                m_Owner.m_CurrentIndex = 0;
                m_Owner.m_CurrentDisplayTime = 0f;
                ChangeState<StateLoading>(fsm);
            }
        }

        private class StateLoading : StateBase
        {
            private bool m_ShouldCloseOwner = false;

            protected override void OnEnter(IFsm<ReceiveItemForm> fsm)
            {
                base.OnEnter(fsm);
                m_Owner.m_CurrentDisplayTime = 0f;
                m_Owner.m_EffectsController.Pause();
                if (m_Owner.m_ShuffledGoods.Count <= 0)
                {
                    m_ShouldCloseOwner = true;
                    return;
                }

                m_ShouldCloseOwner = false;
                int typeId = m_Owner.m_ShuffledGoods[m_Owner.m_CurrentIndex].Key;
                m_Owner.m_Item.InitGeneralItem(typeId, m_Owner.m_ShuffledGoods[m_Owner.m_CurrentIndex].Value);
                ChangeState<StateAnimating>(m_Fsm);
            }

            protected override void OnUpdate(IFsm<ReceiveItemForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);

                if (m_ShouldCloseOwner)
                {
                    m_Owner.CloseSelf(true);
                }
            }
        }

        private class StateAnimating : StateBase
        {
            protected override void OnEnter(IFsm<ReceiveItemForm> fsm)
            {
                base.OnEnter(fsm);
                m_Owner.m_EffectsController.Resume();
                m_Owner.m_CurrentDisplayTime = 0f;
                var goodsNameKey = GeneralItemUtility.GetGeneralItemName(m_Owner.m_ShuffledGoods[m_Owner.m_CurrentIndex].Key);
                m_Owner.m_Text.color = ColorUtility.GetColorForQuality(GeneralItemUtility.GetGeneralItemQuality(m_Owner.m_ShuffledGoods[m_Owner.m_CurrentIndex].Key));
                m_Owner.m_Text.text = GameEntry.Localization.GetString("UI_TEXT_ITEMNAME_COUNT",
                    string.IsNullOrEmpty(goodsNameKey) ? string.Empty : GameEntry.Localization.GetString(goodsNameKey));
                m_Owner.m_Animated.gameObject.SetActive(true);
                m_Owner.m_GeneralItemAnimation.Rewind();
                m_Owner.m_GeneralItemAnimation.Play();
            }

            protected override void OnUpdate(IFsm<ReceiveItemForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                m_Owner.m_CurrentDisplayTime += realElapseSeconds;

                if (!m_Owner.m_GeneralItemAnimation.isPlaying)
                {
                    ChangeState<StateIdle>(fsm);
                }
            }

            public override void OnClickWholeScreenButton(IFsm<ReceiveItemForm> fsm)
            {
                if (m_Owner.m_CurrentDisplayTime <= m_Owner.m_MinDisplayTime)
                {
                    return;
                }

                Next(fsm);
            }
        }

        private class StateIdle : StateBase
        {
            bool m_CanNext = false;
            protected override void OnEnter(IFsm<ReceiveItemForm> fsm)
            {
                base.OnEnter(fsm);
                m_Owner.m_CurrentDisplayTime = 0;
                m_CanNext = false;
            }

            protected override void OnUpdate(IFsm<ReceiveItemForm> fsm, float elapseSeconds, float realElapseSeconds)
            {
                base.OnUpdate(fsm, elapseSeconds, realElapseSeconds);
                if (m_CanNext)
                {
                    return;
                }
                m_Owner.m_CurrentDisplayTime += realElapseSeconds;
                if (m_Owner.m_CurrentDisplayTime > m_Owner.m_MinDisplayTime)
                {
                    m_CanNext = true;
                }
            }

            public override void OnClickWholeScreenButton(IFsm<ReceiveItemForm> fsm)
            {
                if (!m_CanNext)
                {
                    return;
                }

                Next(fsm);
            }
        }
    }
}
