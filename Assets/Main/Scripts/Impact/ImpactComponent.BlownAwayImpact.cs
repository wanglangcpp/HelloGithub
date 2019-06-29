using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ImpactComponent
    {
        private class BlownAwayImpact : ImpactBase
        {
            public override int Type
            {
                get
                {
                    return (int)ImpactType.BlownAway;
                }
            }

            public override BaseApplyImpactData PerformImpact(BasePerformImpactData impactData)
            {
                BlownAwayPerformImpactData data = impactData as BlownAwayPerformImpactData;
                Character targetCharacter = data.Target as Character;
                if (ShouldIgnoreStateImpact(impactData, targetCharacter))
                {
                    return null;
                }

                bool hasDisplacementHarmFreeBuff = targetCharacter.HasDisplacementHarmFreeBuff;

                var ad = BaseApplyImpactData.Create<ApplyBlownAwayImpactData>(data.OriginData, data.TargetData, impactData.SourceType, impactData.DataRow.Id);
                if (targetCharacter.Motion.IsOnGround)
                {
                    float raiseFloatingTime = data.Ground.RaiseFloatingTime;
                    float raiseFloatingDistance = hasDisplacementHarmFreeBuff ? 0f : data.Ground.RaiseFloatingDistance;
                    float fallingFloatingTime = data.Ground.FallingFloatingTime;
                    float fallingFloatingDistance = hasDisplacementHarmFreeBuff ? 0f : data.Ground.FallingFloatingDistance;
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

                    ad.RepulseDirection = impactDirection;
                    ad.RaiseAnimationType = raiseAnimation;
                    ad.FallingAnimationType = fallingAnimation;
                    ad.FloatVelocity = impactDirection * floatingSpeed;
                    ad.FloatFallingVelocity = impactDirection * floatingFallingSpeed;
                    ad.FloatTime = raiseFloatingTime;
                    ad.FloatFallingTime = fallingFloatingTime;
                    ad.DownTime = downTime;
                    ad.RepulseStartTime = repulseStartTime;
                    ad.RepulseVelocity = impactDirection * repulseSpeed;
                    ad.RepulseTime = raiseFloatingTime;
                    ad.StateCategory = CharacterMotionStateCategory.Ground;
                }
                else if (targetCharacter.Motion.IsInAir)
                {
                    int repulseType = data.Air.RepulseType;
                    float fallingFloatingDistance = hasDisplacementHarmFreeBuff ? 0f : data.Air.FallingFloatingDistance;
                    float fallingSpeed = data.Air.FallingFloatingTime > 0f ? fallingFloatingDistance / data.Air.FallingFloatingTime : 0f;
                    Vector3 repulseDirection = GetRepulseDirection(data.Origin, targetCharacter, repulseType);

                    ad.RepulseDirection = repulseDirection;
                    ad.RaiseAnimationType = ImpactAnimationType.None;
                    ad.FallingAnimationType = data.Air.FallingAnimation;
                    ad.FloatVelocity = Vector3.zero;
                    ad.FloatFallingVelocity = repulseDirection * fallingSpeed;
                    ad.FloatTime = 0f;
                    ad.FloatFallingTime = data.Air.FallingFloatingTime;
                    ad.DownTime = data.Air.DownTime;
                    ad.RepulseStartTime = 0f;
                    ad.RepulseVelocity = Vector3.zero;
                    ad.RepulseTime = 0f;
                    ad.StateCategory = CharacterMotionStateCategory.Air;
                }

                return ad;
            }

            public override void ApplyImpact(BaseApplyImpactData impactData)
            {
                var ad = impactData as ApplyBlownAwayImpactData;
                var targetCharacter = ad.TargetData.Entity as Character;
                if (targetCharacter == null)
                {
                    return;
                }

                targetCharacter.Motion.PerformBlownAway(ad);
            }

            public override void FillPacket(CREntityImpact packet, BaseApplyImpactData impactData)
            {
                var ad = impactData as ApplyBlownAwayImpactData;
                packet.BlownAwayImpacts.Add(new PBBlownAwayImpact
                {
                    DownTime = ad.DownTime,
                    FallingAnimationType = (int)ad.FallingAnimationType,
                    FloatFallingTime = ad.FloatFallingTime,
                    FloatFallingVelocity = ad.FloatFallingVelocity,
                    FloatTime = ad.FloatTime,
                    FloatVelocity = ad.FloatVelocity,
                    ImpactId = ad.DataRow.Id,
                    RaiseAnimationType = (int)ad.RaiseAnimationType,
                    RepulseDirection = ad.RepulseDirection,
                    RepulseStartTime = ad.RepulseStartTime,
                    RepulseTime = ad.RepulseTime,
                    RepulseVelocity = ad.RepulseVelocity,
                    StateCategory = (int)ad.StateCategory,
                });
            }
        }
    }
}
