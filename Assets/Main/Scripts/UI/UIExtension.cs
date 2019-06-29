using GameFramework;
using GameFramework.DataTable;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    public static class UIExtension
    {
        public static bool HasUIForm(this UIComponent uiComponent, UIFormId uiFormId, string uiGroup = "Default")
        {
            return uiComponent.HasUIForm((int)uiFormId, uiGroup);
        }

        public static UIForm GetUIForm(this UIComponent uiComponent, UIFormId uiFormId, string uiGroup = "Default")
        {
            return uiComponent.GetUIForm((int)uiFormId, uiGroup);
        }

        public static UIForm[] GetUIForms(this UIComponent uiComponent, UIFormId uiFormId, string uiGroup = "Default")
        {
            return uiComponent.GetUIForms((int)uiFormId, uiGroup);
        }

        public static void OpenUIForm(this UIComponent uiComponent, UIFormId uiFormId, UIFormBaseUserData userData = null)
        {
            uiComponent.OpenUIForm((int)uiFormId, userData);
        }

        public static void OpenUIForm(this UIComponent uiComponent, int uiFormId, UIFormBaseUserData userData = null)
        {
            if (uiFormId == (int)UIFormId.Dialog && NeedUseNativeDialog)
            {
                OpenNativeDialog(userData as DialogDisplayData);
                return;
            }

            IDataTable<DRUIForm> dtUIForm = GameEntry.DataTable.GetDataTable<DRUIForm>();
            DRUIForm dataRow = dtUIForm.GetDataRow(uiFormId);
            if (dataRow == null)
            {
                Log.Warning("Can not load UI form '{0}' from data table.", uiFormId.ToString());
                return;
            }

            GameEntry.Waiting.StartWaiting(WaitingType.Default, ((UIFormId)uiFormId).ToString());
            uiComponent.OpenUIForm(uiFormId, AssetUtility.GetUIFormAsset(dataRow.ResourceName), dataRow.UIGroupName, dataRow.PauseCoveredUIForm, userData);
        }

        /// <summary>
        /// 打开物品详情界面。
        /// </summary>
        /// <param name="uiComponent">界面组件。</param>
        /// <param name="userData">显示用的数据。</param>
        public static void OpenItemInfoForm(this UIComponent uiComponent, GeneralItemInfoDisplayData userData)
        {
            if (userData == null)
            {
                Log.Error("Invalid user data.");
                return;
            }

            var itemTypeId = userData.TypeId;
            if (itemTypeId <= 0)
            {
                Log.Error("Item type id '{0}' should be greater than zero.", itemTypeId.ToString());
                return;
            }

            var dtItem = GameEntry.DataTable.GetDataTable<DRItem>();
            DRItem drItem = dtItem.GetDataRow(itemTypeId);

            if (drItem != null && drItem.Type == (int)ItemType.HeroPieceItem)
            {
                GameEntry.UI.OpenUIForm(UIFormId.GeneralItemInfoForm2_WithWhereToGet, userData);
            }
            else if (itemTypeId >= Constant.GeneralItem.MinHeroQualityItemId && itemTypeId <= Constant.GeneralItem.MaxHeroQualityItemId)
            {
                GameEntry.UI.OpenUIForm(UIFormId.GeneralItemInfoForm2_WithWhereToGet, userData);
            }
            else
            {
                GameEntry.UI.OpenUIForm(UIFormId.GeneralItemInfoForm2, userData);
            }
        }

        private static void OpenNativeDialog(DialogDisplayData dialogData)
        {
            var nativeCallback = GameEntry.NativeCallback;
            NativeDialog.Open(dialogData.Mode, dialogData.Title, dialogData.Message, dialogData.PauseGame, dialogData.ConfirmText, dialogData.CancelText, dialogData.OtherText,
                dialogData.OnClickConfirm == null ? -1 : nativeCallback.AddPreregisteredCallback(delegate () { dialogData.OnClickConfirm(null); }),
                dialogData.OnClickCancel == null ? -1 : nativeCallback.AddPreregisteredCallback(delegate () { dialogData.OnClickCancel(null); }),
                dialogData.OnClickOther == null ? -1 : nativeCallback.AddPreregisteredCallback(delegate () { dialogData.OnClickOther(null); }),
                dialogData.UserData as string);
        }

        private static bool NeedUseNativeDialog
        {
            get
            {
                if (!GameEntry.IsAvailable)
                {
                    return true;
                }

                return (GameEntry.Procedure.CurrentProcedure as ProcedureBase).UseNativeDialog;
            }
        }
    }
}
