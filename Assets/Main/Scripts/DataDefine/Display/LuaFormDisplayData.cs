using GameFramework;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="NGUILuaForm"/> 显示数据。
    /// </summary>
    public class LuaFormDisplayData : UIFormBaseUserData, IDictionary<string, object>
    {
        private IDictionary<string, object> m_UserData;

        public LuaFormDisplayData()
        {
            m_UserData = new Dictionary<string, object>();
        }
        
        public GameObject RootGO { get; set; }

        public NGUILuaForm Form { get; set; }

        public int ToggleGroupBaseValue { get; set; }

        public object this[string key]
        {
            get
            {
                return m_UserData[key];
            }

            set
            {
                m_UserData[key] = value;
            }
        }

        public int Count
        {
            get
            {
                return m_UserData.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return m_UserData.IsReadOnly;
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                return m_UserData.Keys;
            }
        }

        public ICollection<object> Values
        {
            get
            {
                return m_UserData.Values;
            }
        }

        public void Add(KeyValuePair<string, object> item)
        {
            m_UserData.Add(item);
        }

        public void Add(string key, object value)
        {
            m_UserData.Add(key, value);
        }

        public void Clear()
        {
            m_UserData.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return m_UserData.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return m_UserData.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            m_UserData.CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return m_UserData.GetEnumerator();
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return m_UserData.Remove(item);
        }

        public bool Remove(string key)
        {
            return m_UserData.Remove(key);
        }

        public bool TryGetValue(string key, out object value)
        {
            return m_UserData.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_UserData.GetEnumerator() as IEnumerator;
        }
    }
}
