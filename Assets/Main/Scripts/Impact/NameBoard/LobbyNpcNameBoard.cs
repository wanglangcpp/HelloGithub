using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class LobbyNpcNameBoard : BaseNameBoard
    {
        private bool m_LobbyNpcButtonVisible = false;
        private int m_LobbyNpcButtonIcon = 0;

        public Action OnClickNpcButtonReturnAction
        {
            set
            {
                m_NameBoard.OnClickLobbyHeroButtonReturn = value;
            }
        }

        public override void Init(NameBoard nameBoard)
        {
            m_NameBoard = nameBoard;
        }

        public override BaseNameBoard RefreshNameBoard(Entity entity, NameBoardMode mode, float hpRatio, float animSeconds)
        {
            if (m_NameBoard == null)
            {
                return null;
            }
            RefreshNameBoard(entity, mode);
            return this;
        }

        public override void RefreshNameBoard(Entity entity, NameBoardMode mode)
        {
            if (m_LobbyNpcButtonIcon > 0)
            {
                SetLobbyNpcButtonVisible(m_LobbyNpcButtonVisible);
                SetLobbyNpcButtonIcon(m_LobbyNpcButtonIcon);
            }
            Owner = entity;
            m_Mode = mode;
            m_NameBoard.Owner = entity;
            m_NameBoard.gameObject.SetActive(true);
            var lobbyNpc = entity as LobbyNpc;
            if (lobbyNpc == null)
            {
                return;
            }
            Height = lobbyNpc.NpcData.ColliderHeight;
            var hpBars = m_NameBoard.HpBars;
            var elements = m_NameBoard.Elements;
            m_NameBoard.NameLabel.gameObject.SetActive(true);
            for (int i = 0; i < (int)TargetType.Count; i++)
            {
                hpBars[i].gameObject.SetActive(false);
                elements[i].gameObject.SetActive(false);
            }
            return;
        }

        public void SetLobbyNpcButtonVisible(bool visible)
        {
            m_LobbyNpcButtonVisible = visible;
            if (m_NameBoard == null)
            {
                return;
            }
            m_NameBoard.LobbyNpcObject.SetActive(visible);
        }

        public void SetLobbyNpcButtonIcon(int iconId)
        {
            m_LobbyNpcButtonIcon = iconId;
            if (m_NameBoard == null)
            {
                return;
            }
            m_NameBoard.SetLobbyNpcIcon(iconId);
        }

    }
}
