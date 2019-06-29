using GameFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BaseInstanceLogic
    {
        [SerializeField]
        protected GuidePointSetManager m_GuidePointSet = null;

        public GuidePointSetManager GuidePointSet
        {
            get
            {
                return m_GuidePointSet;
            }
        }

        /// <summary>
        /// 寻径点组。
        /// </summary>
        [Serializable]
        public class GuidePointGroup
        {
#pragma warning disable 0414
            [SerializeField]
            private int m_Key = 0;
#pragma warning restore 0414

            [SerializeField]
            private List<Vector2> m_GuidePoints = new List<Vector2>();

            [SerializeField]
            private int m_ActiveGuidePointIndex = -1;

            [SerializeField]
            private int m_AvailableStartIndex = 0;

            private BaseInstanceLogic m_InstanceLogic;

            public GuidePointGroup(BaseInstanceLogic instanceLogic, int groupKey)
            {
                m_InstanceLogic = instanceLogic;
                m_Key = groupKey;
            }

            /// <summary>
            /// 获取当前激活的寻径点。
            /// </summary>
            public Vector2? ActiveGuidePoint
            {
                get
                {
                    if (m_ActiveGuidePointIndex < 0)
                    {
                        return null;
                    }

                    return m_GuidePoints[m_ActiveGuidePointIndex];
                }
            }

            /// <summary>
            /// 将当前激活的寻径点解除激活并移除。
            /// </summary>
            public void InvalidateActiveGuidePoint()
            {
                if (m_ActiveGuidePointIndex < -1)
                {
                    return;
                }

                m_AvailableStartIndex = m_ActiveGuidePointIndex + 1;
                m_ActiveGuidePointIndex = -1;
            }

            /// <summary>
            /// 激活新的寻径点。
            /// </summary>
            public void ActivateNewGuidePoint()
            {
                if (m_InstanceLogic.m_MeHeroCharacter == null)
                {
                    return;
                }

                if (ActiveGuidePoint != null)
                {
                    return;
                }

                int newIndex = -1;
                float minSqrDist = float.PositiveInfinity;
                Vector2 myPos = m_InstanceLogic.m_MeHeroCharacter.CachedTransform.position.ToVector2();
                for (int i = m_AvailableStartIndex; i < m_GuidePoints.Count; ++i)
                {
                    float sqrDist = Vector2.SqrMagnitude(myPos - m_GuidePoints[i]);
                    if (sqrDist < minSqrDist)
                    {
                        newIndex = i;
                        minSqrDist = sqrDist;
                    }
                }

                m_ActiveGuidePointIndex = newIndex;
            }

            /// <summary>
            /// 添加寻径点。
            /// </summary>
            /// <param name="guidePoint">寻径点坐标。</param>
            public void AddGuidePoint(Vector2 guidePoint)
            {
                m_GuidePoints.Add(guidePoint);
            }

            /// <summary>
            /// 重置。将所有寻径点恢复为初始状态。
            /// </summary>
            public void Reset()
            {
                m_AvailableStartIndex = 0;
                m_ActiveGuidePointIndex = -1;
            }

            /// <summary>
            /// 取消激活当前激活的寻径点。
            /// </summary>
            /// <returns>是否取消成功。</returns>
            public bool DeactivateCurrentGuidePoint()
            {
                if (m_ActiveGuidePointIndex < 0)
                {
                    return false;
                }

                m_ActiveGuidePointIndex = -1;
                return true;
            }

            /// <summary>
            /// 组关键字。
            /// </summary>
            public int Key
            {
                get
                {
                    return m_Key;
                }
            }
        }

        /// <summary>
        /// 寻径点集管理器。
        /// </summary>
        [Serializable]
        public class GuidePointSetManager
        {
            [SerializeField]
            private GuidePointGroup m_ActiveGroup = null;

            private Dictionary<int, GuidePointGroup> m_Groups = new Dictionary<int, GuidePointGroup>();
            private BaseInstanceLogic m_InstanceLogic = null;

            public GuidePointSetManager(BaseInstanceLogic instanceLogic)
            {
                m_InstanceLogic = instanceLogic;
            }

            /// <summary>
            /// 获取当前激活的寻径点。
            /// </summary>
            public Vector2? ActiveGuidePoint
            {
                get
                {
                    if (m_ActiveGroup == null)
                    {
                        return null;
                    }

                    return m_ActiveGroup.ActiveGuidePoint;
                }
            }

            /// <summary>
            /// 将当前激活的寻径点解除激活并移除。
            /// </summary>
            public void InvalidateActiveGuidePoint()
            {
                if (m_ActiveGroup == null)
                {
                    return;
                }

                m_ActiveGroup.InvalidateActiveGuidePoint();
            }

            /// <summary>
            /// 从当前激活的组中激活新的寻径点。
            /// </summary>
            public void ActivateNewGuidePoint()
            {
                if (m_ActiveGroup == null)
                {
                    return;
                }

                m_ActiveGroup.ActivateNewGuidePoint();
            }

            /// <summary>
            /// 取消激活当前激活的寻径点。
            /// </summary>
            /// <returns>是否取消成功。</returns>
            public bool DeactivateCurrentGuidePoint()
            {
                if (m_ActiveGroup == null)
                {
                    return false;
                }

                return m_ActiveGroup.DeactivateCurrentGuidePoint();
            }

            /// <summary>
            /// 添加寻径点。
            /// </summary>
            /// <param name="guidePoints">寻径点列表。</param>
            /// <param name="guidePointGroupKeys">组列表。</param>
            public void AddGuidePoints(IList<Vector2> guidePoints, IList<int> guidePointGroupKeys)
            {
                if (guidePoints == null)
                {
                    Log.Error("guidePoints is invalid.");
                    return;
                }

                if (guidePointGroupKeys == null)
                {
                    Log.Error("guidePointGroups is invalid.");
                    return;
                }

                if (guidePoints.Count != guidePointGroupKeys.Count)
                {
                    Log.Error("guidePoints and guidePointGroups don't have the same length.");
                    return;
                }

                for (int i = 0; i < guidePoints.Count; ++i)
                {
                    Vector2 point = guidePoints[i];
                    int groupKey = guidePointGroupKeys[i];
                    var group = EnsureGroup(groupKey);
                    group.AddGuidePoint(point);
                }
            }

            /// <summary>
            /// 激活一个寻径点组。
            /// </summary>
            /// <param name="groupKey">组关键字。</param>
            public void ActivateGroup(int groupKey)
            {
                var group = EnsureGroup(groupKey);
                group.Reset();
                m_ActiveGroup = group;
            }

            /// <summary>
            /// 获取当前激活的寻径点组关键字。
            /// </summary>
            public int ActiveGroupKey
            {
                get
                {
                    if (m_ActiveGroup == null)
                    {
                        return -1;
                    }

                    return m_ActiveGroup.Key;
                }
            }

            private GuidePointGroup EnsureGroup(int groupKey)
            {
                GuidePointGroup group;
                if (m_Groups.TryGetValue(groupKey, out group))
                {
                    return group;
                }

                group = new GuidePointGroup(m_InstanceLogic, groupKey);
                m_Groups.Add(groupKey, group);
                return group;
            }
        }
    }
}
