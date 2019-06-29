using GameFramework;

namespace Genesis.GameClient
{
    public partial class ImpactComponent
    {
        private class SteadyDamageImpact : ImpactBase
        {
            public override int Type
            {
                get
                {
                    return (int)ImpactType.SteadyDamage;
                }
            }

            public override BaseApplyImpactData PerformImpact(BasePerformImpactData impactData)
            {
                OtherPerformImpactData data = impactData as OtherPerformImpactData;
                if (data == null)
                {
                    Log.Error("SteadyDamageImpact's PerformImpact's data is nulls.");
                    return null;
                }

                Character character = data.Target as Character;
                if (character == null)
                {
                    return null;
                }

                if (character.HasNumHarmFreeBuff)
                {
                    return null;
                }

                if (!character.Data.Steady.IsSteadying)
                {
                    return null;
                }

                var applyImpactData = BaseApplyImpactData.Create<ApplySteadyDamageImpactData>(data.OriginData, data.TargetData, impactData.SourceType, impactData.DataRow.Id);
                applyImpactData.DamageSteady = data.ImpactParams[0];

                return applyImpactData;
            }

            public override void ApplyImpact(BaseApplyImpactData impactData)
            {
                var data = impactData as ApplySteadyDamageImpactData;
                var character = impactData.TargetData.Entity as Character;

                if (character.Data.Steady.IsSteadying)
                {
                    character.Data.Steady.Steady -= data.DamageSteady;
                    character.Data.Steady.UpdateSteady();
                }
            }

            public override void FillPacket(CREntityImpact packet, BaseApplyImpactData impactData)
            {
                var data = impactData as ApplySteadyDamageImpactData;
                packet.SteadyDamageImpacts.Add(new PBSteadyDamageImpact
                {
                    ImpactId = data.DataRow.Id,
                    DamageSteady = data.DamageSteady,
                });
            }
        }
    }
}
