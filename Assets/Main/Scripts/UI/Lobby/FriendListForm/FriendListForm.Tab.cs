using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class FriendListForm
    {
        [Serializable]
        private class Tab
        {
            public UIToggle TabToggle = null;
            public UISprite RedDot = null;
            public Transform Content = null;
        }
    }
}
