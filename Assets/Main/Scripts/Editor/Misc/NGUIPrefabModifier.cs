using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Genesis.GameClient.Editor
{
    /// <summary>
    /// NGUI Prefab 修改器。
    /// </summary>
    public static class NGUIPrefabModifier
    {
        [MenuItem("Assets/Automatically Modify NGUI Prefab")]
        public static void CheckAndModify()
        {
            var prefabs = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets).ToList().ConvertAll<GameObject>(o => o as GameObject);

            foreach (var prefab in prefabs)
            {
                CheckAndModify(prefab);
            }
        }

        [MenuItem("Assets/Automatically Modify NGUI Prefab", validate = true)]
        public static bool Validate()
        {
            var prefabs = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);
            return prefabs.Length > 0;
        }

        private static void CheckAndModify(GameObject prefab)
        {
            var path = AssetDatabase.GetAssetPath(prefab);
            Debug.LogFormat("[NGUIPrefabModifier CheckAndModify] prefab being checked: {0}.", path);

            var changed = false;

            changed |= CheckButtonColliders(prefab);
            changed |= CheckAnchorExecution(prefab);
            changed |= PreventAnimFromAutoPlay(prefab);
            changed |= ProcessScrollViews(prefab);

            if (changed)
            {
                EditorUtility.SetDirty(prefab);
                AssetDatabase.SaveAssets();
                Debug.LogFormat("<color=white>Prefab modified and saved.</color>");
            }
            else
            {
                Debug.LogFormat("Prefab not modified.", path);
            }
        }

        private static bool ProcessScrollViews(GameObject prefab)
        {
            var changed = false;

            var scrollViews = prefab.GetComponentsInChildren<UIScrollView>(true);
            foreach (var scrollView in scrollViews)
            {
                if (scrollView.scrollWheelFactor != 0f)
                {
                    scrollView.scrollWheelFactor = 0f;
                    changed = true;
                }
            }

            return changed;
        }

        private static bool CheckAnchorExecution(GameObject prefab)
        {
            var changed = false;

            var rects = prefab.GetComponentsInChildren<UIRect>(true);
            foreach (var rect in rects)
            {
                if (rect.updateAnchors != UIRect.AnchorUpdate.OnEnable)
                {
                    rect.updateAnchors = UIRect.AnchorUpdate.OnEnable;
                    changed = true;
                }
            }

            return changed;
        }

        private static bool CheckButtonColliders(GameObject prefab)
        {
            var changed = false;

            var buttons = prefab.GetComponentsInChildren<UIButton>(true);
            foreach (UIButton button in buttons)
            {
                var tweenTarget = button.tweenTarget;
                if (tweenTarget == null)
                {
                    continue;
                }

                var sprite = tweenTarget.GetComponent<UISprite>();
                if (sprite == null)
                {
                    continue;
                }

                var collider = button.GetComponent<BoxCollider>();
                if (collider == null)
                {
                    continue;
                }

                if (!sprite.autoResizeBoxCollider && (collider.size.x < sprite.width || collider.size.y < sprite.height))
                {
                    sprite.autoResizeBoxCollider = true;
                    changed = true;
                }
            }
            return changed;
        }

        private static bool PreventAnimFromAutoPlay(GameObject prefab)
        {
            var changed = false;

            var anims = prefab.GetComponentsInChildren<Animation>(true);
            foreach (var anim in anims)
            {
                if (anim.playAutomatically)
                {
                    anim.playAutomatically = false;
                    changed = true;
                }
            }

            return changed;
        }
    }
}
