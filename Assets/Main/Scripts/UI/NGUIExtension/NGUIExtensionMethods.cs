using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using Log = GameFramework.Log;

namespace Genesis.GameClient
{
    /// <summary>
    /// Extension methods for NGUI.
    /// </summary>
    public static class NGUIExtensionMethods
    {
        private static Dictionary<int, AsyncLoadTaskData> s_WidgetHashToAsyncLoadTaskData = new Dictionary<int, AsyncLoadTaskData>();
        private static Dictionary<int, UIAtlas> s_WidgetHashWithLoadedAtlases = new Dictionary<int, UIAtlas>();
        private static Dictionary<int, Texture> s_WidgetHashWithLoadedTextures = new Dictionary<int, Texture>();
        private static int s_AsyncLoadTaskId = 0;

        /// <summary>
        /// Clear any unfinished task and other cached data.
        /// </summary>
        public static void Reset()
        {
            s_WidgetHashToAsyncLoadTaskData.Clear();
            s_WidgetHashWithLoadedAtlases.Clear();
            s_WidgetHashWithLoadedTextures.Clear();
        }

        /// <summary>
        /// Get current drag amount of <see cref="UIScrollView"/>. Refer to <see cref="UIScrollView.SetDragAmount(float, float, bool)"/> in NGUI.
        /// </summary>
        /// <param name="scrollView">The given <see cref="UIScrollView"/> object.</param>
        /// <returns>Current drag amount.</returns>
        public static Vector2 GetDragAmount(this UIScrollView scrollView)
        {
            var panel = scrollView.panel;
            var clip = panel.finalClipRegion;
            var b = scrollView.bounds;
            float hx = clip.z * 0.5f;
            float hy = clip.w * 0.5f;
            float ox = clip.x;
            float oy = clip.y;

            float left = b.min.x + hx;
            float right = b.max.x - hx;
            float bottom = b.min.y + hy;
            float top = b.max.y - hy;

            if (panel.clipping == UIDrawCall.Clipping.SoftClip)
            {
                left -= panel.clipSoftness.x;
                right += panel.clipSoftness.x;
                bottom -= panel.clipSoftness.y;
                top += panel.clipSoftness.y;
            }

            float x = NumericalCalcUtility.CalcProportion(left, right, ox);
            float y = NumericalCalcUtility.CalcProportion(top, bottom, oy);
            return new Vector2(x, y);
        }

        /// <summary>
        /// Load sprite asynchronously by icon ID.
        /// </summary>
        /// <param name="sprite">The given sprite.</param>
        /// <param name="iconId">Icon ID.</param>
        /// <param name="onSuccess">Callback on load success.</param>
        /// <param name="onFailure">Callback on load failure.</param>
        /// <param name="onAbort">Callback on load abort.</param>
        /// <param name="userData">User data.</param>
        public static void LoadAsync(this UISprite sprite, int iconId,
            GameFrameworkAction<UISprite, string, object> onSuccess = null,
            GameFrameworkAction<UISprite, object> onFailure = null,
            GameFrameworkAction<UISprite, object> onAbort = null,
            object userData = null)
        {
            var dataTable = GameEntry.DataTable.GetDataTable<DRIcon>();
            DRIcon dataRow = dataTable.GetDataRow(iconId);
            if (dataRow == null)
            {
                Log.Warning("Icon ID '{0}' not found.", iconId);
                if (onFailure != null)
                {
                    onFailure(sprite, userData);
                }

                return;
            }

            LoadAsync(sprite, dataRow.AtlasName, dataRow.SpriteName, onSuccess, onFailure, onAbort, userData);
        }

        /// <summary>
        /// Load atlas for sprite asynchronously.
        /// </summary>
        /// <param name="sprite">The given sprite.</param>
        /// <param name="atlasPath">Atlas path relative to "Main/UI/Atlases/".</param>
        /// <param name="spriteName">Sprite name.</param>
        /// <param name="onSuccess">Callback on load success.</param>
        /// <param name="onFailure">Callback on load failure.</param>
        /// <param name="onAbort">Callback on load abort.</param>
        /// <param name="userData">User data.</param>
        public static void LoadAsync(this UISprite sprite, string atlasPath, string spriteName,
            GameFrameworkAction<UISprite, string, object> onSuccess = null,
            GameFrameworkAction<UISprite, object> onFailure = null,
            GameFrameworkAction<UISprite, object> onAbort = null,
            object userData = null)
        {
            if (sprite == null)
            {
                Log.Warning("Sprite is invalid.");
                return;
            }

