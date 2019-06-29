using System.Collections.Generic;

namespace Genesis.GameClient
{
    /// <summary>
    /// 获取英雄，获取物品，获取装备界面共同使用。
    /// </summary>
    public class ReceiveData : UIFormBaseUserData
    {
        private List<KeyValuePair<int, int>> m_ShowDatas = new List<KeyValuePair<int, int>>();

        public void AddData(int id, int count)
        {
            m_ShowDatas.Add(new KeyValuePair<int, int>(id, count));
        }

        public List<KeyValuePair<int, int>> Datas
        {
            get
            {
                return m_ShowDatas;
            }
        }

        public int Count { get { return m_ShowDatas.Count; } }
    }
}
