using System.Collections.Generic;

namespace Genesis.GameClient
{
    public interface IBuffTargetData
    {
        IList<BuffData> Buffs
        {
            get;
        }

        void OnBuffPoolChanged(BuffData added, IList<BuffData> removed);

        void OnBuffHeartBeat(long serialId, BuffData buff);
    }
}
