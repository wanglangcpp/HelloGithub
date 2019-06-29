using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ImpactComponent : MonoBehaviour
    {
        private abstract class ImpactBase
        {
            public abstract int Type
            {
                get;
            }

            public abstract BaseApplyImpactData PerformImpact(BasePerformImpactData impactData);

            public abstract void ApplyImpact(BaseApplyImpactData impactData);

            public abstract void FillPacket(CREntityImpact packet, BaseApplyImpactData impactData);

            protected static Vector3 GetRepulseDirection(ICampable origin, ITargetable target, int repulseType)
            {
                Entity originEntity = origin as Entity;
                Entity targetEntity = target as Entity;
                Vector3 repulseDirection = Vector3.zero;
                switch (repulseType)
                {
                    case 0:
                        repulseDirection = targetEntity.CachedTransform.localPosition - originEntity.CachedTransform.localPosition;
                        break;
                    case 1:
                        repulseDirection = originEntity.CachedTransform.TransformDirection(Vector3.forward);
                        break;
                    default:
                        Log.Warning("Unknown repulse type '{0}'.", repulseType.ToString());
                        break;
                }

                repulseDirection.y = 0f;
                repulseDirection = repulseDirection.normalized;

                return repulseDirection;
            }

            /// <summary>
            /// 是否应该无视状态伤害。
            /// </summary>
            /// <param name="impactData">伤害数据。</param>
            /// <param name="targetCharacter">目标角色。</param>
            /// <returns>是否应该无视状态伤害。</returns>
            protected static bool ShouldIgnoreStateImpact(BasePerformImpactData impactData, Character targetCharacter)
            {
                if (targetCharacter == null)
                {
                    return true;
                }

                if (impactData.DataRow.IgnoreSteady)
                {
                    return targetCharacter.HasStateHarmFreeAdvancedBuff;
                }

                return targetCharacter.HasStateHarmFreeBuff;
            }
        }
    }
}
