using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class BaseSinglePlayerInstanceLogic
    {
        public List<int> GetAvailableIdsForRandomShowNpcGroup(RandomShowNpcGroupData groupData)
        {
            var ret = new HashSet<int>(groupData.IndicesToWeights.Keys);
            ret.ExceptWith(m_LivingNpcIndices);
            return new List<int>(ret);
        }

        public int GetLivingNpcCountInRandomShowNpcGroup(RandomShowNpcGroupData groupData)
        {
            var indices = new HashSet<int>(groupData.IndicesToWeights.Keys);
            indices.IntersectWith(m_LivingNpcIndices);
            return indices.Count;
        }
    }
}
