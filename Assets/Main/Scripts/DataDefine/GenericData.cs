using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public abstract class GenericData<T, PBT> where T : class, IGenericData<T, PBT>, new()
    {
        [SerializeField]
        private List<T> m_Data = new List<T>();

        public List<T> Data { get { return m_Data; } }

        public bool HasKey(int key)
        {
            for (int i = 0; i < m_Data.Count; i++)
            {
                if (m_Data[i].Key == key)
                {
                    return true;
                }
            }

            return false;
        }

        public T GetData(int key)
        {
            for (int i = 0; i < m_Data.Count; i++)
            {
                if (m_Data[i].Key == key)
                {
                    return m_Data[i];
                }
            }

            return null;
        }

        public void ClearData()
        {
            m_Data.Clear();
        }

        public void ClearAndAddData(List<PBT> pbData)
        {
            ClearData();
            AddData(pbData);
        }

        public void AddData(List<PBT> pbData)
        {
            for (int i = 0; i < pbData.Count; i++)
            {
                AddData(pbData[i]);
            }
        }

        public void AddData(PBT pbData)
        {
            T data = new T();
            data.UpdateData(pbData);
            m_Data.Add(data);
        }

        public void RemoveData(int key)
        {
            T data = GetData(key);
            if (data == null)
            {
                return;
            }

            m_Data.Remove(data);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("m_Data: [");
            for (int i = 0; i < m_Data.Count; ++i)
            {
                if (i != 0)
                {
                    sb.Append(", ");
                }

                sb.Append(i.ToString());
                sb.Append(": {");
                sb.Append(m_Data[i] == null ? "null" : m_Data[i].ToString());
                sb.Append("}");
            }
            sb.Append("]");

            return sb.ToString();
        }
    }
}
