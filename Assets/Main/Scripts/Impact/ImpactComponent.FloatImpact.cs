using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ImpactComponent
    {
        private class FloatImpact : ImpactBase
        {
            public override int Type
            {
                get
                {
                    return (int)ImpactType.Float;
                }
            }

            public override BaseApplyImpactData PerformImpact(BasePerformImpactData impactData)
            {
                FloatPerformImpactData data = impactData as FloatPerformImpactData;

                Character targetCharacter = impactData.Target as Character;
                if (ShouldIgnoreStateImpact(impactData, targetCharacter))
                {
                    return null;
                }

                bool hasDispacementHarmFreeBuff = targetCharacter.HasDisplacementHarmFreeBuff;

                var ad = BaseApplyImpactData.Create<ApplyFloatImpactData>(data.OriginData, data.TargetData, impactData.SourceType, impactData.DataRow.Id);
                if (targetCharacter.Motion.IsOnGround)
                {
                    float raiseFloatingTime = data.Ground.RaiseFloatingTime;
                    float raiseFloatingDistance = hasDispacementHarmFreeBuff ? 0f : data.Ground.RaiseFloatingDistance;
                    float fallingFloatingTime = data.Ground.FallingFloatingTime;
                    float fallingFloatingDistance = hasDispacementHarmFreeBuff ? 0f : data.Ground.FallingFloatingDistance;
                    float downTime = data.Ground.DownTime;
                    float repulseStartTime = data.Ground.RepulseStartTime;
                    int repulseType = data.Ground.RepulseType;
                    ImpactAnimationType raiseAnimation = data.Ground.RaiseAnimation;
                    ImpactAnimationType fallingAnimation = data.Ground.FallingAnimation;
                    float repulseSpeed = raiseFloatingTime > 0f ? raiseFloatingDistance / raiseFloatingTime : 0f;
                    float floatingSpeed = raiseFloatingTime > 0f ? raiseFloatingDistance / raiseFloatingTime : 0f;
                    float floatingFallingSpeed = fallingFloatingTime > 0f ? fallingFloatingDistance / fallingFloatingTime : 0f;
                    GameEntry.Impact.SpecialStateDispose(targetCharacter, ref raiseFloatingDistance, ref raiseAnimation, ref fallingFloatingDistance, ref fallingAnimation);

                    Vector3 impactDirection = GetRepulseDirection(impactData.Origin, targetCharacter, repulseType);

                    ad.StateCategory = CharacterMotionStateCategory.Ground;
                    ad.RepulseDirection = impactDirection;
                    ad.RaiseAnimationType = raiseAnimation;
                    ad.FallingAnimationType = fallingAnimation;
                    ad.FloatVelocity = impactDirection * floatingSpeed;
                    ad.FloatFallingVelocity = impactDirection * floatingFallingSpeed;
                    ad.FloatingTime = raiseFloatingTime;
                    ad.FloatFallingTime = fallingFloatingTime;
                    ad.DownTime = downTime;
                    ad.RepulseStartTime = repulseStartTime;
                    ad.RepulseVelocity = impactDirection * repulseSpeed;
                    ad.RepulseTime = data.Air.RepulseTime;
                    ad.StiffTime = data.Air.StiffnessTime;
                }
                else if (targetCharacter.Motion.IsInAir)
                {
                    float stiffnessTime = data.Air.StiffnessTime;
                    float repulseDistance = hasDispacementHarmFreeBuff ? 0f : data.Air.RepulseDistance;
                    float repulseTime = data.Air.RepulseTime;
                    int repulseType = data.Air.RepulseType;
                    ImpactAnimationType animType = data.Air.RepulseAnimation;
                    float repulseStartTime = data.Air.RepulseStartTime;
                    float repulseSpeed = repulseTime > 0f ? repulseDistance / repulseTime : 0f;
                    float fallingFloatingDistance = hasDispacementHarmFreeBuff ? 0f : data.Ground.FallingFloatingDistance;
                    float fallingSpeed = data.Ground.FallingFloatingTime > 0f ? fallingFloatingDistance / data.Ground.FallingFloatingTime : 0f;
                    Vector3 repulseDirection = GetRepulseDirection(data.Origin, targetCharacter, repulseType);

                    ad.StateCategory = CharacterMotionStateCategory.Air;
                    ad.RepulseDirection = repulseDirection;
                    ad.RaiseAnimationType = animType;
                    ad.FallingAnimationType = data.Ground.FallingAnimation;
                    ad.FloatVelocity = Vector3.zero;
                    ad.FloatFallingVelocity = repulseDirection * fallingSpeed;
                    ad.FloatingTime = 0f;
                    ad.FloatFallingTime = data.Ground.FallingFloatingTime;
                    ad.DownTime = data.Ground.DownTime;
                    ad.RepulseStartTime = repulseStartTime;
                    ad.RepulseVelocity = repulseDirection * repulseSpeed;
                    ad.RepulseTime = repulseTime;
                    ad.StiffTime = stiffnessTime;
                }

                return ad;
            }

            public override void ApplyImpact(BaseApplyImpactData impactData)
            {
                var ad = impactData as ApplyFloatImpactData;
                var targetCharacter = ad.TargetData.Entity as Character;
                if (targetCharacter == null)
                {
                    return;
                }

                targetCharacter.Motion.PerformFloat(ad);
            }

            public override void FillPacket(CREntityImpact packet, BaseApplyImpactData impactData)
            {
                var ad = impactData as ApplyFloatImpactData;
                packet.FloatImpacts.Add(new PBFloatImpact
                {
                    DownTime = ad.DownTime,
                    FallingAnimationType = (int)ad.FallingAnimationType,
                    FloatFallingTime = ad.FloatFallingTime,
                    FloatFallingVelocity = ad.FloatFallingVelocity,
                    FloatingTime = ad.FloatingTime,
                    FloatVelocity = ad.FloatVelocity,
                    ImpactId = ad.DataRow.Id,
                    RaiseAnimationType = (int)ad.RaiseAnimationType,
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
