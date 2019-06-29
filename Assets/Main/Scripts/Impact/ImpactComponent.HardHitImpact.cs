using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ImpactComponent
    {
        private class HardHitImpact : ImpactBase
        {
            public override int Type
            {
                get
                {
                    return (int)ImpactType.HardHit;
                }
            }

            public override BaseApplyImpactData PerformImpact(BasePerformImpactData impactData)
            {
                HardHitPerformImpactData data = impactData as HardHitPerformImpactData;
                Character targetCharacter = data.Target as Character;

                if (ShouldIgnoreStateImpact(impactData, targetCharacter))
                {
                    return null;
                }

                float stiffnessTime = 0.0f;
                float repulseDistance = 0.0f;
                float repulseTime = 0.0f;
                int repulseType = 0;
                ImpactAnimationType animType = ImpactAnimationType.None;
                float repulseStartTime = 0.0f;

                float fallingFloatingParameter = 0f;
                ImpactAnimationType fallingAnimationParameter = ImpactAnimationType.None;

                bool hasDispacementHarmFreeBuff = targetCharacter.HasDisplacementHarmFreeBuff;

                var ad = BaseApplyImpactData.Create<ApplyHardHitImpactData>(data.OriginData, data.TargetData, impactData.SourceType, impactData.DataRow.Id);
                if (targetCharacter.Motion.IsOnGround)
                {
                    stiffnessTime = data.Ground.StiffnessTime;
                    repulseDistance = hasDispacementHarmFreeBuff ? 0f : data.Ground.RepulseDistance;
                    repulseTime = data.Ground.RepulseTime;
                    repulseType = data.Ground.RepulseType;
                    animType = data.Ground.RepulseAnimation;
                    repulseStartTime = data.Ground.RepulseStartTime;
                    float repulseSpeed = repulseTime > 0f ? repulseDistance / repulseTime : 0f;
                    GameEntry.Impact.SpecialStateDispose(targetCharacter, ref repulseDistance, ref animType, ref fallingFloatingParameter, ref fallingAnimationParameter);
                    Vector3 repulseDirection = GetRepulseDirection(data.Origin, targetCharacter, repulseType);

                    ad.RepulseDirection = repulseDirection;
                    ad.ImpactAnimationType = animType;
                    ad.FallingAnimationType = ImpactAnimationType.None;
                    ad.FloatFallingVelocity = Vector3.zero;
                    ad.FloatFallingTime = 0f;
                    ad.DownTime = 0f;
                    ad.StiffTime = stiffnessTime;
                    ad.RepulseStartTime = repulseStartTime;
                    ad.RepulseVelocity = repulseDirection * repulseSpeed;
                    ad.RepulseTime = repulseTime;
                    ad.StateCategory = CharacterMotionStateCategory.Ground;
                }
                else if (targetCharacter.Motion.IsInAir)
                {
                    float fallingFloatingDistance = hasDispacementHarmFreeBuff ? 0f : data.Air.FallingFloatingDistance;
                    float fallingSpeed = data.Air.FallingFloatingTime > 0f ? fallingFloatingDistance / data.Air.FallingFloatingTime : 0f;
                    Vector3 repulseDirection = GetRepulseDirection(data.Origin, targetCharacter, repulseType);

                    ad.RepulseDirection = repulseDirection;
                    ad.ImpactAnimationType = animType;
                    ad.FallingAnimationType = data.Air.FallingAnimation;
                    ad.FloatFallingVelocity = repulseDirection * fallingSpeed;
                    ad.FloatFallingTime = data.Air.FallingFloatingTime;
                    ad.DownTime = data.Air.DownTime;
                    ad.StiffTime = stiffnessTime;
                    ad.RepulseStartTime = repulseStartTime;
                    ad.RepulseVelocity = Vector3.zero;
                    ad.RepulseTime = repulseTime;
                    ad.StateCategory = CharacterMotionStateCategory.Air;
                }

                return ad;
            }

            public override void ApplyImpact(BaseApplyImpactData impactData)
            {
                var ad = impactData as ApplyHardHitImpactData;
                var targetCharacter = ad.TargetData.Entity as Character;
                if (targetCharacter == null)
                {
                    return;
                }

                targetCharacter.Motion.PerformHardHit(ad);
            }

            public override void FillPacket(CREntityImpact packet, BaseApplyImpactData impactData)
            {
                var ad = impactData as ApplyHardHitImpactData;
                packet.HardHitImpacts.Add(new PBHardHitImpact
                {
                    DownTime = ad.DownTime,
                    FallingAnimationType = (int)ad.FallingAnimationType,
                    FloatFallingTime = ad.FloatFallingTime,
                    FloatFallingVelocity = ad.FloatFallingVelocity,
                    ImpactId = ad.DataRow.Id,
                    ImpactAnimationType = (int)ad.ImpactAnimationType,
                    RepulseDirection = ad.RepulseDirection,
                    RepulseStartTime = ad.RepulseStartTime,
                    RepulseTime = ad.RepulseTime,
                    RepulseVelocity = ad.RepulseVelocity,
                    StateCategory = (int)ad.StateCategory,
                    StiffTime = ad.StiffTime,
                });
            }
        }
    }
}