            if (string.IsNullOrEmpty(atlasPath))
            {
                Log.Warning("Atlas path is invalid.");
                return;
            }

            if (string.IsNullOrEmpty(spriteName))
            {
                Log.Warning("Sprite name is invalid.");
                return;
            }

            string[] split = atlasPath.Split('/');
            string atlasName = split[split.Length - 1];

            if (string.IsNullOrEmpty(atlasName))
            {
                Log.Warning("Atlas name is invalid.");
                return;
            }

            // If the names are equal, we consider the wanted atlas is the sprite's current atlas.
            if (sprite.atlas != null && sprite.atlas.name == atlasName)
            {
                sprite.spriteName = spriteName;
                if (onSuccess != null) onSuccess(sprite, spriteName, userData);
                return;
            }

            int taskSerialId = s_AsyncLoadTaskId++;
            int widgetHash = sprite.GetHashCode();

            var internalUserData = new NGUIAtlasComponent.LoadAtlasData(OnLoadAtlasSuccess, OnLoadAtlasFailure, new SpriteData
            {
                TaskSerialId = taskSerialId,
                Sprite = sprite,
                WidgetHash = widgetHash,
                SpriteName = spriteName,
                OnSuccess = onSuccess,
                OnFailure = onFailure,
                OnAbort = onAbort,
                UserData = userData,
            });

            s_WidgetHashToAsyncLoadTaskData[widgetHash] = new AsyncLoadTaskData
            {
                TaskSerialId = taskSerialId,
                UserData = userData,
            };

            if (!GameEntry.IsAvailable)
            {
                return;
            }

            GameEntry.NGUIAtlas.LoadAtlas(atlasPath, internalUserData);
        }

        /// <summary>
        /// Load texture for <see cref="UITexture"/> asynchronously.
        /// </summary>
        /// <param name="texture">The given <see cref="UITexture"/> object.</param>
        /// <param name="textureId">texture ID from <see cref="Genesis.GameClient.DRUITexture"/></param>
        /// <param name="onSuccess">Callback on load success.</param>
        /// <param name="onFailure">Callback on load failure.</param>
        /// <param name="onAbort">Callback on load abort.</param>
        /// <param name="userData">User data.</param>
        public static void LoadAsync(this UITexture texture, int textureId,
            GameFrameworkAction<UITexture, object> onSuccess = null,
            GameFrameworkAction<UITexture, object> onFailure = null,
            GameFrameworkAction<UITexture, object> onAbort = null,
            object userData = null)
        {
            var dataTable = GameEntry.DataTable.GetDataTable<DRUITexture>();
            DRUITexture dataRow = dataTable.GetDataRow(textureId);
            if (dataRow == null)
            {
                Log.Warning("Icon ID '{0}' not found.", textureId);
                if (onFailure != null)
                {
                    onFailure(texture, userData);
                }

                return;
            }

            LoadAsync(texture, dataRow.TexturePath, onSuccess, onFailure, onAbort, userData);
        }

        /// <summary>
        /// Load texture for <see cref="UITexture"/> asynchronously.
        /// </summary>
        /// <param name="texture">The given <see cref="UITexture"/> object.</param>
        /// <param name="texturePath">Texture path relative to "Main/UI/Textures/UITextures".</param>
        /// <param name="onSuccess">Callback on load success.</param>
        /// <param name="onFailure">Callback on load failure.</param>
        /// <param name="onAbort">Callback on load abort.</param>
        /// <param name="userData">User data.</param>
        public static void LoadAsync(this UITexture texture, string texturePath,
            GameFrameworkAction<UITexture, object> onSuccess = null,
            GameFrameworkAction<UITexture, object> onFailure = null,
            GameFrameworkAction<UITexture, object> onAbort = null,
            object userData = null)
        {
            if (texture == null)
            {
                Log.Warning("Texture is invalid.");
                return;
            }

            if (string.IsNullOrEmpty(texturePath))
            {
                Log.Warning("Resource path is invalid.");
            }

            int taskSerialId = s_AsyncLoadTaskId++;
            int widgetHash = texture.GetHashCode();

            var textureData = new TextureData
            {
                TaskSerialId = taskSerialId,
                Texture = texture,
                WidgetHash = widgetHash,
                OnSuccess = onSuccess,
                OnFailure = onFailure,
                OnAbort = onAbort,
                UserData = userData,
            };

            s_WidgetHashToAsyncLoadTaskData[widgetHash] = new AsyncLoadTaskData
            {
                TaskSerialId = taskSerialId,
                UserData = userData,
            };

            if (!GameEntry.IsAvailable)
            {
                return;
            }

