using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public class DragStartEventArgs : GameEventArgs
    {
        public DragStartEventArgs(Vector2 startPosition, Vector2 swipeVector, Vector2 deltaPosition)
        {
            StartPosition = startPosition;
            SwipeVector = swipeVector;
            DeltaPosition = deltaPosition;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.DragStart;
            }
        }

        public Vector2 StartPosition { get; private set; }
        public Vector2 SwipeVector { get; private set; }
        public Vector2 DeltaPosition { get; private set; }
    }
}
