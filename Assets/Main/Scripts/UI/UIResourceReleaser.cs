using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class UIResourceReleaser
    {
        IList<int> spriteHashes = new List<int>();
        IList<int> textureHashes = new List<int>();

        public UIResourceReleaser()
        {

        }

        public void CollectWidgets(GameObject rootGO)
        {
            var sprites = rootGO.GetComponentsInChildren<UISprite>(true);
            for (int i = 0; i < sprites.Length; i++)
            {
                spriteHashes.Add(sprites[i].GetHashCode());
            }

            var textures = rootGO.GetComponentsInChildren<UITexture>(true);
            for (int i = 0; i < textures.Length; i++)
            {
                textureHashes.Add(textures[i].GetHashCode());
            }
        }

        public void ReleaseResources()
        {
            NGUIExtensionMethods.ReleaseAtlasesIfNeeded(spriteHashes);
            NGUIExtensionMethods.ReleaseTexturesIfNeeded(textureHashes);
        }
    }
}
