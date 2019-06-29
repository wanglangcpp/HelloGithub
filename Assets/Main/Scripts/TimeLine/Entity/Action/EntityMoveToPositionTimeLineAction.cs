using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 用于 NPC 在一定时间内向指定点移动的技能，配合 AI 使用。
    /// </summary>
    public class EntityMoveToPositionTimeLineAction : EntityMoveInDurationTimeLineAction
    {
        private EntityMoveToPositionTimeLineActionData m_Data;

        public EntityMoveToPositionTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityMoveToPositionTimeLineActionData;
            m_Delay = m_Data.Delay;
            m_FaceTargetPositionOnMove = m_Data.FaceTargetPosOnMove;
        }

        protected override Vector3 GetTargetPosition(NpcCharacter npcOwner)
        {
            return npcOwner.SelectTargetPosition();
        }
    }
}
