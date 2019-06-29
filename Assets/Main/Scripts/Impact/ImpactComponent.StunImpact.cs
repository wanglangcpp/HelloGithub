namespace Genesis.GameClient
{
    public partial class ImpactComponent
    {
        private class StunImpact : ImpactBase
        {
            public override int Type
            {
                get
                {
                    return (int)ImpactType.Stun;
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
                float stunTime = data.ImpactParams[index++];

                var applyImpactData = BaseApplyImpactData.Create<ApplyStunImpactData>(data.OriginData, data.TargetData, impactData.SourceType, impactData.DataRow.Id);
                applyImpactData.StunTime = stunTime;
                return applyImpactData;
            }

            public override void ApplyImpact(BaseApplyImpactData impactData)
            {
                var ad = impactData as ApplyStunImpactData;
                Character targetCharacter = ad.TargetData.Entity as Character;
                if (targetCharacter == null)
                {
                    return;
                }

                targetCharacter.Motion.PerformStun(ad);
            }

            public override void FillPacket(CREntityImpact packet, BaseApplyImpactData impactData)
            {
                var ad = impactData as ApplyStunImpactData;
                packet.StunImpacts.Add(new PBStunImpact
                {
                    ImpactId = ad.DataRow.Id,
                    StunTime = ad.StunTime,
                });
            }
        }
    }
}