            GameEntry.Texture.LoadTexture(texturePath, OnLoadTextureSuccess, OnLoadTextureFailure, textureData);
        }

        public static void ReleaseAtlasesIfNeeded(IList<int> spriteHashes)
        {
            if (spriteHashes == null) return;

            for (int i = 0; i < spriteHashes.Count; i++)
            {
                int widgetHash = spriteHashes[i];
                ReleaseAtlasIfNeeded(widgetHash);
            }
        }

        public static void ReleaseAtlasIfNeeded(int spriteHash)
        {
            if (!GameEntry.IsAvailable) return;

            UIAtlas atlas;
            if (!s_WidgetHashWithLoadedAtlases.TryGetValue(spriteHash, out atlas))
            {
                return;
            }

            GameEntry.NGUIAtlas.UnloadAtlas(atlas.gameObject);
            s_WidgetHashWithLoadedAtlases.Remove(spriteHash);
        }

        public static void ReleaseTexturesIfNeeded(IList<int> textureHashes)
        {
            if (textureHashes == null) return;

            for (int i = 0; i < textureHashes.Count; i++)
            {
                int widgetHash = textureHashes[i];
                ReleaseTextureIfNeeded(widgetHash);
            }
        }

        public static void ReleaseTextureIfNeeded(int textureHash)
        {
            if (!GameEntry.IsAvailable) return;

            Texture textureAsset;
            if (!s_WidgetHashWithLoadedTextures.TryGetValue(textureHash, out textureAsset))
            {
                return;
            }

            s_WidgetHashWithLoadedTextures.Remove(textureHash);
            GameEntry.Texture.UnloadTexture(textureAsset);
        }

        /// <summary>
        /// Center on a child immediately.
        /// </summary>
        /// <param name="centerOnChild">Center on child component.</param>
        /// <param name="target">The target child transform.</param>
        /// <param name="scrollView">The scroll view involved.</param>
        public static void CenterOnImmediately(this UICenterOnChild centerOnChild, Transform target, UIScrollView scrollView)
        {
            scrollView.TryCenterOn(target);
            centerOnChild.CenterOn(target);
        }

        /// <summary>
        /// Try center on a child.
        /// </summary>
        /// <param name="scrollView">The scroll view involved.</param>
        /// <param name="target">The target child transform.</param>
        /// <param name="restrictWithinBounds">Whether the content should be restricted within the bounds of the panel.</param>
        public static void TryCenterOn(this UIScrollView scrollView, Transform target, bool restrictWithinBounds = false)
        {
            var panel = scrollView.panel;
            Vector3[] corners = panel.worldCorners;
            Vector3 panelCenter = (corners[2] + corners[0]) * 0.5f;

            Transform panelTrans = panel.cachedTransform;
            Vector3 cp = panelTrans.InverseTransformPoint(target.position);
            Vector3 cc = panelTrans.InverseTransformPoint(panelCenter);
            Vector3 localOffset = cp - cc;

            if (!scrollView.canMoveHorizontally) localOffset.x = 0f;
            if (!scrollView.canMoveVertically) localOffset.y = 0f;
            localOffset.z = 0f;

            panelTrans.localPosition = panelTrans.localPosition - localOffset;

            Vector2 co = panel.clipOffset;
            co.x += localOffset.x;
            co.y += localOffset.y;
            scrollView.panel.clipOffset = co;

            if (restrictWithinBounds)
            {
                scrollView.RestrictWithinBounds(true);
            }
        }

        private static void OnLoadAtlasSuccess(string assetName, object asset, object userData)
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            var spriteData = userData as SpriteData;
            if (spriteData == null)
            {
                Log.Warning("Sprite data is invalid.");
                return;
            }

            UISprite sprite = spriteData.Sprite;
            string spriteName = spriteData.SpriteName;

            var atlasPrefab = asset as GameObject;
            var atlasScript = atlasPrefab.GetComponent<UIAtlas>();

            if (AbortSpriteLoadingIfNeeded(spriteData))
            {
                GameEntry.NGUIAtlas.UnloadAtlas(atlasPrefab);
                return;
            }

            s_WidgetHashToAsyncLoadTaskData.Remove(spriteData.WidgetHash);

            if (sprite != null)
            {
                UIAtlas oldAtlas;
                if (s_WidgetHashWithLoadedAtlases.TryGetValue(spriteData.WidgetHash, out oldAtlas))
                {
                    GameEntry.NGUIAtlas.UnloadAtlas(oldAtlas.gameObject);
                }

                sprite.atlas = s_WidgetHashWithLoadedAtlases[spriteData.WidgetHash] = atlasScript;
                sprite.spriteName = spriteName;
            }

