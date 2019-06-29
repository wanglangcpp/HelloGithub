using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public class UIScrollViewCache<T> where T : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_ItemTemplate = null;

        [SerializeField]
        protected GameObject m_ItemParent = null;

        [SerializeField]
        protected UIScrollView m_ScrollView = null;

        private readonly List<T> m_CachedItems = new List<T>();
        private Mode m_Mode = Mode.Undetermined;
        private UIGrid m_Grid = null;
        private UITable m_Table = null;
        private GameFrameworkAction m_RepositionDelegate = null;
        private GameFrameworkFunc<List<Transform>> m_GetChildListDelegate = null;
        private GameFrameworkFunc<int> m_GetMaxRowCountInPanelDelegate = null;
        private GameFrameworkFunc<int> m_GetMaxColCountInPanelDelegate = null;

        public int Count
        {
            get
            {
                return m_CachedItems.Count;
            }
        }

        public T this[int index]
        {
            get
            {
                return m_CachedItems[index];
            }
        }

        public GameObject ItemTemplate
        {
            get
            {
                return m_ItemTemplate;
            }

            set
            {
                if (value != m_ItemTemplate)
                {
                    m_ItemTemplate = value;
                }
            }
        }

        public void SetActive(bool active)
        {
            m_ScrollView.gameObject.SetActive(active);
        }

        public T CreateItem()
        {
            if (!EnsureMode())
            {
                return null;
            }

            T item = NGUITools.AddChild(m_ItemParent, m_ItemTemplate).GetComponent<T>();
            m_CachedItems.Add(item);
            item.gameObject.SetActive(true);
            return item;
        }

        public T GetItem(int index)
        {
            if (!EnsureMode())
            {
                return null;
            }

            if (index < m_CachedItems.Count)
            {
                var item = m_CachedItems[index];
                item.gameObject.SetActive(true);
                return item;
            }

            return null;
        }

        public T GetOrCreateItem(int index, GameFrameworkAction<T> onCreate = null, GameFrameworkAction<T> onGet = null)
        {
            T item = GetItem(index);
            if (item != null)
            {
                if (onGet != null) onGet(item);
                return item;
            }

            item = CreateItem();
            if (onCreate != null) onCreate(item);
            return item;
        }

        public List<Transform> GetChildList()
        {
            if (!EnsureMode())
            {
                return null;
            }

            return m_GetChildListDelegate();
        }

        public void RecycleItemAt(int index)
        {
            if (!EnsureMode())
            {
                return;
            }

            if (index < m_CachedItems.Count)
            {
                m_CachedItems[index].gameObject.SetActive(false);
            }
        }

        public void RecycleItemsAtAndAfter(int index)
        {
            if (!EnsureMode())
            {
                return;
            }

            for (int i = index; i < m_CachedItems.Count; i++)
            {
                m_CachedItems[i].gameObject.SetActive(false);
            }
        }

        public void RecycleAllItems()
        {
            RecycleItemsAtAndAfter(0);
        }

        public void Reposition()
        {
            if (!EnsureMode())
            {
                return;
            }

            m_RepositionDelegate();
        }

        public void ResetPosition()
        {
            Reposition();
            m_ScrollView.ResetPosition();
        }

        public void InvalidateBounds()
        {
            Reposition();
            m_ScrollView.InvalidateBounds();
        }

        public void DestroyAllItems()
        {
            for (int i = 0; i < m_CachedItems.Count; ++i)
            {
                if (m_CachedItems[i] != null)
                {
                    UnityEngine.Object.Destroy(m_CachedItems[i].gameObject);
                }
            }

            m_CachedItems.Clear();
        }

        /// <summary>
        /// 获取能同时显示的最大列数。
        /// </summary>
        /// <returns>能同时显示的最大列数。</returns>
        public int GetMaxColumnCountInPanel()
        {
            if (!EnsureMode())
            {
                return -1;
            }

            return m_GetMaxColCountInPanelDelegate();
        }

        /// <summary>
        /// 获取能同时显示的最大行数。
        /// </summary>
        /// <returns>能同时显示的最大行数。</returns>
        public int GetMaxRowCountInPanel()
        {
            if (!EnsureMode())
            {
                return -1;
            }

            return m_GetMaxRowCountInPanelDelegate();
        }

        private int GetMaxRowCountInPanel_Grid()
        {
            float panelHeight = m_ScrollView.panel.height;
            return Mathf.FloorToInt(panelHeight / m_Grid.cellHeight);
        }

        private int GetMaxRowCountInPanel_Table()
        {
            float panelHeight = m_ScrollView.panel.height;
            var itemHeight = m_ItemTemplate.GetComponent<UIWidget>().height;
            var padding = m_Table.padding.y;
            return Mathf.FloorToInt((panelHeight + padding) / (itemHeight + padding));
        }

        private int GetMaxColCountInPanel_Grid()
        {
            float panelWidth = m_ScrollView.panel.width;
            return Mathf.FloorToInt(panelWidth / m_Grid.cellWidth);
        }

        private int GetMaxColCountInPanel_Table()
        {
            float panelWidth = m_ScrollView.panel.width;
            var itemWidth = m_ItemTemplate.GetComponent<UIWidget>().width;
            var padding = m_Table.padding.x;
            return Mathf.FloorToInt((panelWidth + padding) / (itemWidth + padding));
        }

        private bool EnsureMode()
        {
            if (m_Mode != Mode.Undetermined)
            {
                return true;
            }

            UIGrid uiGrid = m_ItemParent.GetComponent<UIGrid>();
            UITable uiTable = m_ItemParent.GetComponent<UITable>();
            if (uiGrid != null)
            {
                m_Grid = uiGrid;
                m_Mode = Mode.Grid;
                m_RepositionDelegate = m_Grid.Reposition;
                m_GetChildListDelegate = m_Grid.GetChildList;
                m_GetMaxRowCountInPanelDelegate = GetMaxRowCountInPanel_Grid;
                m_GetMaxColCountInPanelDelegate = GetMaxColCountInPanel_Grid;
            }
            else if (uiTable != null)
            {
                m_Table = uiTable;
                m_Mode = Mode.Table;
                m_RepositionDelegate = m_Table.Reposition;
                m_GetChildListDelegate = m_Table.GetChildList;
                m_GetMaxRowCountInPanelDelegate = GetMaxRowCountInPanel_Table;
                m_GetMaxColCountInPanelDelegate = GetMaxColCountInPanel_Table;
            }

            if (m_Mode == Mode.Undetermined)
            {
                Log.Error("Oops, I cannot determine my working mode! Are you sure you have a UIGrid or UITable?");
                return false;
            }

            return true;
        }

        private enum Mode
        {
            Undetermined,
            Grid,
            Table,
        }
    }
}
