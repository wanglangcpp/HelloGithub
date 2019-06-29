using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ImpactComponent
    {
        private class HPRecoverImpact : ImpactBase
        {
            public override int Type
            {
                get
                {
                    return (int)ImpactType.HPRecover;
                }
            }

            public override BaseApplyImpactData PerformImpact(BasePerformImpactData impactData)
            {
                OtherPerformImpactData data = impactData as OtherPerformImpactData;
                if (data == null)
                {
                    Log.Error("HPRecoverImpact's PerformImpact's data is nulls.");
                    return null;
                }

                var targetData = data.TargetData as TargetableObjectData;
                if (targetData.IsDead)
                {
                    return null;
                }

                IImpactDataProvider originImpactData = data.OriginData as IImpactDataProvider;
                int hpRecoverFromDirect = Mathf.RoundToInt(data.ImpactParams[2]);
                int hpRecoverFromTarget = Mathf.RoundToInt(targetData.MaxHP * data.ImpactParams[3]);
                int hpRecoverFromPhysicalAttack = originImpactData == null ? 0 : Mathf.RoundToInt(originImpactData.PhysicalAttack * data.ImpactParams[4]);
                int hpRecoverFromMagicAttack = originImpactData == null ? 0 : Mathf.RoundToInt(originImpactData.MagicAttack * data.ImpactParams[5]);
                int hpRecoverTotal = hpRecoverFromDirect + hpRecoverFromTarget + hpRecoverFromPhysicalAttack + hpRecoverFromMagicAttack;
                if (hpRecoverTotal <= 0)
                {
                    return null;
                }

                var applyImpactData = BaseApplyImpactData.Create<ApplyHPRecoverImpactData>(data.OriginData, data.TargetData, impactData.SourceType, impactData.DataRow.Id);
                applyImpactData.RecoverHP = hpRecoverTotal;

                return applyImpactData;
            }

            public override void ApplyImpact(BaseApplyImpactData impactData)
            {
                var data = impactData as ApplyHPRecoverImpactData;
                if (data == null)
                {
                    Log.Error("ApplyHPRecoverImpactData is invalid.");
                    return;
                }

                var targetData = impactData.TargetData as TargetableObjectData;
                if (targetData == null)
                {
                    Log.Error("Targetable object data is invalid.");
                    return;
                }

                var isMeAttackOpp = AIUtility.GetRelation(GameEntry.SceneLogic.BaseInstanceLogic.MyCamp, targetData.Camp) != RelationType.Friendly;

                targetData.HP += data.RecoverHP;
                if (targetData.HP > targetData.MaxHP)
                {
                    targetData.HP = targetData.MaxHP;
                }

                var target = targetData.Entity;
                if (target != null && target.Motion != null)
                {
                    if (data.RecoverHP > 0)
                    {
                        int hudTextType = Mathf.RoundToInt(data.DataRow.ImpactParams[isMeAttackOpp ? 0 : 1]);
                        if (hudTextType >= 0)
                        {
                           // GameEntry.Impact.CreateHudText(hudTextType, target.CachedTransform.position, "+" + data.RecoverHP.ToString(), target);
                        }
                    }
                }
                else
                {
                    Log.Error("Targetable object or motion '{0}' is invalid.", targetData.Id.ToString());
                }
            }

            public override void FillPacket(CREntityImpact packet, BaseApplyImpactData impactData)
            {
                var data = impactData as ApplyHPRecoverImpactData;
                packet.HPRecoverImpacts.Add(new PBHPRecoverImpact
                {
                    ImpactId = data.DataRow.Id,
                    RecoverHP = data.RecoverHP,
                });
            }
        }
    }
}
