using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Genesis.GameClient.Editor
{
    [CustomEditor(typeof(PlayerPortrait))]
    [ExecuteInEditMode]
    public class PlayerPortraitInspector : UnityEditor.Editor
    {
        private float m_IconPersent = -1f;
        private bool m_ShowVip = true;

        public override void OnInspectorGUI()
        {
            var portrait = target as PlayerPortrait;

            m_ShowVip = portrait.VipLabel.transform.parent.gameObject.activeSelf;

            EditorGUILayout.BeginHorizontal("box");
            {
                EditorGUILayout.BeginVertical();
                {
                    portrait.PortraitSprite = EditorGUILayout.ObjectField("Portrait Sprite", portrait.PortraitSprite, typeof(UISprite), true) as UISprite;
                    portrait.PortraitBorderSprite = EditorGUILayout.ObjectField("Border Sprite", portrait.PortraitBorderSprite, typeof(UISprite), true) as UISprite;
                }
                EditorGUILayout.EndVertical();
                portrait.DistinguishOnlineStatus = EditorGUILayout.Toggle("Show Online/Offline", portrait.DistinguishOnlineStatus);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                portrait.PlayerNameLabel = EditorGUILayout.ObjectField("Name Label", portrait.PlayerNameLabel, typeof(UILabel), true) as UILabel;
                portrait.ShowLevelInName = EditorGUILayout.Toggle("Show Level In Name", portrait.ShowLevelInName);
            }
            EditorGUILayout.EndHorizontal();

            portrait.LevelLabel = EditorGUILayout.ObjectField("Level Label", portrait.LevelLabel, typeof(UILabel), true) as UILabel;

            EditorGUILayout.BeginHorizontal();
            {
                portrait.VipLabel = EditorGUILayout.ObjectField("VIP Label", portrait.VipLabel, typeof(UILabel), true) as UILabel;
                m_ShowVip = EditorGUILayout.Toggle("Show VIP Label", m_ShowVip);
                portrait.VipLabel.transform.parent.gameObject.SetActive(m_ShowVip);
            }
            EditorGUILayout.EndHorizontal();

            portrait.ShowMight = EditorGUILayout.Toggle("Show Might Label", portrait.ShowMight);
            if (portrait.ShowMight)
                portrait.MightLabel = EditorGUILayout.ObjectField("Might Label", portrait.MightLabel, typeof(UILabel), true) as UILabel;

            EditorGUILayout.BeginHorizontal("box");
            {
                if (GUILayout.Button("Large Icon"))
                {
                    SetIconSize(portrait, IconSize.Large);
                }

                if (GUILayout.Button("Middle Icon"))
                {
                    SetIconSize(portrait, IconSize.Middle);
                }

                if (GUILayout.Button("Small Icon"))
                {
                    SetIconSize(portrait, IconSize.Small);
                }
            }
            EditorGUILayout.EndHorizontal();

            if (m_IconPersent < 0)
            {
                m_IconPersent = portrait.PortraitSprite.GetComponent<UIWidget>().width / iconSizeMap[IconSize.Small].x;
            }

            EditorGUILayout.BeginHorizontal("box");
            {
                m_IconPersent = GUILayout.HorizontalSlider(m_IconPersent, 1f, 2.5f);
                GUILayout.TextField((m_IconPersent - 1).ToString("p"), GUILayout.Width(60));
            }
            EditorGUILayout.EndHorizontal();
            SetIconSize(portrait, iconSizeMap[IconSize.Small] * m_IconPersent, borderSizeMap[IconSize.Small] * m_IconPersent);
        }

        public void SetIconSize(PlayerPortrait portrait, Vector2 iconSize, Vector2 borderSize)
        {
            if (portrait.PortraitSprite == null)
            {
                Debug.LogError("Please ensure portrait sprite is not null.");
                return;
            }

            if (portrait.PortraitBorderSprite == null)
            {
                Debug.LogError("Please ensure portrait border sprite is not null.");
                return;
            }

            portrait.PortraitSprite.GetComponent<UIWidget>().SetDimensions((int)iconSize.x, (int)iconSize.y);
            portrait.PortraitBorderSprite.GetComponent<UIWidget>().SetDimensions((int)borderSize.x, (int)borderSize.y);
        }

        public void SetIconSize(PlayerPortrait portrait, IconSize size)
        {
            if (portrait.PortraitSprite == null)
            {
                Debug.LogError("Please ensure portrait sprite is not null.");
                return;
            }

            if (portrait.PortraitBorderSprite == null)
            {
                Debug.LogError("Please ensure portrait border sprite is not null.");
                return;
            }

            m_IconPersent = iconSizeMap[size].x / iconSizeMap[IconSize.Small].x;

            portrait.PortraitSprite.GetComponent<UIWidget>().SetDimensions((int)iconSizeMap[size].x, (int)iconSizeMap[size].y);
            portrait.PortraitBorderSprite.GetComponent<UIWidget>().SetDimensions((int)borderSizeMap[size].x, (int)borderSizeMap[size].y);
        }

        private Dictionary<IconSize, Vector2> iconSizeMap = new Dictionary<IconSize, Vector2>()
        {
            {IconSize.Large, new Vector2(111,121) },
            {IconSize.Middle, new Vector2(93,101) },
            {IconSize.Small, new Vector2(79,84) }
        };

        private Dictionary<IconSize, Vector2> borderSizeMap = new Dictionary<IconSize, Vector2>()
        {
            {IconSize.Large, new Vector2(110,111) },
            {IconSize.Middle, new Vector2(91,91) },
            {IconSize.Small, new Vector2(77,77) }
        };

        public enum IconSize
        {
            Small,
            Middle,
            Large
        }
    }

    public class PlayerPortraitEditorMenu : EditorWindow
    {
        [MenuItem("Game Framework/Generate UI/Portrait")]
        public static void AttachPortrait()
        {
            if(Selection.gameObjects.Length <= 0)
            {
                Debug.LogError("You should select an object first.");
                return;
            }

            if (Selection.gameObjects.Length > 1)
            {
                Debug.LogError("Only allow select one object.");
                return;
            }

            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Main/Prefabs/UI/Common/PlayerPortrait.prefab");
            var prefabInstance = Instantiate(prefab);

            prefabInstance.transform.name = "PlayerPortrait";
            prefabInstance.SetActive(true);
            prefabInstance.layer = LayerMask.NameToLayer("UI");
            prefabInstance.transform.SetParent(Selection.gameObjects[0].transform);
            prefabInstance.transform.localScale = Vector3.one;
        }
    }

}