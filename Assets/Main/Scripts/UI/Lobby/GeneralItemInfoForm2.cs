using GameFramework;
using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 物品信息界面。
    /// </summary>
    public class GeneralItemInfoForm2 : NGUIForm
    {
        [SerializeField]
        private GeneralItemView m_ItemView = null;

        [SerializeField]
        private UILabel m_ItemNameLbl = null;

        [SerializeField]
        private UILabel m_ItemDescLbl = null;

        [SerializeField]
        private UILabel m_ItemWhereToGetText = null;

        private GeneralItemInfoDisplayData m_CachedDisplayData = null;

        #region NGUIForm

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            m_CachedDisplayData = userData as GeneralItemInfoDisplayData;

            var typeId = m_CachedDisplayData.TypeId;
            if (typeId < 0 || (typeId >= Constant.GeneralItem.MinHeroQualityItemId && typeId <= Constant.GeneralItem.MaxHeroQualityItemId))
            {
                Log.Error("Item type ID '{0}' is invalid.", typeId.ToString());
                return;
            }

            var itemDT = GameEntry.DataTable.GetDataTable<DRItem>();

            DRItem itemDR = itemDT.GetDataRow(typeId);
            if (itemDR == null)
            {
                Log.Error("Item type ID '{0}' not found.", typeId.ToString());
                return;
            }

            if (itemDR.Type == (int)ItemType.HeroPieceItem)
            {
                Log.Error("Item '{0}' is hero piece, and shouldn't be displayed in this form.", typeId.ToString());
                return;
            }

            m_ItemView.InitItem(typeId, (QualityType)itemDR.Quality);
            m_ItemNameLbl.text = GameEntry.Localization.GetString(itemDR.Name);
            m_ItemDescLbl.text = GameEntry.Localization.GetString(itemDR.Description);

            var itemWhereToGetDT = GameEntry.DataTable.GetDataTable<DRGeneralItemWhereToGet>();
            var whereToGetTextKey = string.Empty;
            DRGeneralItemWhereToGet itemWhereToGetDR = itemWhereToGetDT.GetDataRow(typeId);
            if (itemWhereToGetDR != null)
            {
                whereToGetTextKey = itemWhereToGetDR.WhereToGetText;
            }

            m_ItemWhereToGetText.text = string.IsNullOrEmpty(whereToGetTextKey) ? string.Empty : GameEntry.Localization.GetString(whereToGetTextKey);
        }

        protected override void OnClose(object userData)
        {
            m_CachedDisplayData = null;
            base.OnClose(userData);
        }

        #endregion NGUIForm
    }
}
