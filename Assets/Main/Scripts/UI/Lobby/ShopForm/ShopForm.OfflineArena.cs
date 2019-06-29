using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ShopForm
    {
        private class OfflineArena : StrategyCommon
        {
            protected override ShopScenario ShopType
            {
                get
                {
                    return ShopScenario.OfflineArena;
                }
            }

            public override void Init(ShopForm form)
            {
                base.Init(form);
                m_Form.GetComponent<UITitle>().ShowArenaToken = true;
            }

            public override void ShutDown()
            {
                m_Form.GetComponent<UITitle>().ShowArenaToken = false;
                base.ShutDown();
            }
        }
    }
}
