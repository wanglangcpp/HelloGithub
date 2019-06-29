using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// Boss关卡
    /// </summary>
    [RequireComponent(typeof(UIPanel))]
    [SerializeField]
    public class BossItem : MonoBehaviour
    {
        [SerializeField]
        private int currentIndex = 0;
        [SerializeField]
        private bool isChanging = false;
        public UITexture m_BossIcon = null;
        public UILabel m_BossName = null;
        public GameObject[] m_StarList = null;
        public UILabel m_OpenLevel = null;
        public UISprite m_LockIcon = null;
        [SerializeField]
        private GameObject m_Star = null;

        private UIPanel panel;
        public bool IsChanging { get { return isChanging; } private set { isChanging = value; } }
        public int CurrentIndex { get { return currentIndex; } set { currentIndex = value; } }
        public DRInstance CurrentData { get; set; }
        private int textureId = 0;
        void Awake()
        {
            panel = GetComponent<UIPanel>();
        }

        public void SetData(DRInstance drInstance, int textureId)
        {
            CurrentData = drInstance;
            this.textureId = textureId;
            RefreshShow();
        }

        void RefreshShow()
        {
            //DRNpc npc = GameEntry.DataTable.GetDataTable<DRNpc>().GetDataRow(CurrentData.NpcId);
            m_BossIcon.LoadAsync(textureId);
            //m_BossIcon.LoadAsync(UIUtility.GetHeroBigPortraitTextureId(npc.Id));
            //m_BossName.text = npc.Name;
            //m_OpenLevel.text = CurrentData.PrerequisitePlayerLevel.ToString();
            m_BossName.text = GameEntry.Localization.GetString(CurrentData.Name);
            m_OpenLevel.text = GameEntry.Localization.GetString("UI_TEXT_BOSS_OPENLEVEL", CurrentData.PrerequisitePlayerLevel);
            m_LockIcon.gameObject.SetActive(GameEntry.Data.Player.Level < CurrentData.PrerequisitePlayerLevel);
            m_Star.SetActive(GameEntry.Data.Player.Level >= CurrentData.PrerequisitePlayerLevel);
            for (int i = 0; i < m_StarList.Length; i++)
            {
                if (i < GameEntry.Data.InstanceForBossData.GetInstanceInstanceStarCount(CurrentData.Id))
                    m_StarList[i].SetActive(true);
                else
                    m_StarList[i].SetActive(false);
            }

        }
        /// <summary>
        /// 修改显示的位置
        /// </summary>
        /// <param name="newIndex"></param>
        internal void ChangeShowIndex(int targetIndex, float duration, Vector2 targetPos, float targetAlpha, Vector3 targetScale, int depth)
        {
            currentIndex = targetIndex;
            panel.depth = depth;
            IsChanging = true;
            TweenPosition.Begin(gameObject, duration, targetPos, false).onFinished = new List<EventDelegate>() { new EventDelegate(() => { IsChanging = false; }) };
            TweenAlpha.Begin(gameObject, duration, targetAlpha);
            TweenScale.Begin(gameObject, duration, targetScale);

        }
    }
}

