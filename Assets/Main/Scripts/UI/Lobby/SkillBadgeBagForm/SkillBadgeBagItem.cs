using UnityEngine;
using System.Collections;
using System;
using GameFramework;

namespace Genesis.GameClient
{
    public class SkillBadgeBagItem : MonoBehaviour
    {
        [SerializeField]
        private UISprite m_BadgeSelectSprite = null;

        [SerializeField]
        private GeneralItemView m_GeneralItemView = null;

        private GameFrameworkAction<SkillBadgeBagItem> m_OnClickSelf;

        private bool m_IsSelected = false;

        public bool IsSelected
        {
            get { return m_IsSelected; }
            set
            {
                m_IsSelected = value;
                m_BadgeSelectSprite.gameObject.SetActive(m_IsSelected);
            }
        }

        private int m_BadgeId = 0;

        public int BadgeId
        {
            get { return m_BadgeId; }
        }

        private int m_BadgeCount = 0;

        public int BadgeCount
        {
            get { return m_BadgeCount; }
        }

        public void RefreshData(int badgeId, int count, GameFrameworkAction<SkillBadgeBagItem> onClickSelf)
        {
            m_BadgeId = badgeId;
            m_BadgeCount = count;
            m_OnClickSelf = onClickSelf;
            IsSelected = false;
            m_GeneralItemView.InitSkillBadge(badgeId, count);
        }

        public void OnClickSelf()
        {
            m_OnClickSelf(this);
        }
    }
}