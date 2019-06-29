using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class ImpactComponent
    {
        private class SoundAndEffectImpact : ImpactBase
        {
            public override int Type
            {
                get
                {
                    return (int)ImpactType.SoundAndEffect;
                }
            }

            public override BaseApplyImpactData PerformImpact(BasePerformImpactData impactData)
            {
                OtherPerformImpactData data = impactData as OtherPerformImpactData;
                if (data == null)
                {
                    Log.Error("SoundAndEffectImpact's PerformImpact's data is nulls.");
                    return null;
                }

                var applyImpactData = BaseApplyImpactData.Create<ApplySoundAndEffectImpactData>(data.OriginData, data.TargetData, impactData.SourceType, impactData.DataRow.Id);
                return applyImpactData;
            }

            public override void ApplyImpact(BaseApplyImpactData impactData)
            {
                var data = impactData as ApplySoundAndEffectImpactData;

                PlaySoundIfNeeded(data.OriginData, data.TargetData, data.DataRow.ImpactParams);
                ShowEffectIfNeeded(data.OriginData, data.TargetData, data.DataRow.ImpactParams);
            }

            public override void FillPacket(CREntityImpact packet, BaseApplyImpactData impactData)
            {
                var data = impactData as ApplySoundAndEffectImpactData;
                packet.SoundAndEffectImpacts.Add(new PBSoundAndEffectImpact
                {
                    ImpactId = data.DataRow.Id,
                });
            }

            private static void PlaySoundIfNeeded(EntityData originData, EntityData targetData, float[] impactParams)
            {
                int soundId = Mathf.RoundToInt(impactParams[0]);
                if (soundId < 0)
                {
                    return;
                }

                var targetTargetableObj = targetData.Entity as TargetableObject;
                if (targetTargetableObj == null)
                {
                    return;
                }

                bool soundNeedsBroadcast = Mathf.RoundToInt(impactParams[1]) != 0;
                if (soundNeedsBroadcast || AIUtility.EntityDataIsMine(originData) || targetData.Entity is MeHeroCharacter)
                {
                    GameEntry.Sound.PlaySound(soundId, targetTargetableObj);
                }
            }

            private static void ShowEffectIfNeeded(EntityData originData, EntityData targetData, float[] impactParams)
            {
                int effectId = Mathf.RoundToInt(impactParams[2]);
                if (effectId < 0)
                {
                    return;
                }

                Character targetCharacter = targetData.Entity as Character;
                if (targetCharacter == null)
                {
                    return;
                }

                int attachPointId = Mathf.RoundToInt(impactParams[3]);
                var dtCharacter = GameEntry.DataTable.GetDataTable<DRCharacter>();
                string hitPointPath = dtCharacter[targetCharacter.Data.CharacterId].GetHitPoint(attachPointId);

                var dtEffect = GameEntry.DataTable.GetDataTable<DREffect>();
                DREffect drEffect = dtEffect.GetDataRow(effectId);
                if (drEffect != null)
                {
                    GameEntry.Entity.ShowEffect(new EffectData(GameEntry.Entity.GetSerialId(), hitPointPath, drEffect.ResourceName, targetCharacter.Id));
                }
            }
        }
    }
}
