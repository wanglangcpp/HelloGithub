using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ImpactComponent : MonoBehaviour
    {
        private class StiffnessImpact : ImpactBase
        {
            public override int Type
            {
                get
                {
                    return (int)ImpactType.Stiffness;
                }
            }

            public override BaseApplyImpactData PerformImpact(BasePerformImpactData impactData)
            {
                StiffnessPerformImpactData data = impactData as StiffnessPerformImpactData;
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

                ApplyStiffnessImpactData applyImpactData = null;
                applyImpactData = BaseApplyImpactData.Create<ApplyStiffnessImpactData>(data.OriginData, data.TargetData, impactData.SourceType, impactData.DataRow.Id);
                bool hasDisplacementFreeBuff = targetCharacter.HasDisplacementHarmFreeBuff;

                if (targetCharacter.Motion.IsOnGround)
                {
                    stiffnessTime = data.Ground.StiffnessTime;
                    repulseDistance = hasDisplacementFreeBuff ? 0 : data.Ground.RepulseDistance;
                    repulseTime = data.Ground.RepulseTime;
                    repulseType = data.Ground.RepulseType;
                    animType = data.Ground.RepulseAnimation;
                    repulseStartTime = data.Ground.RepulseStartTime;
                    float repulseSpeed = repulseTime > 0f ? repulseDistance / repulseTime : 0f;
                    GameEntry.Impact.SpecialStateDispose(targetCharacter, ref repulseDistance, ref animType, ref fallingFloatingParameter, ref fallingAnimationParameter);
                    Vector3 repulseDirection = GetRepulseDirection(data.Origin, targetCharacter, repulseType);

                    applyImpactData.StateCatetory = CharacterMotionStateCategory.Ground;
                    applyImpactData.FallingAnimationType = ImpactAnimationType.None;
                    applyImpactData.RepulseDirection = repulseDirection;
                    applyImpactData.FloatFallingVelocity = Vector3.zero;
                    applyImpactData.FloatFallingTime = 0f;
                    applyImpactData.DownTime = 0f;
                    applyImpactData.StiffTime = stiffnessTime;
                    applyImpactData.RepulseStartTime = repulseStartTime;
                    applyImpactData.RepulseVelocity = repulseDirection * repulseSpeed;
                    applyImpactData.RepulseTime = repulseTime;
                    applyImpactData.ImpactAnimationType = animType;
                }
                else if (targetCharacter.Motion.IsInAir)
                {
                    stiffnessTime = data.Air.StiffnessTime;
                    repulseDistance = hasDisplacementFreeBuff ? 0 : data.Air.RepulseDistance;
                    repulseTime = data.Air.RepulseTime;
                    repulseType = data.Air.RepulseType;
                    animType = data.Air.RepulseAnimation;
                    repulseStartTime = data.Air.RepulseStartTime;
                    float repulseSpeed = repulseTime > 0f ? repulseDistance / repulseTime : 0f;
                    float fallingFloatingDistance = hasDisplacementFreeBuff ? 0f : data.Air.FallingFloatingDistance;
                    float fallingSpeed = data.Air.FallingFloatingTime > 0f ? fallingFloatingDistance / data.Air.FallingFloatingTime : 0f;
                    Vector3 repulseDirection = GetRepulseDirection(data.Origin, targetCharacter, repulseType);

                    applyImpactData.StateCatetory = CharacterMotionStateCategory.Air;
                    applyImpactData.FallingAnimationType = data.Air.FallingAnimation;
                    applyImpactData.RepulseDirection = repulseDirection;
                    applyImpactData.FloatFallingVelocity = repulseDirection * fallingSpeed;
                    applyImpactData.FloatFallingTime = data.Air.FallingFloatingTime;
                    applyImpactData.DownTime = data.Air.DownTime;
                    applyImpactData.StiffTime = stiffnessTime;
                    applyImpactData.RepulseStartTime = repulseStartTime;
                    applyImpactData.RepulseVelocity = repulseDirection * repulseSpeed;
                    applyImpactData.RepulseTime = repulseTime;
                    applyImpactData.ImpactAnimationType = animType;
                }

                return applyImpactData;
            }

            public override void ApplyImpact(BaseApplyImpactData impactData)
            {
                var data = impactData as ApplyStiffnessImpactData;
                var targetCharacter = data.TargetData.Entity as Character;
                if (targetCharacter == null)
                {
                    return;
                }

                targetCharacter.Motion.PerformStiffness(data);
            }

            public override void FillPacket(CREntityImpact packet, BaseApplyImpactData impactData)
            {
                var data = impactData as ApplyStiffnessImpactData;
                packet.StiffnessImpacts.Add(new PBStiffnessImpact
                {
                    ImpactId = data.DataRow.Id,
                    DownTime = data.DownTime,
                    FallingAnimationType = (int)data.FallingAnimationType,
                    ImpactAnimationType = (int)data.ImpactAnimationType,
                    FloatFallingSpeed = data.FloatFallingVelocity,
                    FloatFallingTime = data.FloatFallingTime,
                    RepulseDirection = data.RepulseDirection,
                    RepulseSpeed = data.RepulseVelocity,
                    RepulseStartTime = data.RepulseStartTime,
                    RepulseTime = data.RepulseTime,
                    StateCategory = (int)data.StateCatetory,
                    StiffnessTime = data.StiffTime,
                });
            }
        }
    }
}
