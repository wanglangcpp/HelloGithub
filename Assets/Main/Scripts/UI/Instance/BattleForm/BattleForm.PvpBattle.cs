using GameFramework.Network;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class BattleForm
    {
        [SerializeField]
        private PvpInfo m_PvpInfo = null;

        [SerializeField]
        private UIButton m_PvpPauseButton = null;

        [SerializeField]
        private GameObject m_PvpGiveupPanel = null;

        [SerializeField]
        private UIButton m_ChatButton = null;

        [SerializeField]
        private UIToggle m_PvpMusicIsUnmuted = null;

        [SerializeField]
        private UIToggle m_PvpSoundIsUnmuted = null;

        private BasePvpInstanceLogic m_PvpInstanceLogic = null;
        private void InitPvpPanel()
        {
            m_PvpInstanceLogic = GameEntry.SceneLogic.BaseInstanceLogic as BasePvpInstanceLogic;
            m_PvpInfo.Panel.SetActive(m_PvpInstanceLogic != null);
            m_PvpPauseButton.gameObject.SetActive(m_PvpInstanceLogic != null);
            
            if (m_PvpInstanceLogic == null)
                return;
            m_PvpInfo.PlayerInBattle.Root.SetActive(m_PvpInstanceLogic != null);
            m_HeroInBattleRoot.SetActive(m_PvpInstanceLogic == null);
            m_HeroInBattle.Root.SetActive(m_PvpInstanceLogic == null);
            m_HeroInBattle.PortraitObj.SetActive(m_PvpInstanceLogic == null);
            m_HeroInBattle.PortraitInObj.SetActive(m_PvpInstanceLogic == null);
            m_HeroInBattle.PortraitOutObj.SetActive(m_PvpInstanceLogic == null);
            m_HeroInBattle.PortraitMeleeObj.SetActive(m_PvpInstanceLogic == null);
            
            m_Pause.gameObject.SetActive(m_PvpInstanceLogic == null);
            m_ChatButton.gameObject.SetActive(m_PvpInstanceLogic == null);
            m_Auto.gameObject.SetActive(m_PvpInstanceLogic == null );
            m_PvpGiveupPanel.SetActive(false);
            if (m_PvpInstanceLogic == null)
            {
                return;
            }

            INetworkChannel networkChannel = GameEntry.Network.GetNetworkChannel(Constant.Network.RoomNetworkChannelName);
            if (networkChannel == null || !networkChannel.Connected)
            {
                m_PvpInfo.Panel.SetActive(false);
                return;
            }

            m_Ping = new Ping(networkChannel.RemoteIPAddress.ToString(), m_PvpInfo.Status.PingInterval);

            //RoomPlayerData playerData = GameEntry.Data.Room.GetPlayer(m_PvpInstanceLogic.OtherPlayerId);
            PlayerData playerData = GameEntry.Data.PvpArenaOpponent.Player;
            if (playerData == null)
            {
                m_PvpInfo.OppoName.text = string.Empty;
            }
            else if (GameEntry.Data.ServerNames.GetServerData(GameEntry.Data.PvpArenaOpponent.ServerId) != null)
            {
                m_PvpInfo.OppoName.text = GameEntry.Localization.GetString("UI_TEXT_PLAYER_NAME_AND_SERVER", playerData.Name, GameEntry.Data.ServerNames.GetServerData(GameEntry.Data.PvpArenaOpponent.ServerId).Name);
            }
            m_SwitchFirstHero = true;
            RefreshPvpHeroBattle();
            m_SwitchFirstHero = true;
            RefreshPvpOppHeroBattle();
        }

        private void UpdatePvpInfo()
        {
            if (m_PvpInstanceLogic == null)
            {
                return;
            }

            int pingValue = (int)m_Ping.PingValue;
            NetworkReachability networkReachability = Application.internetReachability;
            m_PvpInfo.Status.Wifi.gameObject.SetActive(networkReachability == NetworkReachability.ReachableViaLocalAreaNetwork);
            m_PvpInfo.Status.Cellular.gameObject.SetActive(networkReachability == NetworkReachability.ReachableViaCarrierDataNetwork);
            m_PvpInfo.Status.NotAvailable.gameObject.SetActive(networkReachability == NetworkReachability.NotReachable);
            if (networkReachability == NetworkReachability.NotReachable)
            {
                m_PvpInfo.Status.Delay.text = string.Empty;
            }
            else
            {
                m_PvpInfo.Status.Delay.text = GameEntry.Localization.GetString("UI_TEXT_NETWORK_DELAY", (pingValue < 1 ? "< 1" : pingValue.ToString()));
            }

            Color color = m_PvpInfo.Status.NormalColor;
            if (pingValue <= m_PvpInfo.Status.GoodDelay)
            {
                color = m_PvpInfo.Status.GoodColor;
            }
            else if (pingValue >= m_PvpInfo.Status.BadDelay)
            {
                color = m_PvpInfo.Status.BadColor;
            }

            m_PvpInfo.Status.Wifi.color = color;
            m_PvpInfo.Status.Cellular.color = color;
            m_PvpInfo.Status.NotAvailable.color = color;
            m_PvpInfo.Status.Delay.color = color;

            var oppCurHero = m_PvpInstanceLogic.OppHeroesData.CurrentHeroData;
            var myCurHero = m_PvpInstanceLogic.MyHeroesData.CurrentHeroData;
            m_PvpInfo.PlayerInBattle.OpponentInBattle.HpBar.value = oppCurHero.HPRatio;
            m_PvpInfo.PlayerInBattle.OpponentInBattle.SteadyBar.value = m_PvpInfo.PlayerInBattle.OpponentInBattle.SteadyRecoverBar.value = oppCurHero.Steady.SteadyRatio;
            m_PvpInfo.PlayerInBattle.OpponentInBattle.SteadyBar.gameObject.SetActive(oppCurHero.Steady.IsSteadying);
            m_PvpInfo.PlayerInBattle.OpponentInBattle.SteadyRecoverBar.gameObject.SetActive(!oppCurHero.Steady.IsSteadying);

            m_PvpInfo.PlayerInBattle.SelfInBattle.SteadyBar.value = m_PvpInfo.PlayerInBattle.SelfInBattle.SteadyRecoverBar.value = myCurHero.Steady.SteadyRatio;
            m_PvpInfo.PlayerInBattle.SelfInBattle.SteadyBar.gameObject.SetActive(myCurHero.Steady.IsSteadying);
            m_PvpInfo.PlayerInBattle.SelfInBattle.SteadyRecoverBar.gameObject.SetActive(!myCurHero.Steady.IsSteadying);
            m_PvpInfo.PlayerInBattle.SelfInBattle.HpBar.value = myCurHero.HPRatio;
        }

        private void RefreshPvpHeroBattle()
        {
            if (m_PvpInstanceLogic == null)
            {
                return;
            }
            var myHeroesData = GameEntry.SceneLogic.BaseInstanceLogic.MyHeroesData;
            int currentIndex = myHeroesData.CurrentHeroIndex;
            var meHeroes = myHeroesData.GetHeroes();

            DRHero heroRow = m_DTHero.GetDataRow(meHeroes[currentIndex].HeroId);
            if (heroRow == null)
            {
                return;
            }
            RefreshBattleHeroInfo(m_PvpInfo.PlayerInBattle.SelfInBattle, heroRow);
            RefreshHeroButtonVisible(currentIndex);
        }

        private void RefreshPvpOppHeroBattle()
        {
            var oppHeroData = m_PvpInstanceLogic.OppHeroesData;
            var oppHeroes = oppHeroData.GetHeroes();


            DRHero heroRow = m_DTHero.GetDataRow(oppHeroes[oppHeroData.CurrentHeroIndex].HeroId);
            if (heroRow == null)
            {
                return;
            }
            RefreshBattleHeroInfo(m_PvpInfo.PlayerInBattle.OpponentInBattle, heroRow);

            for (int j = 0; j < m_PvpInfo.PlayerInBattle.OpponentBgHeros.Length; j++)
            {
                m_PvpInfo.PlayerInBattle.OpponentBgHeros[j].Root.SetActive(j < oppHeroes.Length - 1);
            }

            for (int i = 0, j = 0; (i < oppHeroes.Length) && (j < m_PvpInfo.PlayerInBattle.OpponentBgHeros.Length); i++)
            {
                if (i == oppHeroData.CurrentHeroIndex)
                {
                    continue;
                }
                SetPvpBgHeroInfo(m_PvpInfo.PlayerInBattle.OpponentBgHeros[j], oppHeroes[i]);
                j++;
            }
        }

        private void SetPvpBgHeroInfo(PvpBgHeroInfo bgHero, HeroData data)
        {

            DRHero heroRow = m_DTHero.GetDataRow(data.HeroId);
            if (heroRow == null)
            {
                return;
            }

            SetPortraitIcon(heroRow.IconId, bgHero.HeroIcon);
            bgHero.HeroId = data.Id;
            bgHero.HpBar.fillAmount = data.HPRatio;
            if (data.IsDead)
            {
                bgHero.HpBar.gameObject.SetActive(false);
                bgHero.DeathBar.SetActive(true);
                bgHero.HeroIcon.alpha = 0.5f;
            }
            else
            {
                bgHero.HpBar.gameObject.SetActive(true);
                bgHero.DeathBar.SetActive(false);
                bgHero.HeroIcon.alpha = 1f;
            }
        }

        public void OnClickPvpPauseButton()
        {
            m_PvpGiveupPanel.SetActive(true);
            m_PvpMusicIsUnmuted.value = !GameEntry.Sound.MusicIsMuted();
            m_PvpSoundIsUnmuted.value = !GameEntry.Sound.SoundIsMuted();
        }

        public void OnClickContinuePvpButton()
        {
            m_PvpGiveupPanel.SetActive(false);
        }
        /// <summary>
        /// 关闭pvp暂停的界面
        /// </summary>
        public void HidePvpPanel()
        {
            m_PvpGiveupPanel.SetActive(false);
        }
    }
}