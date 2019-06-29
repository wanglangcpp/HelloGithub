using GameFramework.Fsm;

namespace Genesis.GameClient
{
    public partial class BaseArenaBattleResultForm
    {
        protected class RewardState : StateBase
        {
            protected override void OnInit(IFsm<BaseArenaBattleResultForm> fsm)
            {
                base.OnInit(fsm);
                m_CachedSubPanel = fsm.Owner.m_RewardSubPanel;
                m_LastSubPanel = fsm.Owner.m_RankSubPanel;
                fsm.Owner.m_ArenaTokenObtained.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", fsm.Owner.m_UserData.ArenaTokenObtained);
                fsm.Owner.m_ArenaCoinObtained.text = GameEntry.Localization.GetString("UI_TEXT_PLUS", fsm.Owner.m_UserData.ArenaCoinObtained);

                for (int i = 0; i < fsm.Owner.m_ItemsObtained.Length; i++)
                {
                    var itemEarned = fsm.Owner.m_ItemsObtained[i];
                    if (i >= fsm.Owner.m_UserData.ArenaItemsObtained.Count)
                    {
                        itemEarned.gameObject.SetActive(false);
                        continue;
                    }

                    itemEarned.gameObject.SetActive(true);
                    var itemData = fsm.Owner.m_UserData.ArenaItemsObtained[i];
                    itemEarned.InitGeneralItem(itemData.Type, itemData.Count);
                }
            }

            protected override void OnEnter(IFsm<BaseArenaBattleResultForm> fsm)
            {
                base.OnEnter(fsm);
            }

            public override void OnClickWholeScreenButton(IFsm<BaseArenaBattleResultForm> fsm)
            {
                CachedAnimation.Stop();
                CachedAnimation[InwardClipName].clip.SampleAnimation(m_CachedSubPanel.gameObject, 1f);
                m_LastSubPanel.gameObject.SetActive(false);
            }
        }
    }
}
