using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 简单点击事件。
    /// </summary>
    public class SimpleTapEventArgs : GameEventArgs
    {
        public SimpleTapEventArgs(Vector2 position)
        {
            Position = position;
        }

        public override int Id
        {
            get
            {
                return (int)EventId.SimpleTap;
            }
        }

        public Vector2 Position { get; private set; }
    }
}
