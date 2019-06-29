using System;

namespace Genesis.GameClient
{
    [Serializable]
    public class InstanceDropsData : GenericData<InstanceDropData, PBDropInfo>
    {
        public int DropGoodsCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < Data.Count; i++)
                {
                    count += Data[i].Count;
                }

                return count;
            }
        }
    }
}
