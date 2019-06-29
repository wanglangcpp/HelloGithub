using GameFramework.Event;

namespace Genesis.GameClient
{
    internal class SelectEpigraphEventArgs : GameEventArgs
    {
        public SelectEpigraphEventArgs(int id, int index)
        {
            EpigraphId = id;
            Index = index;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.SelectEpigraph;
            }
        }

        public int EpigraphId
        {
            get;
            set;
        }

        public int Index
        {
            get;
            set;
        }
    }
}
