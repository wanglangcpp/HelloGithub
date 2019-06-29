using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class HeroTeamEpigraphItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_Select = null;

        [SerializeField]
        private GameObject m_Add = null;

        [SerializeField]
        private GameObject m_Lock = null;

        [SerializeField]
        private GameObject m_CanUnLock = null;

        [SerializeField]
        private UISprite m_Icon = null;

        [SerializeField]
        private UILabel m_LockLevel = null;

        public int Id { set; get; }

        private int m_Index = -1;

        private Action<int> m_OnClickIconReturn = null;

        public void SetUndress(bool undress, EpigraphData data, int index, Action<int> onClickIconReturn)
        {
            SetSelect(false);
            m_OnClickIconReturn = onClickIconReturn;
            m_Index = index;
            m_Lock.SetActive(false);
            m_CanUnLock.SetActive(false);
            m_Add.SetActive(!undress);
            m_Icon.gameObject.SetActive(undress);
            if (undress)
            {
                int iconId = GeneralItemUtility.GetGeneralItemIconId(data.Id);
                m_Icon.LoadAsync(iconId);
            }
        }

        public void SetLock(int unlockLevel)
        {
            SetSelect(false);
            m_Lock.SetActive(true);
            m_CanUnLock.SetActive(false);
            m_Add.SetActive(false);
            m_Icon.gameObject.SetActive(false);
            m_LockLevel.text = GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", unlockLevel);
        }

        public void SetCanUnLock()
        {
            SetSelect(false);
            m_Lock.SetActive(false);
            m_CanUnLock.SetActive(true);
            m_Add.SetActive(false);
            m_Icon.gameObject.SetActive(false);
        }

        public void SetSelect(bool select)
        {
            m_Select.SetActive(select);
        }

        public void OnClickUnLock()
        {
            if (GameEntry.OfflineMode.OfflineModeEnabled)
            {
                return;
            }
//             int count = GameEntry.Data.EpigraphSlots.Data.Count;
//             int nextSlotlevel = GameEntry.ServerConfig.GetInt(string.Format(Constant.ServerConfig.EpigraphRequiredLevel, count.ToString()));
//             if (nextSlotlevel <= GameEntry.Data.Player.Level)
//             {
//                 GameEntry.Network.Send(new CLUnlockEpigraph());
//             }
        }

        public void OnClickIcon()
        {
            if (m_OnClickIconReturn != null)
            {
                m_OnClickIconReturn(m_Index);
            }
        }

        public void OnClickAdd()
        {
            GameEntry.UI.OpenUIForm(UIFormId.InventoryForm, new InventoryDisplayData
            {
                InventoryType = InventoryForm.InventoryType.EpigraphDress,
                EpigraphIndex = m_Index,
            });
        }
    }
}
