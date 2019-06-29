using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 手势 -- 触摸抬起事件。
    /// </summary>
    public class TouchUpEventArgs : GameEventArgs
    {
        public TouchUpEventArgs(Vector2 position)
        {
            Position = position;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.TouchUp;
            }
        }

        public Vector2 Position { get; private set; }
    }
}
