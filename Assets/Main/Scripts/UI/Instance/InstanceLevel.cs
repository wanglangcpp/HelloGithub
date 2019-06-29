using UnityEngine;
using System.Collections.Generic;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// 副本关卡
    /// </summary>
#if UNITY_EDITOR
    [ExecuteInEditMode]
#endif
    public class InstanceLevel : MonoBehaviour
    {
        public enum LevelType
        {
            /// <summary>
            /// 剧情副本
            /// </summary>
            Ordinary = 0,
            /// <summary>
            /// 精英副本
            /// </summary>
            Elite = 1,
            /// <summary>
            /// boss副本
            /// </summary>
            BossOridinary = 2,
            /// <summary>
            /// boss精英副本
            /// </summary>
            BossElite = 3,
            /// <summary>
            /// 爬塔副本
            /// </summary>
            Tower = 4,
        }

        private const string PathDisabledSmall = "Path2";
        private const string PathDisabledLarge = "Path1";
        private const string PathEnabledSmall = "Path4";
        private const string PathEnabledLarge = "Path3";
        #region UNITY_EDITOR 
#if UNITY_EDITOR
        [ContextMenu("Binding Objects")]
        public void BindingObjects()
        {
            for (int i = 0; i < UnityEditor.Selection.gameObjects.Length; i++)
            {
                GameObject root = UnityEditor.Selection.gameObjects[i];

                m_NextLevelPathRootObject = FindObject(root.transform, "Path Root");
                m_ArrowObject = FindObject(root.transform, "Arrow Remind");

                m_OrdinaryLevelIcon = new LevelIcon();
                m_OrdinaryLevelIcon.BindingObject(true, root);
                m_EliteLevelIcon = new LevelIcon();
                m_EliteLevelIcon.BindingObject(false, root);
                m_StarObjectList = new List<GameObject>();
                m_StarObjectList.Add(FindObject(root.transform, "Star List/Star Icon1"));
                m_StarObjectList.Add(FindObject(root.transform, "Star List/Star Icon2"));
                m_StarObjectList.Add(FindObject(root.transform, "Star List/Star Icon3"));
            }
        }

        private GameObject FindObject(Transform p, string name)
        {
            var res = p.transform.FindChild(name);
            if (res == null)
            {
                Debug.LogError("can not find 【" + name + "】");
                return null;
            }

            return res.gameObject;
        }
#endif
        #endregion
        [Serializable]
        public class LevelIcon
        {
            #region UNITY_EDITOR


#if UNITY_EDITOR
            public void BindingObject(bool isOrdinary, GameObject root)
            {
                if (isOrdinary)
                {
                    RootObject = FindObject(root.transform, "Instance Common");
                    m_LevelOpenObject = FindObject(root.transform, "Instance Common/Common Icon");
                    m_LevelCloseObject = FindObject(root.transform, "Instance Common/Lock Icon");
                    m_HeroIconObject = FindObject(root.transform, "Instance Common/Hero Icon");
                    m_HeroIconSprite = FindObject(root.transform, "Instance Common/Hero Icon/Hero Border").GetComponent<UISprite>();
                    m_HeroAttributeSprite = FindObject(root.transform, "Instance Common/Hero Icon/Element Icon").GetComponent<UISprite>();
                }
                else
                {
                    RootObject = FindObject(root.transform, "Instance Boss");
                    m_LevelOpenObject = FindObject(root.transform, "Instance Boss/Boss Icon");
                    m_LevelCloseObject = FindObject(root.transform, "Instance Boss/Boss Lock");
                    m_HeroIconObject = FindObject(root.transform, "Instance Boss/Hero Icon");
                    m_HeroIconSprite = FindObject(root.transform, "Instance Boss/Hero Icon/Hero Border").GetComponent<UISprite>();
                    m_HeroAttributeSprite = FindObject(root.transform, "Instance Boss/Hero Icon/Element Icon").GetComponent<UISprite>();
                }

                m_OpenHintLabel = FindObject(root.transform, "Need Level Open Bg/Level Text").GetComponent<UILabel>();
            }

            private GameObject FindObject(Transform p, string name)
            {
                var res = p.transform.FindChild(name);
                if (res == null)
                {
                    Debug.LogError("can not find 【" + name + "】");
                    return null;
                }

                return res.gameObject;
            }
#endif
            #endregion
            public GameObject RootObject = null;

            [SerializeField]
            private GameObject m_LevelOpenObject = null;

            [SerializeField]
            private GameObject m_LevelCloseObject = null;

            [SerializeField]
            private GameObject m_HeroIconObject = null;

            [SerializeField]
            private UISprite m_HeroIconSprite = null;

            [SerializeField]
            private UISprite m_HeroAttributeSprite = null;

            [SerializeField]
            private UILabel m_OpenHintLabel = null;

            public void SetActing(bool enable)
            {
                RootObject.SetActive(enable);
            }

            public void SetLevel(InstanceGroupData.InstanceData levelData)
            {
                SetIconStatus(levelData.IsOpen);
                SetHintStatus(levelData.NeedShowHint, levelData.LevelConfig.PrerequisitePlayerLevel);
                // TODO: 这里Prefab的层级有问题，由于数量太大了，所以就没改，下面的逻辑显得有点怪。有时间再改这里。
                if (levelData.NpcInfo != null)
                {
                    m_HeroAttributeSprite.spriteName = UIUtility.GetElementSpriteName(levelData.NpcInfo.ElementId);
                    m_HeroAttributeSprite.gameObject.SetActive(true);
                }
                else
                {
                    m_HeroAttributeSprite.gameObject.SetActive(false);
                }

                if (levelData.NeedShowNpcIcon)
                {
                    if (levelData.NpcInfo != null)
                    {
                        m_HeroIconSprite.enabled = true;
                        m_HeroIconObject.GetComponent<UISprite>().enabled = true;

                        var icons = GameEntry.DataTable.GetDataTable<DRIcon>();
                        DRIcon row = icons.GetDataRow(levelData.NpcInfo.IconId);
                        if (row != null)
                            m_HeroIconObject.GetComponent<UISprite>().spriteName = row.SpriteName;

                        SetHeroIconStatus(true);
                    }
                }
                else
                {
                    m_HeroIconObject.GetComponent<UISprite>().enabled = false;
                    m_HeroIconSprite.enabled = false;
                    SetHeroIconStatus(false);
                }
            }

            public void SetIconStatus(bool isOpen)
            {
                m_LevelOpenObject.SetActive(isOpen);
                m_LevelCloseObject.SetActive(!isOpen);
            }

            public void SetHintStatus(bool needShow, int level)
            {
                if (needShow)
                {
                    m_OpenHintLabel.transform.parent.gameObject.SetActive(true);
                    m_OpenHintLabel.text = GameEntry.Localization.GetString("UI_TEXT_NEED_LEVEL_OPEN", level);
                }
                else
                {
                    m_OpenHintLabel.transform.parent.gameObject.SetActive(false);
                }
            }

            public void SetHeroIconStatus(bool isShow)
            {
                m_HeroIconObject.SetActive(isShow);
            }
        }

        /// <summary>
        /// 路径是开启自己的路径（从上一个关卡开启自己走的路径），也就是第一关没有路径
        /// </summary>
        [SerializeField]
        private GameObject m_NextLevelPathRootObject = null;

        [SerializeField]
        private List<GameObject> m_StarObjectList = null;

        [SerializeField]
        private LevelIcon m_OrdinaryLevelIcon = null;

        [SerializeField]
        private LevelIcon m_EliteLevelIcon = null;

        [SerializeField]
        private GameObject m_ArrowObject = null;

        private InstanceGroupData.InstanceData m_LevelData;
        public InstanceGroupData.InstanceData LevelData
        {
            get { return m_LevelData; }
        }

        public void InitializeLevel()
        {
            m_OrdinaryLevelIcon.SetActing(true);
            m_OrdinaryLevelIcon.SetIconStatus(false);
            m_OrdinaryLevelIcon.SetHeroIconStatus(false);
            m_EliteLevelIcon.SetActing(true);
            m_EliteLevelIcon.SetIconStatus(false);
            m_EliteLevelIcon.SetHeroIconStatus(false);
            m_ArrowObject.SetActive(false);
            m_OrdinaryLevelIcon.SetHintStatus(false, 0);
            m_EliteLevelIcon.SetHintStatus(false, 0);
            SetPathStatus(false);

            for (int i = 0; i < m_StarObjectList.Count; i++)
                m_StarObjectList[i].SetActive(false);
        }

        public void SetLevelData(InstanceGroupData.InstanceData levelData)
        {
            m_LevelData = levelData;

            LevelIcon levelIcon = null;
            if (levelData.LevelConfig.InstanceType == (int)LevelType.Ordinary)
            {
                m_OrdinaryLevelIcon.SetActing(true);
                m_EliteLevelIcon.SetActing(false);
                levelIcon = m_OrdinaryLevelIcon;
            }
            else if (levelData.LevelConfig.InstanceType == (int)LevelType.Elite)
            {
                m_OrdinaryLevelIcon.SetActing(false);
                m_EliteLevelIcon.SetActing(true);
                levelIcon = m_EliteLevelIcon;
            }

            levelIcon.SetLevel(levelData);

            if (levelData.StarCount > 0)
            {
                for (int i = 0; i < m_StarObjectList.Count; i++)
                {
                    if (levelData.StarCount > i)
                        m_StarObjectList[i].GetComponent<UISprite>().color = Color.white;
                    else
                        m_StarObjectList[i].GetComponent<UISprite>().color = new Color(100 / 255f, 100 / 255f, 100 / 255f, 255 / 255);

                    m_StarObjectList[i].SetActive(true);
                }
            }
            m_ArrowObject.SetActive(levelData.IsOpen && !levelData.IsDone);

            // 当前关开启了
            SetPathStatus(levelData.IsOpen);

            UIEventListener.Get(gameObject).onClick = OnLevelIconClicked;
        }

        public void OnLevelIconClicked(GameObject go)
        {
            if (m_LevelData.IsOpen)
            {
                GameEntry.UI.OpenUIForm(UIFormId.InstanceInfoForm, new InstanceInfoDisplayData { InstanceId = m_LevelData.Id });
            }
            //else
            //{
            //    GameEntry.UI.OpenUIForm(UIFormId.Toast, new ToastDisplayData { Message = GameEntry.Localization.GetString("UI_TEXT_INSTANCE_LEVEL_CAN_NOT_OPEN_NEW_SECTION", m_LevelData.LevelConfig.PrerequisitePlayerLevel) });
            //}
        }

        public void SetPathStatus(bool enabled)
        {
            if (m_NextLevelPathRootObject == null)
                return;

            var pathItems = m_NextLevelPathRootObject.GetComponentsInChildren<UISprite>();

            for (int i = 0; i < pathItems.Length; i++)
            {
                string spriteName = pathItems[i].spriteName;
                pathItems[i].spriteName = m_ExchangeMap[spriteName].GetStatusValue(enabled);
            }
        }

        private struct ExchangeData
        {
            public ExchangeData(string enabledValue, string disabledValue)
            {
                EnabledValue = enabledValue;
                DisabledValue = disabledValue;
            }
            public string EnabledValue { get; private set; }
            public string DisabledValue { get; private set; }
            public string GetStatusValue(bool isEnable)
            {
                return isEnable ? EnabledValue : DisabledValue;
            }
        }

        private Dictionary<string, ExchangeData> m_ExchangeMap = new Dictionary<string, ExchangeData>()
        {
            { PathDisabledSmall, new ExchangeData(PathEnabledSmall, PathDisabledSmall) },
            { PathDisabledLarge, new ExchangeData(PathEnabledLarge, PathDisabledLarge) },
            { PathEnabledSmall, new ExchangeData(PathEnabledSmall, PathDisabledSmall) },
            { PathEnabledLarge, new ExchangeData(PathEnabledLarge, PathDisabledLarge) }
        };
    }
}