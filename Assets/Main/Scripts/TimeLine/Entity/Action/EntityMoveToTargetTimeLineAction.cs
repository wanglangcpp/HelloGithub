using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 用于 NPC 在一定时间内向其目标所在点移动的技能，配合 AI 使用。
    /// </summary>
    public class EntityMoveToTargetTimeLineAction : EntityMoveInDurationTimeLineAction
    {
        private EntityMoveToTargetTimeLineActionData m_Data;

        public EntityMoveToTargetTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityMoveToTargetTimeLineActionData;
            m_Delay = m_Data.Delay;
            m_FaceTargetPositionOnMove = m_Data.FaceTargetPosOnMove;
        }

        protected override Vector3 GetTargetPosition(NpcCharacter npcOwner)
        {
            if (!npcOwner.HasTarget)
            {
                Log.Info("Target not found, so this action will be performed in place.");
                return npcOwner.CachedTransform.position;
            }

            var targetEntity = npcOwner.Target as Entity;
            var targetOriginalPosition = targetEntity.CachedTransform.position;
            var targetPos = targetOriginalPosition + (targetOriginalPosition - npcOwner.CachedTransform.position).normalized * m_Data.OffsetFromTarget;
            return targetPos;
        }
    }
}
