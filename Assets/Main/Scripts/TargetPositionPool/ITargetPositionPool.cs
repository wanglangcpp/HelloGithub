using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public interface ITargetPositionPool
    {
        void AddTargetPositions(IList<Vector3> positions);

        void ClearTargetPositions();

        Vector3 SelectTargetPosition();
    }
}
