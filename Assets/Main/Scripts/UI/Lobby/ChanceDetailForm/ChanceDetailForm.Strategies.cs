using GameFramework;
using GameFramework.DataTable;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ChanceDetailForm
    {
        private static StrategyBase CreateStrategy(ChanceType chanceType)
        {
            switch (chanceType)
            {
                case ChanceType.Coin:
                    return new StrategyCoin();
                case ChanceType.Money:
                    return new StrategyMoney();
                default:
                    Log.Error("Chance type {0} is unsupported.", chanceType.ToString());
                    return null;
            }
        }

        private abstract class StrategyBase
        {
            protected internal IDataTable<DRChanceCost> m_DRChanceCost = null;
            protected internal ChanceDetailForm m_Form = null;

            internal void Init(ChanceDetailForm form)
            {
                m_Form = form;
                m_DRChanceCost = GameEntry.DataTable.GetDataTable<DRChanceCost>();
            }

            internal abstract GameObject CardTemplate { get; }

            internal abstract void InitCurrencyIcons();

            internal abstract int BuyAllCost { get; }

            internal abstract int BuyOneCost { get; }

            internal abstract string SpendCurrencyNoticeKey { get; }

            internal abstract CurrencyType CurrencyType { get; }
        }

        private class StrategyCoin : StrategyBase
        {
            internal override int BuyAllCost
            {
                get
                {
                    int openedCount = m_Form.m_CurrentChanceData.ChancedCount;
                    if (openedCount == Constant.MaxChancedCardCount)
                    {
                        return 0;
                    }
                    DRChanceCost drChanceCost = m_Form.m_CurrentChanceData.ChanceCost[openedCount];
                    if (drChanceCost == null)
                    {
                        return 0;
                    }

                    return drChanceCost.CostAll;
                }
            }

            internal override int BuyOneCost
            {
                get
                {
                    int openedCount = m_Form.m_CurrentChanceData.ChancedCount;
                    if (openedCount == Constant.MaxChancedCardCount)
                    {
                        return 0;
                    }
                    DRChanceCost drChanceCost = m_Form.m_CurrentChanceData.ChanceCost[openedCount];
                    if (drChanceCost == null)
                    {
                        return 0;
                    }

                    return drChanceCost.Cost;
                }
            }

            internal override GameObject CardTemplate
            {
                get
                {
                    return m_Form.m_CoinCardTemplate;
                }
            }

            internal override CurrencyType CurrencyType
            {
                get
                {
                    return CurrencyType.Coin;
                }
            }

            internal override string SpendCurrencyNoticeKey
            {
                get
                {
                    return "UI_TEXT_SPEND_COIN_NOTICE";
                }
            }

            internal override void InitCurrencyIcons()
            {
                for (int i = 0; i < m_Form.m_CoinIcons.Length; i++)
                {
                    m_Form.m_CoinIcons[i].enabled = true;
                }

                for (int i = 0; i < m_Form.m_MoneyIcons.Length; i++)
                {
                    m_Form.m_MoneyIcons[i].enabled = false;
                }
            }
        }

        private class StrategyMoney : StrategyBase
        {
            internal override int BuyAllCost
            {
                get
                {
                    int openedCount = m_Form.m_CurrentChanceData.ChancedCount;
                    if (openedCount == Constant.MaxChancedCardCount)
                    {
                        return 0;
                    }
                    DRChanceCost drChanceCost = m_Form.m_CurrentChanceData.ChanceCost[openedCount];
                    if (drChanceCost == null)
                    {
                        return 0;
                    }

                    return drChanceCost.CostAll;
                }
            }

            internal override int BuyOneCost
            {
                get
                {
                    int openedCount = m_Form.m_CurrentChanceData.ChancedCount;
                    if (openedCount == Constant.MaxChancedCardCount)
                    {
                        return 0;
                    }
                    DRChanceCost drChanceCost = m_Form.m_CurrentChanceData.ChanceCost[openedCount];
                    if (drChanceCost == null)
                    {
                        return 0;
                    }

                    return drChanceCost.Cost;
                }
            }

            internal override GameObject CardTemplate
            {
                get
                {
                    return m_Form.m_MoneyCardTemplate;
                }
            }

            internal override CurrencyType CurrencyType
            {
                get
                {
                    return CurrencyType.Money;
                }
            }

            internal override string SpendCurrencyNoticeKey
            {
                get
                {
                    return "UI_TEXT_SPEND_MONEY_NOTICE";
                }
            }

            internal override void InitCurrencyIcons()
            {
                for (int i = 0; i < m_Form.m_CoinIcons.Length; i++)
                {
                    m_Form.m_CoinIcons[i].enabled = false;
                }

                for (int i = 0; i < m_Form.m_MoneyIcons.Length; i++)
                {
                    m_Form.m_MoneyIcons[i].enabled = true;
                }
            }
        }
    }
}
