using GameFramework;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CostConfirmDialog
    {
        private abstract class StrategyBase
        {
            protected CostConfirmDialog m_Form = null;
            protected const string CoinIconName = "icon_gold";
            protected const string MoneyIconName = "icon_diamond";
            protected const string EnergyName = "icon_energy";

            public readonly string[] IconNames = new string[]
            {
                "",
                EnergyName,
                MoneyIconName,
                CoinIconName,
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
                "",
            };

            public virtual void Init(CostConfirmDialog form)
            {
                m_Form = form;
                m_Form.m_Title.text = GameEntry.Localization.GetString("UI_BUTTON_BUYTIMES");
            }

            public virtual void RefreshData()
            {

            }

            public virtual void OnClickBuyButton()
            {
                if (m_Form.m_Data.OnClickConfirm != null)
                {
                    m_Form.m_Data.OnClickConfirm(m_Form.m_Data.UserData);
                }
            }

            public void Shutdown()
            {
                m_Form = null;
            }
        }

        private static StrategyBase CreateStrategy(CostConfirmDialogType scenario)
        {
            switch (scenario)
            {
                case CostConfirmDialogType.Coin:
                    return new StrategyCoin();
                case CostConfirmDialogType.Energy:
                    return new StrategyEnergy();
                case CostConfirmDialogType.ArenaBattleCount:
                    return new StrategyArenaCount();
                case CostConfirmDialogType.Other:
                    return new StrategyOther();
                default:
                    Log.Error("Scenario {0} not supported.", scenario);
                    return null;
            }
        }
    }
}
