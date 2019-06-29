using GameFramework.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public abstract class BaseNameBoard
    {
        protected enum TargetType
        {
            Normal = 0,
            Elite = 1,
            Pvp = 2,
            PvpSelf = 3,
            Building = 4,
            Count,
        }

        [SerializeField]
        private Entity m_Owner = null;

        [SerializeField]
        protected NameBoardMode m_Mode = NameBoardMode.NameOnly;

        protected NameBoard m_NameBoard = null;

        private NpcCharacter m_Character = null;

        protected IDictionary<string, string> m_HPColorNames = new Dictionary<string, string>();

        public virtual void Init(NameBoard nameBoard)
        {
            m_NameBoard = nameBoard;
            m_HPColorNames.Add("Red", "porgress_hp_monster");
            m_HPColorNames.Add("Yellow", "porgress_hp_monster_orange");
            m_HPColorNames.Add("Green", "porgress_hp_monster_green");
        }

        public virtual BaseNameBoard RefreshNameBoard(Entity entity, NameBoardMode mode, float hpRatio, float animSeconds)
        {
            if (m_NameBoard == null)
            {
                return null;
            }

            Owner = entity;
            m_Mode = mode;
            m_NameBoard.gameObject.SetActive(true);
            RefreshNameBoard(entity, mode);
            m_NameBoard.SetHPWithAnim(hpRatio, animSeconds);
            m_NameBoard.LobbyNpcObject.SetActive(false);
            return this;
        }

        public virtual void RefreshNameBoard(Entity entity, NameBoardMode mode)
        {
            TargetableObject targetableObject = entity as TargetableObject;
            m_NameBoard.TargetType = (int)NameBoardType;
            m_Mode = mode;
            Owner = entity;
            Height = targetableObject.ModelHeight;
            m_NameBoard.NameLabel.gameObject.SetActive(m_Mode != NameBoardMode.HPBarOnly);
            var hpBars = m_NameBoard.HpBars;
            var elements = m_NameBoard.Elements;
            for (int i = 0; i < (int)TargetType.Count; i++)
            {
                hpBars[i].gameObject.SetActive(m_Mode != NameBoardMode.NameOnly && i == (int)NameBoardType);
                elements[i].gameObject.SetActive(m_Mode != NameBoardMode.NameOnly && i == (int)NameBoardType);
            }
            elements[(int)NameBoardType].InitElementView(targetableObject.ElementId);
            m_NameBoard.LobbyNpcObject.SetActive(false);
            RefreshPosition();
            return;
        }

        public virtual void OnUpdate()
        {
        }

        public void DestroyNameBoard(IObjectPool<NameBoardObject> nameBoardObjects)
        {
            if (!GameEntry.IsAvailable)
            {
                return;
            }

            Owner = null;
            if (m_NameBoard != null && m_NameBoard.gameObject != null)
            {
                m_NameBoard.HideAll();
                m_NameBoard.gameObject.SetActive(false);
                nameBoardObjects.Unspawn(m_NameBoard);
                m_NameBoard = null;
            }
        }

        protected virtual TargetType NameBoardType
        {
            get
            {
                return TargetType.Normal;
            }
        }

        public Entity Owner
        {
            get
            {
                return m_Owner;
            }
            set
            {
                m_Owner = value;
                m_NameBoard.Owner = value;
            }
        }

        public void SetNameLabelShow(bool isShow)
        {
            m_NameBoard.NameLabel.gameObject.SetActive(isShow);
        }

        public NameBoard NameBoard
        {
            get { return m_NameBoard; }
        }

        public NameBoardMode Mode
        {
            get
            {
                return m_Mode;
            }
            set
            {
                m_Mode = value;
            }
        }

        public bool IsElite
        {
            get
            {
                NpcCharacter character = Owner as NpcCharacter;
                return character != null && character.Data.Category == NpcCategory.Elite && (character.Camp == CampType.Enemy || character.Camp == CampType.Enemy2);
            }
        }

        public Color MyNameColor
        {
            get
            {
                return m_NameBoard.MyNameColor;
            }
        }

        public Color OtherNameColor
        {
            get
            {
                return m_NameBoard.OtherNameColor;
            }
        }

        public float StartTime
        {
            get
            {
                return m_NameBoard.StartTime;
            }
            set
            {
                m_NameBoard.StartTime = value;
            }
        }

        public float Height
        {
            get
            {
                return m_NameBoard.Height;
            }
            set
            {
                m_NameBoard.Height = value;
            }
        }

        public NpcCharacter Character
        {
            get
            {
                return m_Character;
            }
            set
            {
                m_Character = value;
            }
        }

        public void SetName(string name)
        {
            m_NameBoard.SetName(name);
        }

        public void SetMeleeName(string name)
        {
            m_NameBoard.SetMeleeName(name);
        }

        public void SetMyMeleeName(string name)
        {
            m_NameBoard.SetMyMeleeName(name);
        }

        public void SetNameColor(Color color)
        {
            m_NameBoard.SetNameColor(color);
        }

        public void SetAlpha(float alpha)
        {
            m_NameBoard.SetAlpha(alpha);
        }

        public void SetHPBarColor(string colorName)
        {
            m_NameBoard.SetHPBarColor(colorName);
        }

        public void SetHPWithAnim(float hpRatio, float animSeconds)
        {
            m_NameBoard.SetHPWithAnim(hpRatio, animSeconds);
        }

        public void RefreshPosition()
        {
            m_NameBoard.RefreshPosition();
        }
    }
}
