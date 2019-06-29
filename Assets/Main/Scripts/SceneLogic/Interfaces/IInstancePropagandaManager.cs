using GameFramework;

namespace Genesis.GameClient
{
    public interface IInstancePropagandaManager
    {
        event GameFrameworkAction<InstancePropagandaData> OnPropagandaBegin;

        event GameFrameworkAction OnPropagandaEnd;

        InstancePropagandaData Current { get; }

        void Add(InstancePropagandaData data);

        void Update(float realElapseTime);

        void Clear();
    }
}
