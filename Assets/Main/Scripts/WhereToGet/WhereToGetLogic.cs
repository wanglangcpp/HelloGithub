using System.Collections.Generic;

namespace Genesis.GameClient
{
    public abstract class WhereToGetLogic_Base
    {
        public abstract WhereToGetType Type { get; }

        public string TextKey { get; private set; }

        public int IconId { get; private set; }

        public int MixedItemId { get; set; }

        public int NeedItemId { get; set; }

        public virtual bool IsLocked { get { return false; } }

        public virtual bool IsCleanOutInstance { get { return false; } }

        public WhereToGetLogic_Base(string textKey, int iconId)
        {
            TextKey = textKey;
            IconId = iconId;
        }

        public virtual void OnClick() { }
    }

    public class WhereToGetLogic_Text : WhereToGetLogic_Base
    {
        public override WhereToGetType Type
        {
            get
            {
                return WhereToGetType.Text;
            }
        }

        public WhereToGetLogic_Text(string textKey, int iconId) : base(textKey, iconId)
        {

        }
    }

    public class WhereToGetLogic_SinglePlayerInstance : WhereToGetLogic_Base
    {
        public override WhereToGetType Type
        {
            get
            {
                return WhereToGetType.SinglePlayerInstance;
            }
        }

        public int InstanceId { get; private set; }

        public DRWhereToGet WhereToGetConfig { get; private set; }

        private int NeedCount
        {
            get
            {
                var drHeroQualityItem = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>()[MixedItemId];
                var synthItemIds = drHeroQualityItem.GetSynthItemIds();
                var synthItemCounts = drHeroQualityItem.GetSynthItemCounts();

                for (int i = 0; i < synthItemIds.Count; i++)
                {
                    if (synthItemIds[i] == NeedItemId)
                    {
                        var itemInBagInfo = GameEntry.Data.HeroQualityItems.GetData(NeedItemId);
                        if (itemInBagInfo == null)
                            return synthItemCounts[i];
                        else
                            return synthItemCounts[i] - itemInBagInfo.Count;
                    }
                }

                return 0;
            }
        }
        public override bool IsLocked
        {
            get
            {
                return !GameEntry.LobbyLogic.InstanceIsOpen(InstanceId);
            }
        }

        public override bool IsCleanOutInstance
        {
            get
            {
                return true;
            }
        }

        public WhereToGetLogic_SinglePlayerInstance(string textKey, int iconId, int instanceId) : base(textKey, iconId)
        {
            InstanceId = instanceId;
        }

        public WhereToGetLogic_SinglePlayerInstance(DRWhereToGet dr) : base(dr.Params[0], dr.IconId)
        {
            WhereToGetConfig = dr;
            InstanceId = int.Parse(dr.Params[1]);
        }

        public override void OnClick()
        {
            SweepDisplayData displayData = new SweepDisplayData();
            displayData.SetFromWhereToGetData(WhereToGetConfig, NeedItemId, NeedCount);

            GameEntry.UI.OpenUIForm(UIFormId.CleanOutResultForm, displayData);
        }
    }

    public class WhereToGetLogic_UI : WhereToGetLogic_Base
    {
        public override WhereToGetType Type
        {
            get
            {
                return WhereToGetType.UI;
            }
        }

        public UIFormId UIFormId { get; private set; }

        private IList<string> m_CustomParams;

        public WhereToGetLogic_UI(string textKey, int iconId, UIFormId uiFormId, IList<string> customParams) : base(textKey, iconId)
        {
            UIFormId = uiFormId;
            m_CustomParams = customParams;
        }

        public override void OnClick()
        {
            switch (UIFormId)
            {
                case UIFormId.ShopForm:
                    GameEntry.UI.OpenUIForm(UIFormId, new ShopDisplayData { Scenario = (ShopScenario)int.Parse(m_CustomParams[2]) });
                    break;
                case UIFormId.ChanceDetailForm:
                    GameEntry.UI.OpenUIForm(UIFormId, new ChanceDetailDisplayData { ChanceType = (ChanceType)int.Parse(m_CustomParams[2]) });
                    break;
                case UIFormId.CostConfirmDialog:
                    GameEntry.UI.OpenUIForm(UIFormId.CostConfirmDialog, new CostConfirmDialogDisplayData { Mode = (CostConfirmDialogType)int.Parse(m_CustomParams[2]) });
                    break;
                default:
                    GameEntry.UI.OpenUIForm(UIFormId);
                    break;
            }
        }
    }
}
