using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class RewardChest : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_OpenIconObject = null;

        [SerializeField]
        private GameObject m_CloseIconObject = null;

        [SerializeField]
        private UILabel m_CountLabel = null;

        [SerializeField]
        private UIEffectsController m_Effects = null;

        private const string OpenEffectName = "effect_ui_baoxiang_01";
        private const string PickEffectName = "effect_ui_baoxiang_02";

        private List<DRInstanceGroup.Reward> m_Rewards = null;
        private int m_Count = default(int);
        private int m_ChestIndex = default(int);
        private DRInstanceGroup m_ChapterConfig = null;
        private bool m_EnabelPickReward = false;

        public void SetBoxData(DRInstanceGroup chapterConfig, int chestIndex, int currentCount)
        {
            m_ChestIndex = chestIndex;
            m_ChapterConfig = chapterConfig;

            if (!InitChestData())
                return;

            m_EnabelPickReward = currentCount >= m_Count;

            bool isPicked = GameEntry.Data.InstanceGroups.GetChestStatus(chapterConfig.Id, chestIndex);
            m_OpenIconObject.SetActive(m_EnabelPickReward && isPicked);
            m_CloseIconObject.SetActive(!isPicked);

            m_Effects.Pause();
            if (m_EnabelPickReward && !isPicked)
            {
                if (m_Effects.HasEffect(OpenEffectName))
                    m_Effects.Resume();
                else
                    m_Effects.ShowEffect(OpenEffectName);
            }
            m_CountLabel.text = m_Count.ToString();

            UIEventListener.Get(gameObject).onClick = OnBoxClicked;
        }

        public void PauseEffect()
        {
            m_Effects.Pause();
        }

        private bool InitChestData()
        {
            switch (m_ChestIndex)
            {
                case 0:
                    m_Rewards = m_ChapterConfig.Chest1Rewards;
                    m_Count = m_ChapterConfig.Chest1NeedStar;
                    return true;
                case 1:
                    m_Rewards = m_ChapterConfig.Chest2Rewards;
                    m_Count = m_ChapterConfig.Chest2NeedStar;
                    return true;
                case 2:
                    m_Rewards = m_ChapterConfig.Chest3Rewards;
                    m_Count = m_ChapterConfig.Chest3NeedStar;
                    return true;
                default:
                    Log.Error("The parameter of chestIndex is invalid.");
                    return false;
            }
        }

        private void OnBoxClicked(GameObject go)
        {
            bool isPicked = GameEntry.Data.InstanceGroups.GetChestStatus(m_ChapterConfig.Id, m_ChestIndex);
            if (m_EnabelPickReward && !isPicked)
            {
                m_Effects.Resume();
                m_Effects.ShowEffect(PickEffectName);
                GameEntry.LobbyLogic.PickUpChestReward(m_ChapterConfig.Id, m_ChestIndex);
            }
            else
            {
                string title = GameEntry.Localization.GetString("UI_TEXT_PROMPT");
                string instruction = GameEntry.Localization.GetString("UI_TEXT_INSTANCE_ON_CONDITION", m_Count);

                List<ChestRewardDisplayData.Reward> rewards = new List<ChestRewardDisplayData.Reward>();

                for (int i = 0; i < m_Rewards.Count; i++)
                    rewards.Add(new ChestRewardDisplayData.Reward(m_Rewards[i].Id, m_Rewards[i].Count));

                GameEntry.UI.OpenUIForm(UIFormId.ChestRewardSubForm, new ChestRewardDisplayData(rewards, title, instruction));
            }
        }
    }
}