            if (spriteData.OnSuccess != null)
            {
                spriteData.OnSuccess(sprite, spriteName, spriteData.UserData);
            }
        }

        private static void OnLoadAtlasFailure(string assetName, string errorMessage, object userData)
        {
            var spriteData = userData as SpriteData;
            if (spriteData == null)
            {
                Log.Warning("User data is invalid.");
                return;
            }

            if (AbortSpriteLoadingIfNeeded(spriteData))
            {
                return;
            }

            s_WidgetHashToAsyncLoadTaskData.Remove(spriteData.WidgetHash);
            if (spriteData.OnFailure != null)
            {
                spriteData.OnFailure(spriteData.Sprite, spriteData.UserData);
            }
        }

        private static void OnLoadTextureSuccess(string assetName, object asset, object userData)
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            var textureData = userData as TextureData;
            if (textureData == null)
            {
                Log.Warning("Texture data is invalid.");
                return;
            }

            if (AbortTextureLoadingIfNeeded(textureData))
            {
                GameEntry.Texture.UnloadTexture(asset as Texture);
                return;
            }

            s_WidgetHashToAsyncLoadTaskData.Remove(textureData.WidgetHash);

            if (textureData.Texture != null)
            {
                Texture oldTexture;
                if (s_WidgetHashWithLoadedTextures.TryGetValue(textureData.WidgetHash, out oldTexture))
                {
                    GameEntry.Texture.UnloadTexture(oldTexture);
                }

                textureData.Texture.mainTexture = s_WidgetHashWithLoadedTextures[textureData.WidgetHash] = asset as Texture;
            }

            if (textureData.OnSuccess != null)
            {
                textureData.OnSuccess(textureData.Texture, textureData.UserData);
            }
        }

        private static void OnLoadTextureFailure(string assetName, string errorMessage, object userData)
        {
            var textureData = userData as TextureData;
            if (textureData == null)
            {
                Log.Warning("Texture data is invalid.");
                return;
            }

            if (AbortTextureLoadingIfNeeded(textureData))
            {
                textureData.Texture = null;
                return;
            }

            s_WidgetHashToAsyncLoadTaskData.Remove(textureData.WidgetHash);
            if (textureData.OnFailure != null)
            {
                textureData.OnFailure(textureData.Texture, textureData.UserData);
            }
        }

        private static bool AbortSpriteLoadingIfNeeded(SpriteData spriteData)
        {
            if (spriteData.Sprite == null)
            {
                return false;
            }

            int widgetHash = spriteData.Sprite.GetHashCode();
            AsyncLoadTaskData taskData;
            if (s_WidgetHashToAsyncLoadTaskData.TryGetValue(widgetHash, out taskData) && taskData.TaskSerialId != spriteData.TaskSerialId)
            {
                if (spriteData.OnAbort != null)
                {
                    spriteData.OnAbort(spriteData.Sprite, spriteData.UserData);
                }

                return true;
            }

            return false;
        }

        private static bool AbortTextureLoadingIfNeeded(TextureData textureData)
        {
            if (textureData.Texture == null)
            {
                return false;
            }

            int widgetHash = textureData.Texture.GetHashCode();
            AsyncLoadTaskData taskData;
            if (s_WidgetHashToAsyncLoadTaskData.TryGetValue(widgetHash, out taskData) && taskData.TaskSerialId != textureData.TaskSerialId)
            {
                if (textureData.OnAbort != null)
                {
                    textureData.OnAbort(textureData.Texture, textureData.UserData);
                }

                return true;
            }

            return false;
        }

        private abstract class WidgetData
        {
            public int TaskSerialId { get; set; }
            public int WidgetHash { get; set; }
        }

        private class SpriteData : WidgetData
        {
            public UISprite Sprite { get; set; }
            public string SpriteName { get; set; }
            public object UserData { get; set; }
            public GameFrameworkAction<UISprite, string, object> OnSuccess;
            public GameFrameworkAction<UISprite, object> OnFailure;
            public GameFrameworkAction<UISprite, object> OnAbort;
        }

        private class TextureData : WidgetData
        {
            public UITexture Texture { get; set; }
            public object UserData { get; set; }
            public GameFrameworkAction<UITexture, object> OnSuccess;
            public GameFrameworkAction<UITexture, object> OnFailure;
            public GameFrameworkAction<UITexture, object> OnAbort;
        }

        private class AsyncLoadTaskData
        {
            public int TaskSerialId { get; set; }
            public object UserData { get; set; }
        }
    }
}
