using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Genesis.GameClient
{
    /// <summary>
    /// Boss副本章节
    /// </summary>
    public class BossChapterInstance : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_InterActGo = null;
        [SerializeField]
        private List<BossItem> m_Levels = null;
        [SerializeField]
        private float duration = 0.5f;

        private Dictionary<int, PointData> m_LevelMap;

        Vector3 startMousePos;
        public Action<BossItem> selectedBossChanged;
        void Awake()
        {

        }
        public void SetData(DRInstanceGroupForBoss drBoss, int instanceType)
        {
            if (m_LevelMap == null)
            {
                StartCoroutine(InitDepth());
            }

            List<DRInstance> drInstances = GameEntry.Data.InstanceForBossData.GetDatas(drBoss.Id, instanceType);
            if (drInstances.Count != 3)
            {
                return;
            }
            drInstances.Sort((s1, s2) => { return s1.Id.CompareTo(s2.Id); });
            m_Levels[0].SetData(drInstances[2], drBoss.BossTextureId3);
            m_Levels[1].SetData(drInstances[0], drBoss.BossTextureId1);
            m_Levels[2].SetData(drInstances[1], drBoss.BossTextureId2);
        }

        IEnumerator InitDepth()
        {
            yield return null;
            UIEventListener.Get(m_InterActGo).onDragEnd = OnDragEnd;
            UIEventListener.Get(m_InterActGo).onDragStart = OnDragStart;

            Vector2 pos = m_Levels[0].transform.localPosition;
            float alpha = m_Levels[0].GetComponent<UIPanel>().alpha;
            m_LevelMap = new Dictionary<int, PointData>();
            for (int i = 0; i < m_Levels.Count; i++)
            {
                m_Levels[i].CurrentIndex = i;
                UIPanel panel = m_Levels[i].GetComponent<UIPanel>();
                m_LevelMap.Add(i, new PointData(m_Levels[i].transform.localPosition, panel.alpha, m_Levels[i].transform.localScale, panel.depth));
            }
        }
        private void OnDragStart(GameObject go)
        {
            startMousePos = Input.mousePosition;
        }

        private void OnDragEnd(GameObject go)
        {
            Vector2 offset = Input.mousePosition - startMousePos;
            if (offset.x > 0)
            {
                ChangeBossItemState(true);
            }
            else
            {
                ChangeBossItemState(false);
            }
        }
        void ChangeBossItemState(bool isRight)
        {
            int count = m_Levels.Count;
            foreach (var item in m_Levels)
            {
                if (item.IsChanging)
                {
                    return;
                }
            }
            for (int i = 0; i < count; i++)
            {
                BossItem bossItem = m_Levels[i];

                int targetIndex = isRight ? (bossItem.CurrentIndex + 1) >= count ? 0 : (bossItem.CurrentIndex + 1) : (bossItem.CurrentIndex - 1) < 0 ? count - 1 : (bossItem.CurrentIndex - 1);

                if (targetIndex == 1 && selectedBossChanged != null)
                {
                    selectedBossChanged(bossItem);
                }
                Vector2 targetPos = m_LevelMap[targetIndex].localPos;
                float targetAlpha = m_LevelMap[targetIndex].alpha;
                Vector3 targetScale = m_LevelMap[targetIndex].localScale;
                int targetDepth = m_LevelMap[targetIndex].depth;
                bossItem.ChangeShowIndex(targetIndex, duration, targetPos, targetAlpha, targetScale, targetDepth);
            }
        }

        class PointData
        {
            public Vector2 localPos;
            public float alpha;
            public Vector3 localScale;
            public int depth;
            public PointData(Vector2 p, float a, Vector3 s, int d)
            {
                localPos = p;
                alpha = a;
                localScale = s;
                depth = d;
            }
        }

    }
}


