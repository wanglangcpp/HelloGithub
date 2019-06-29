using System;

namespace Genesis.GameClient
{
    public partial class ImpactComponent
    {
        private class FreezeImpact : ImpactBase
        {
            public override int Type
            {
                get
                {
                    return (int)ImpactType.Freeze;
                }
            }

            public override BaseApplyImpactData PerformImpact(BasePerformImpactData impactData)
            {
                OtherPerformImpactData data = impactData as OtherPerformImpactData;
                Character targetCharacter = data.Target as Character;
                if (targetCharacter == null)
                {
                    return null;
                }

                if (targetCharacter.HasStateHarmFreeAdvancedBuff)
                {
                    return null;
                }

                int index = 0;
                float freezeTime = data.ImpactParams[index++];
                float downTime = data.ImpactParams[index++];

                var applyImpactData = BaseApplyImpactData.Create<ApplyFreezeImpactData>(data.OriginData, data.TargetData, impactData.SourceType, impactData.DataRow.Id);

                float freezeTimeDelta = GetFreezeTimeDeltaByIceBadge(impactData.SkillBadges, impactData.SourceType);
                applyImpactData.FreezeTime = freezeTime + freezeTimeDelta;
                if (targetCharacter.Motion.IsOnGround)
                {
                    applyImpactData.StateCategory = CharacterMotionStateCategory.Ground;
                }
                else if (targetCharacter.Motion.IsInAir)
                {
                    applyImpactData.StateCategory = CharacterMotionStateCategory.Air;
                    applyImpactData.DownTime = downTime;
                }
                return applyImpactData;
            }

            private float GetFreezeTimeDeltaByIceBadge(SkillBadgesData skillBadges, ImpactSourceType sourceType)
            {
                if (skillBadges == null)
                {
                    return 0f;
                }

                var genericBadges = skillBadges.GenericBadges;
                float ret = 0f;
                for (int i = 0; i < genericBadges.Count; ++i)
                {
                    if (genericBadges[i].BadgeId <= 0)
                    {
                        continue;
                    }

                    var drGenericBadge = GameEntry.DataTable.GetDataTable<DRGenericSkillBadge>().GetDataRow(genericBadges[i].BadgeId);
                    if (drGenericBadge == null)
                    {
                        continue;
                    }

                    if (drGenericBadge.ElementId == (int)HeroElementType.Ice)
                    {
                        ret += drGenericBadge.Params[0];
                    }
                }

                return ret;
            }

            public override void ApplyImpact(BaseApplyImpactData impactData)
            {
                var ad = impactData as ApplyFreezeImpactData;
                Character targetCharacter = ad.TargetData.Entity as Character;
                if (targetCharacter == null)
                {
                    return;
                }

                targetCharacter.Motion.PerformFreeze(ad);
            }

            public override void FillPacket(CREntityImpact packet, BaseApplyImpactData impactData)
            {
                var ad = impactData as ApplyFreezeImpactData;
                packet.FreezeImpacts.Add(new PBFreezeImpact
                {
                    ImpactId = ad.DataRow.Id,
                    StateCategory = (int)ad.StateCategory,
                    FreezeTime = ad.FreezeTime,
                });
            }
        }
    }
}
