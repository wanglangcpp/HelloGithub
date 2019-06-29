using UnityEngine;
using System.Collections;
using LuaInterface;

namespace Genesis.GameClient
{
    public static class LuaUtility
    {
        #region UI

        public static void LoadUISpriteAsyncByIconId(UISprite sprite, int iconId)
        {
            sprite.LoadAsync(iconId);
        }

        #endregion UI

        public static void Reset()
        {

        }
    }
}
