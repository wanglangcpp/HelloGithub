using GameFramework;
using System;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 从一组 NPC 中随机选取固定个数加给定 Buff。
    /// </summary>
    public class EntityAddBuffToNpcsTimeLineAction : EntityAbstractTimeLineAction
    {
        private EntityAddBuffToNpcsTimeLineActionData m_Data;
        private int m_SkillIndex;
        private SkillBadgesData m_SkillBadges;

        public EntityAddBuffToNpcsTimeLineAction(TimeLineActionData data)
            : base(data)
        {
            m_Data = data as EntityAddBuffToNpcsTimeLineActionData;
        }

        public override void OnStart(ITimeLineInstance<Entity> timeLineInstance)
        {
            var owner = timeLineInstance.Owner as TargetableObject;
            var instanceLogic = GameEntry.SceneLogic.BaseSinglePlayerInstanceLogic as BaseSinglePlayerInstanceLogic;
            if (instanceLogic == null)
            {
                Log.Error("Instance type is not supported.");
                return;
            }

            if (!instanceLogic.CanAddBuffInSkillTimeLine(owner, owner))
            {
                return;
            }

            m_SkillIndex = GetSkillIndex(timeLineInstance);
            m_SkillBadges = GetSkillBadges(timeLineInstance, m_SkillIndex);

            UserData myUserData = GetMyUserData(timeLineInstance);
            AddBuffsToNpcs(owner, instanceLogic, myUserData);
        }

        public override void OnFinish(ITimeLineInstance<Entity> timeLineInstance)
        {

        }

        public override void OnUpdate(ITimeLineInstance<Entity> timeLineInstance, float elapseSeconds)
        {

        }

        public override void OnBreak(ITimeLineInstance<Entity> timeLineInstance, object userData)
        {

        }

        public override void OnEvent(ITimeLineInstance<Entity> timeLineInstance, object sender, int eventId, object userData)
        {

        }

        public override void OnDebugDraw(ITimeLineInstance<Entity> timeLineInstance)
        {

        }

        private static UserData GetMyUserData(ITimeLineInstance<Entity> timeLineInstance)
        {
            UserData myUserData;
            var userDataDict = timeLineInstance.UserData as Dictionary<string, object>;
            object myUserDataObj;
            if (!userDataDict.TryGetValue(Constant.TimeLineAddBuffToNpcsKey, out myUserDataObj))
            {
                myUserData = new UserData();
                userDataDict.Add(Constant.TimeLineAddBuffToNpcsKey, myUserData);
            }
            else
            {
                myUserData = myUserDataObj as UserData;
            }

            return myUserData;
        }

        private void AddBuffsToNpcs(TargetableObject owner, BaseSinglePlayerInstanceLogic instanceLogic, UserData myUserData)
        {
            // Copy NPC indices and shuffle.
            int[] npcs = new int[m_Data.NpcIndices.Length];
            Array.Copy(m_Data.NpcIndices, npcs, m_Data.NpcIndices.Length);
            AIUtility.Shuffle(npcs);

            int countToAffect = m_Data.CountToAffect;
            var affectedNpcs = myUserData.AffectedNpcIndices;
            for (int i = 0; i < npcs.Length && countToAffect > 0; ++i)
            {
                int npcIndex = npcs[i];
                if (m_Data.AvoidAffectedNpcs && affectedNpcs.Contains(npcIndex))
                {
                    continue;
                }

                var npcCharacter = instanceLogic.GetNpcFromIndex(npcIndex);
                if (AIUtility.TargetCanBeSelected(npcCharacter))
                {
                    AddBuffsToNpc(owner, npcCharacter);
                    countToAffect--;
                }
            }
        }

        private void AddBuffsToNpc(TargetableObject owner, NpcCharacter npcCharacter)
        {
            for (int i = 0; i < m_Data.BuffIds.Length; ++i)
            {
                npcCharacter.AddBuff(m_Data.BuffIds[i], owner.Data, OfflineBuffPool.GetNextSerialId(), m_SkillBadges);
            }
        }

        private class UserData
        {
            public HashSet<int> AffectedNpcIndices = new HashSet<int>();
        }
    }
}
