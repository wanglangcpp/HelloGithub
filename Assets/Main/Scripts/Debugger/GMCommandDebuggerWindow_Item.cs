using GameFramework.Debugger;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Genesis.GameClient
{
    public class GMCommandDebuggerWindow_Item : IDebuggerWindow
    {
        private Vector2 m_ScrollPosition = Vector2.zero;

        private List<KeyValuePair<int, string>> m_NormalItemIdsToNameKeys = null;
        private List<KeyValuePair<int, string>> m_HeroQualityItemIdsToNameKeys = null;
        private List<KeyValuePair<int, string>> m_SpecificSkillBadgeIdsToNameKeys = null;
        private List<KeyValuePair<int, string>> m_GenericSkillBadgeIdsToNameKeys = null;
        private int m_ItemCountToAdd = 1;
        private string m_ItemCountToAddText = string.Empty;

        public void Initialize(params object[] args)
        {
            m_ItemCountToAddText = m_ItemCountToAdd.ToString();
        }

        public void Shutdown()
        {

        }

        public void OnEnter()
        {
            if (m_NormalItemIdsToNameKeys == null)
            {
                m_NormalItemIdsToNameKeys = GameEntry.DataTable.GetDataTable<DRItem>()
                    .GetAllDataRows(x => !x.AutoUse).ToList().ConvertAll(o => new KeyValuePair<int, string>(o.Id, o.Name));
            }

            if (m_HeroQualityItemIdsToNameKeys == null)
            {
                m_HeroQualityItemIdsToNameKeys = GameEntry.DataTable.GetDataTable<DRHeroQualityItem>()
                    .GetAllDataRows().ToList().ConvertAll(o => new KeyValuePair<int, string>(o.Id, o.Name));
            }

            if (m_SpecificSkillBadgeIdsToNameKeys == null)
            {
                m_SpecificSkillBadgeIdsToNameKeys = GameEntry.DataTable.GetDataTable<DRSpecificSkillBadge>()
                    .GetAllDataRows().ToList().ConvertAll(o => new KeyValuePair<int, string>(o.Id, o.Name));
            }

            if (m_GenericSkillBadgeIdsToNameKeys == null)
            {
                m_GenericSkillBadgeIdsToNameKeys = GameEntry.DataTable.GetDataTable<DRGenericSkillBadge>()
                    .GetAllDataRows().ToList().ConvertAll(o => new KeyValuePair<int, string>(o.Id, o.Name));
            }
        }

        public void OnLeave()
        {

        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {

        }

        public void OnDraw()
        {
            if (!(GameEntry.Procedure.CurrentProcedure is ProcedureMain && !GameEntry.OfflineMode.OfflineModeEnabled))
            {
                DrawTips();
                return;
            }

            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            {
                DrawSectionItemCount();
                DrawSectionAddItemCommand();
                DrawSectionAddHeroQualityItemCommand();
                DrawSectionAddSpecificSkillBadgeItemCommand();
                DrawSectionAddGenericSkillBadgeItemCommand();
            }
            GUILayout.EndScrollView();
        }

        private void DrawTips()
        {
            GUILayout.BeginHorizontal("box");
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("<b><color=yellow>You cannot use this page currently.</color></b>");
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();
        }

        private void DrawSectionItemCount()
        {
            GUILayout.BeginHorizontal("box");
            {
                GUILayout.Label("Item count to add: ");
                string newText = GUILayout.TextField(m_ItemCountToAddText);
                if (newText != m_ItemCountToAddText)
                {
                    int newCount;
                    if (int.TryParse(newText, out newCount))
                    {
                        m_ItemCountToAdd = Mathf.Clamp(newCount, 1, 9999);
                        m_ItemCountToAddText = m_ItemCountToAdd.ToString();
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        public void DrawSectionAddItemCommand()
        {
            GUILayout.Label("<b>Add (Normal) Item</b>");
            DrawItems(m_NormalItemIdsToNameKeys);
        }

        private void DrawSectionAddHeroQualityItemCommand()
        {
            GUILayout.Label("<b>Add Hero Quality Item</b>");
            DrawItems(m_HeroQualityItemIdsToNameKeys);
        }

        private void DrawSectionAddSpecificSkillBadgeItemCommand()
        {
            GUILayout.Label("<b>Add Specific Skill Badge</b>");
            DrawItems(m_SpecificSkillBadgeIdsToNameKeys);
        }

        private void DrawSectionAddGenericSkillBadgeItemCommand()
        {
            GUILayout.Label("<b>Add Generic Skill Badge</b>");
            DrawItems(m_GenericSkillBadgeIdsToNameKeys);
        }

        private void DrawItems(IList<KeyValuePair<int, string>> items)
        {
            int itemCount = items.Count;
            int columnCount = 3;
            int rowCount = (itemCount + columnCount - 1) / columnCount;
            GUILayout.BeginVertical("box");
            {
                for (int i = 0; i < rowCount; i++)
                {
                    GUILayout.BeginHorizontal();
                    {
                        for (int j = 0; j < columnCount; j++)
                        {
                            if (columnCount * i + j > itemCount - 1)
                            {
                                break;
                            }
                            int itemId = items[columnCount * i + j].Key;
                            if (GUILayout.Button(string.Format("{0}. {1}", itemId.ToString(), GameEntry.Localization.GetString(items[columnCount * i + j].Value)), GUILayout.Height(30)))
                            {
                                CLGMCommand request = new CLGMCommand();
                                request.Type = (int)GMCommandType.AddItem;
                                request.Params.Add(itemId.ToString());
                                request.Params.Add(m_ItemCountToAdd.ToString());
                                GameEntry.Network.Send(request);
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndVertical();
        }
    }
}
