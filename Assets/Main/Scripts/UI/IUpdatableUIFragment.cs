using UnityEngine;

namespace Genesis.GameClient
{
    public interface IUpdatableUIFragment
    {
        UIInvoke LastUIInvoke { get; }

        bool DataChanged { get; }

        void ClearLastUIInvoke();

        void ClearDataChanged();

        bool TryGetData(string key, out string value);

        GameObject GetGameObject(string gameObjectPath);

        UIEffectsController EffectsController { get; }
    }
}
