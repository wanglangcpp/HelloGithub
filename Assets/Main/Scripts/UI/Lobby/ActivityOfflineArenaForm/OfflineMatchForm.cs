using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class OfflineMatchForm : NGUIForm
    {
        [Serializable]
        private class BattleAvatar
        {
            [SerializeField]
            private UILabel m_AvatarNameLabel = null;

            [SerializeField]
            private UITexture m_AvatarTexture = null;

#pragma warning disable 0414
            [SerializeField]
            private UISprite m_AvatarBorderSprite = null;
#pragma warning restore 0414

            public void SetAvatarData(LobbyHeroData avatar)
            {
                if (avatar != null)
                {
                    m_AvatarNameLabel.text = avatar.Name;
                    m_AvatarTexture.LoadAsync(UIUtility.GetHeroBigPortraitTextureId(avatar.Type));
                }
                else
                {
                    m_AvatarNameLabel.text = string.Empty;
                }
            }
        }

        [SerializeField]
        private Animation m_MaskAnimation = null;

        [SerializeField]
        private Animation m_VsAnimation = null;

        [SerializeField]
        private Animation m_SelfAnimation = null;

        [SerializeField]
        private Animation m_EnemyAnimation = null;

        [SerializeField]
        private PlayerPortrait m_SelfPlayer = null;

        [SerializeField]
        private PlayerPortrait m_EnemyPlayer = null;

        [SerializeField]
        private BattleAvatar[] m_SelfAvatars = null;

        [SerializeField]
        private BattleAvatar[] m_EnemyAvatars = null;

        private OfflineArenaPlayerData m_EnemyData;

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            var enemyData = userData as OfflineMatchFormDisplayData;
            m_EnemyData = enemyData.EnemyPlayerData;

            for (int i = 0; i < m_EnemyAvatars.Length; i++)
            {
                if (m_EnemyData.Heroes.Data.Count > i)
                    m_EnemyAvatars[i].SetAvatarData(m_EnemyData.Heroes.Data[i]);
                else
                    m_EnemyAvatars[i].SetAvatarData(null);
            }

            var heros = GameEntry.Data.HeroTeams.GetData((int)HeroTeamType.Arena).HeroType;
            for (int i = 0; i < heros.Count; i++)
            {
                var hero = GameEntry.Data.LobbyHeros.GetData(heros[i]);
                m_SelfAvatars[i].SetAvatarData(hero);
            }

            m_SelfPlayer.SetPortrait(GameEntry.Data.Player);
            m_EnemyPlayer.SetPortrait(m_EnemyData.Player);

            StartCoroutine(WaitingChangeToBattleScene());
        }

        private IEnumerator WaitingChangeToBattleScene()
        {
            while (m_MaskAnimation.isPlaying
                || m_VsAnimation.isPlaying
                || m_SelfAnimation.isPlaying
                || m_EnemyAnimation.isPlaying)
            {
                yield return null;
            }

            GameEntry.LobbyLogic.EnterArenaBattle(m_EnemyData.Player.Id);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
            StopCoroutine(WaitingChangeToBattleScene());
        }
    }
}
