using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// Data class for single player instance (dungeon).
    /// </summary>
    [Serializable]
    public class InstanceData
    {
        [SerializeField]
        private PlayerHeroesData m_MeHeroesData = new PlayerHeroesData();

        private readonly Dictionary<string, RandomShowNpcGroupData> m_RandomShowNpcGroupDatas = new Dictionary<string, RandomShowNpcGroupData>();
        private readonly List<RandomShowNpcGroupData> m_RandomShowNpcGroupList = new List<RandomShowNpcGroupData>();

        private readonly Dictionary<string, object> m_MiscParams = new Dictionary<string, object>();

#if UNITY_EDITOR

        [Serializable]
        private class MiscParamToInspect
        {
            public string Key;
            public string Value;
        }

        [SerializeField]
        private List<MiscParamToInspect> m_MiscParamsToInspect = new List<MiscParamToInspect>();

#endif

        public PlayerHeroesData MeHeroesData
        {
            get
            {
                return m_MeHeroesData;
            }
        }

        public IList<RandomShowNpcGroupData> RandomShowNpcGroupList
        {
            get
            {
                return m_RandomShowNpcGroupList;
            }
        }

        public InstanceData()
        {
            // Empty
        }

        #region Random NPC showing groups

        public void AddRandomShowNpcGroupData(string key, IDictionary<int, int> npcIndicesToWeights, int targetTotalCount, int upperLimit)
        {
            var data = new RandomShowNpcGroupData(key, npcIndicesToWeights, targetTotalCount, upperLimit);
            m_RandomShowNpcGroupDatas.Add(key, data);
        }

        public bool RemoveRandomShowNpcGroupData(string key)
        {
            if (m_RandomShowNpcGroupDatas.ContainsKey(key))
            {
                m_RandomShowNpcGroupList.Remove(m_RandomShowNpcGroupDatas[key]);
            }

            return m_RandomShowNpcGroupDatas.Remove(key);
        }

        public RandomShowNpcGroupData GetRandomShowNpcGroupData(string key)
        {
            return m_RandomShowNpcGroupDatas[key];
        }

        #endregion Random NPC showing groups

        #region Misc params

        public void AddMiscParam<T>(string key, T initialValue)
        {
            m_MiscParams.Add(key, initialValue);

#if UNITY_EDITOR
            m_MiscParamsToInspect.Add(new MiscParamToInspect { Key = key, Value = initialValue == null ? "<null>" : initialValue.ToString() });
            m_MiscParamsToInspect.Sort((a, b) => (a.Key.CompareTo(b.Key)));
#endif
        }

        public void SetMiscParam<T>(string key, T newValue)
        {
            GameEntry.Event.Fire(this, new InstanceParamChangedEventArgs());
            m_MiscParams[key] = newValue;

#if UNITY_EDITOR
            var param = m_MiscParamsToInspect.Find(x => x.Key == key);
            param.Value = newValue == null ? "<null>" : newValue.ToString();
#endif
        }

        public T GetMiscParam<T>(string key)
        {
            object originalValue;
            bool hasGot = m_MiscParams.TryGetValue(key, out originalValue);

            if (!hasGot)
            {
                Log.Error("[InstanceData GetMiscParam] Key '{0}' not found.", key);
                return default(T);
            }
            return (T)(m_MiscParams[key]);
        }

        public bool HasMiscParam(string key)
        {
            return m_MiscParams.ContainsKey(key);
        }

        #endregion Misc params
    }
}
