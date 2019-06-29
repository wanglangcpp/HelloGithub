using GameFramework;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class BaseInstanceLogic
    {
        protected readonly InstanceStat m_InstanceStat = new InstanceStat();

        public InstanceStat Stat
        {
            get
            {
                return m_InstanceStat;
            }
        }

        public class InstanceStat
        {
            private readonly IList<StatInfo> m_Statistics;

            public InstanceStat()
            {
                m_Statistics = new List<StatInfo>();
            }

            public void Shutdown()
            {
                m_Statistics.Clear();
            }

            public void ResetStat(int entityId, string customKey, int ownerEntityId)
            {
                StatInfo statInfo = GetStatInfo(entityId, customKey);
                if (statInfo != null)
                {
                    statInfo.OwnerEntityId = ownerEntityId;
                    statInfo.Reset();
                }
                else
                {
                    m_Statistics.Add(new StatInfo(entityId, customKey, ownerEntityId));
                }
            }

            public void RecordDamage(int entityId, int damageValue)
            {
                for (int i = 0; i < m_Statistics.Count; i++)
                {
                    if (m_Statistics[i].EntityId == entityId)
                    {
                        m_Statistics[i].DamageCount++;
                        m_Statistics[i].DamageValue += damageValue;
                        if (m_Statistics[i].OwnerEntityId != 0)
                        {
                            RecordDamage(m_Statistics[i].OwnerEntityId, damageValue);
                        }
                    }
                }

                GameEntry.Event.Fire(this, new StatDamageRecordedEventArgs(entityId, damageValue));
            }

            public int GetDamageCount(int entityId, string customKey)
            {
                StatInfo statInfo = GetStatInfo(entityId, customKey);
                if (statInfo == null)
                {
                    Log.Warning("Can not find stat key, entity id '{0}' custom key '{1}'.", entityId.ToString(), customKey);
                    return 0;
                }

                return statInfo.DamageCount;
            }

            public int GetDamageValue(int entityId, string customKey)
            {
                StatInfo statInfo = GetStatInfo(entityId, customKey);
                if (statInfo == null)
                {
                    Log.Warning("Can not find stat key, entity id '{0}' custom key '{1}'.", entityId.ToString(), customKey);
                    return 0;
                }

                return statInfo.DamageValue;
            }

            private StatInfo GetStatInfo(int entityId, string customKey)
            {
                if (customKey == null)
                {
                    customKey = string.Empty;
                }

                for (int i = 0; i < m_Statistics.Count; i++)
                {
                    if (m_Statistics[i].EntityId == entityId && m_Statistics[i].CustomKey == customKey)
                    {
                        return m_Statistics[i];
                    }
                }

                return null;
            }

            private class StatInfo
            {
                private readonly int m_EntityId;
                private readonly string m_CustomKey;

                public StatInfo(int entityId, string customKey, int ownerEntityId)
                {
                    m_EntityId = entityId;
                    m_CustomKey = customKey ?? string.Empty;
                    OwnerEntityId = ownerEntityId;

                    Reset();
                }

                public int EntityId
                {
                    get
                    {
                        return m_EntityId;
                    }
                }

                public string CustomKey
                {
                    get
                    {
                        return m_CustomKey;
                    }
                }

                public int OwnerEntityId
                {
                    get;
                    set;
                }

                public int DamageCount
                {
                    get;
                    set;
                }

                public int DamageValue
                {
                    get;
                    set;
                }

                public void Reset()
                {
                    DamageCount = 0;
                    DamageValue = 0;
                }
            }
        }
    }
}
