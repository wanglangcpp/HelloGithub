using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class CleanOutResultForm : NGUIForm
    {
        [SerializeField]
        private SweepPreview m_SweepPreviewRoot = null;

        [SerializeField]
        private SweepResult m_SweepResultRoot = null;

        private SweepDisplayData m_DisplayData = null;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            var displayData = userData as SweepDisplayData;
            if(displayData == null)
            {
                Log.Error("Error parameters for sweep form, please check in.");
                return;
            }

            m_DisplayData = displayData;

            m_SweepResultRoot.Initialize();
            m_SweepPreviewRoot.Initialize();

            m_SweepResultRoot.OnHideAction = () => { CloseSelf(); };
            m_SweepPreviewRoot.Show(displayData);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        public void OnSweepButtonClick()
        {
            var levelData = GameEntry.Data.InstanceGroups.GetLevelById(m_DisplayData.LevelId);

            // 副本得到的星数小于3则提示是否打该副本
            if (levelData.StarCount < 3)
            {
                GameEntry.UI.OpenUIForm(UIFormId.Dialog, new DialogDisplayData
                {
                    Mode = 2,
                    Message = GameEntry.Localization.GetString("UI_TEXT_STAR_INSUFFICIENT"),
                    OnClickConfirm = (o) =>
                    {
                        GameEntry.UI.OpenUIForm(UIFormId.InstanceInfoForm, new InstanceInfoDisplayData { InstanceId = m_DisplayData.LevelId });
                        CloseSelf(true);
                    },
                    OnClickCancel = (o) => { CloseSelf(); },
                });

                return;
            }

            // 体力是否满足条件
            int sweepOnceCostEnergy = 6; // GameEntry.ServerConfig.GetInt(Constant.ServerConfig.EnergyPerInstance, 6);
            int needEnergy = m_SweepPreviewRoot.IsAutoSweep ? sweepOnceCostEnergy * m_DisplayData.MaxSweepCount : sweepOnceCostEnergy;

            if (!UIUtility.CheckEnergy(needEnergy))
                return;

            OnSweep(m_SweepPreviewRoot.IsAutoSweep);
        }

        private void OnSweep(bool isAutoSweep)
        {
            m_SweepPreviewRoot.Hide();
            m_SweepResultRoot.Show(m_DisplayData, isAutoSweep);
        }

    }
}
