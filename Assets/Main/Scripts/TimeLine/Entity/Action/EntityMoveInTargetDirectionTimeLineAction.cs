using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 用于 NPC 在一定时间内向其目标所在点移动一段距离的技能，配合 AI 使用。
    /// </summary>
    public class EntityMoveInTargetDirectionTimeLineAction : EntityMoveInDurationTimeLineAction
    {

        private EntityMoveInTargetDirectionTimeLineActionData m_Data;

        public EntityMoveInTargetDirectionTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityMoveInTargetDirectionTimeLineActionData;
            m_Delay = m_Data.Delay;
            m_FaceTargetPositionOnMove = m_Data.FaceTargetPosOnMove;
        }

        protected override Vector3 GetTargetPosition(NpcCharacter npcOwner)
        {
            if (!npcOwner.HasTarget)
            {
                //Log.Warning("Target not found, so this action will be performed in place.");
                return npcOwner.CachedTransform.position;
            }

            var targetEntity = npcOwner.Target as Entity;
            var targetOriginalPosition = targetEntity.CachedTransform.position;
            var ownerPosition = npcOwner.CachedTransform.position;
            var targetPos = ownerPosition + (targetOriginalPosition - ownerPosition).normalized * m_Data.Offset;
            return targetPos;
        }
    }
}
