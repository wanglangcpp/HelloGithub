using UnityEngine;
using System.Collections.Generic;
using GameFramework.DataTable;

namespace Genesis.GameClient
{
    public class ActivitySinglePvpResultForm : NGUIForm
    {
        public InstanceResultType resultType = InstanceResultType.Draw;
        [SerializeField]
        private Animation m_WinIconAnimation = null;

        [SerializeField]
        private Animation m_RewardBgAnimation = null;

        [SerializeField]
        private GoodsView m_Item = null;

        [SerializeField]
        private GameObject m_ConfirmBtn = null;

        private bool m_IsPlayAnim = true;
        [SerializeField]
        private UISprite spTitle;
        [SerializeField]
        private UILabel lblCoin;
        [SerializeField]
        private UILabel lblGold;
        [SerializeField]
        private Transform myInfo;
        [SerializeField]
        private Transform oppInfo;
        [SerializeField]
        private PlayerResult myResult;
        [SerializeField]
        private PlayerResult oppResult;

        private IDataTable<DRHero> m_DTHero;
        private IDataTable<DRIcon> m_DTIcon = null;
        private SinglePvpResultDisplayData displayData = null;
        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);
            SinglePvpResultDisplayData displayData = userData as SinglePvpResultDisplayData;
            if (displayData == null)
            {
                displayData = GameEntry.Data.SingleMatchData.DisplayData;
            }
            if (displayData == null)
            {
                //Debug.LogError("无结算结果！");
                return;
            }
            this.displayData = displayData;
            OnInitData();
            ShowDisplayData();
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (m_IsPlayAnim && !m_WinIconAnimation.isPlaying && !m_RewardBgAnimation.isPlaying)
            {
                m_IsPlayAnim = false;
                m_Item.gameObject.SetActive(true);
                m_ConfirmBtn.SetActive(true);
            }
        }

        private void OnInitData()
        {
            m_IsPlayAnim = true;
            m_WinIconAnimation.Rewind();
            m_RewardBgAnimation.Rewind();
            m_WinIconAnimation.Play();
            m_RewardBgAnimation.Play();
            //m_Item.InitGoodsView((int)CurrencyType.Coin, 2);
            //m_ConfirmBtn.SetActive(false);
            m_Item.gameObject.SetActive(false);
            m_DTHero = GameEntry.DataTable.GetDataTable<DRHero>();
            m_DTIcon = GameEntry.DataTable.GetDataTable<DRIcon>();
        }

        public void OnClickContinueButton()
        {
            GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenActivitySinglePvpMainForm, true);
            GameEntry.SceneLogic.GoBackToLobby(false);
            //GameEntry.Network.GetNetworkChannel(Constant.Network.RoomNetworkChannelName).Close();
            //GameEntry.Data.Room.Id = -1;
            //GameEntry.Data.Room.HasReconnected = false;
        }
        public void OnClickConfirmButton()
        {
            //GameEntry.Data.AddOrUpdateTempData(Constant.TempData.AutoOpenActivitySinglePvpMainForm, true);
            GameEntry.SceneLogic.GoBackToLobby(true);
            //GameEntry.Network.GetNetworkChannel(Constant.Network.RoomNetworkChannelName).Close();
            //GameEntry.Data.Room.Id = -1;
            //GameEntry.Data.Room.HasReconnected = false;
        }
        void ShowDisplayData()
        {
            //displayData
            //my
            switch (displayData.ResultType)
            {
                case InstanceResultType.Undefined:
                    return;
                case InstanceResultType.Win:
                    spTitle.spriteName = "image_win";
                    break;
                case InstanceResultType.Lose:
                    spTitle.spriteName = "image_defeat";
                    break;
                case InstanceResultType.Draw:
                    spTitle.spriteName = "image_draw";
                    break;
                default:
                    break;
            }
            List<PlayerResult> playerList = new List<PlayerResult>();
            PlayerResult myResult = new PlayerResult();
            PlayerResult oppResult = new PlayerResult();
            for (int i = 0; i < displayData.Settlements.Count; i++)
            {
                PBPVPSettlement pBPVPSettlement = displayData.Settlements[i];
                if (pBPVPSettlement.UserId == GameEntry.Data.Player.Id)
                {
                    myResult.Coin = pBPVPSettlement.Coin;
                    myResult.Level = GameEntry.Data.Player.Level;
                    myResult.Name = GameEntry.Data.Player.Name;
                    myResult.OldRank = pBPVPSettlement.OldRank;
                    myResult.Rank = pBPVPSettlement.Rank;
                    myResult.OldScore = pBPVPSettlement.OldScore;
                    myResult.Score = pBPVPSettlement.Score;
                    int totalDamage = 0;
                    for (int k = 0; k < pBPVPSettlement.Damages.Count; k++)
                    {
                        totalDamage += pBPVPSettlement.Damages[k];
                    }
                    for (int j = 0; j < GameEntry.Data.SingleMatchData.HeroDatas.Count; j++)
                    {
                        LobbyHeroData data = GameEntry.Data.SingleMatchData.HeroDatas[j];
                        CharacterResult character = new CharacterResult();
                        character.CharacterId = data.Type;
                        character.Damage = pBPVPSettlement.Damages[j];
                        character.KillCount = pBPVPSettlement.KillCounts[j];
                        character.DamagePercent = totalDamage == 0 ? 0 : (int)(character.Damage * 100 / totalDamage);
                        character.Dead = pBPVPSettlement.DeadState[j];
                        myResult.CharacterDatas.Add(character);
                    }
                    myResult.PortraitType = GameEntry.Data.Player.PortraitType;
                    //竞技币显示积分变动，金币显示竞技币获得数量，要问为什么，找策划
                    lblCoin.text = (pBPVPSettlement.Score - pBPVPSettlement.OldScore).ToString();
                    lblGold.text = pBPVPSettlement.Coin.ToString();
                    //GameEntry.UIFragment.LoadAndInstantiate
                }
                else
                {
                    oppResult.Coin = pBPVPSettlement.Coin;
                    oppResult.Level = GameEntry.Data.PvpArenaOpponent.Player.Level;
                    oppResult.Name = GameEntry.Data.PvpArenaOpponent.Player.Name;
                    oppResult.OldRank = pBPVPSettlement.OldRank;
                    oppResult.Rank = pBPVPSettlement.Rank;
                    oppResult.OldScore = pBPVPSettlement.OldScore;
                    oppResult.Score = pBPVPSettlement.Score;
                    int totalDamage = 0;
                    for (int k = 0; k < pBPVPSettlement.Damages.Count; k++)
                    {
                        totalDamage += pBPVPSettlement.Damages[k];
                    }
                    for (int j = 0; j < GameEntry.Data.PvpArenaOpponent.HeroDatas.Count; j++)
                    {
                        LobbyHeroData data = GameEntry.Data.PvpArenaOpponent.HeroDatas[j];
                        CharacterResult character = new CharacterResult();
                        character.CharacterId = data.Type;
                        character.Damage = pBPVPSettlement.Damages[j];
                        character.KillCount = pBPVPSettlement.KillCounts[j];
                        character.DamagePercent = totalDamage == 0 ? 0 : (int)(character.Damage * 100 / totalDamage);
                        character.Dead = pBPVPSettlement.DeadState[j];
                        oppResult.CharacterDatas.Add(character);
                    }
                    oppResult.PortraitType = GameEntry.Data.PvpArenaOpponent.Player.PortraitType;
                }
            }

            SetPlayerResult(myResult, myInfo);
            SetPlayerResult(oppResult, oppInfo);
            //myResultzO
        }

        void SetPlayerResult(PlayerResult playerResult, Transform playerInfoTrans)
        {
            string str = GameEntry.Localization.GetRawString("UI_TEXT_PVP_RESULT_PLAYER_NAME");
            //str = GameEntry.StringReplacement.GetString("UI_TEXT_PVP_RESULT_PLAYER_NAME");
            playerInfoTrans.Find("name").GetComponent<UILabel>().text = string.Format(str, playerResult.Name, playerResult.Level);
            playerInfoTrans.Find("score").GetComponent<UILabel>().text = playerResult.Score.ToString();
            playerInfoTrans.Find("PlayerBg/PlayerPortrait").GetComponent<UISprite>().LoadAsync(UIUtility.GetPlayerPortraitIconId(playerResult.PortraitType));
            //.Portrait.LoadAsync(UIUtility.GetPlayerPortraitIconId(player.PortraitType))
            List<CharacterResult> characters = new List<CharacterResult>();
            for (int i = 0; i < 3; i++)
            {
                if (i >= playerResult.CharacterDatas.Count)
                {
                    playerInfoTrans.Find("Grid/Character" + i).gameObject.SetActive(false);
                }
                else
                    SetCharacterResult(playerResult.CharacterDatas[i], playerInfoTrans.Find("Grid/Character" + i));
            }
            playerInfoTrans.Find("Grid").GetComponent<UIGrid>().repositionNow = true;

        }

        void SetCharacterResult(CharacterResult characterResult, Transform characterInfoTrans)
        {
            DRHero dRHero = m_DTHero.GetDataRow(characterResult.CharacterId);
            SetPortraitIcon(dRHero.IconId, characterInfoTrans.Find("icon").GetComponent<UISprite>());
            characterInfoTrans.Find("icon/element").GetComponent<UISprite>().spriteName = UIUtility.GetElementSpriteName(dRHero.ElementId);
            characterInfoTrans.Find("kill").GetComponent<UILabel>().text = GameEntry.Localization.GetString("UI_TEXT_PVP_KILL", characterResult.KillCount.ToString());
            characterInfoTrans.Find("damage").GetComponent<UILabel>().text = GameEntry.Localization.GetString("UI_TEXT_PVP_DPS", characterResult.DamagePercent);
            characterInfoTrans.Find("dead").gameObject.SetActive(characterResult.Dead);
        }
        private void SetPortraitIcon(int iconId, UISprite targetSprite)
        {
            DRIcon row = m_DTIcon.GetDataRow(iconId);
            if (row == null)
            {
                return;
            }

            targetSprite.spriteName = row.SpriteName;
        }
        [SerializeField]
        public class PlayerResult
        {
            [SerializeField]
            public string Name;
            [SerializeField]
            public int Level;
            [SerializeField]
            public int OldRank;
            [SerializeField]
            public int Rank;
            [SerializeField]
            public int OldScore;
            [SerializeField]
            public int Score;
            [SerializeField]
            public int Coin;
            [SerializeField]
            public List<CharacterResult> CharacterDatas = new List<CharacterResult>();
            [SerializeField]
            public int PortraitType;
        }
        [SerializeField]
        public class CharacterResult
        {
            [SerializeField]
            public int CharacterId;
            [SerializeField]
            public int Damage;
            [SerializeField]
            public int DamagePercent;
            [SerializeField]
            public int KillCount;
            [SerializeField]
            public bool Dead;
        }

    }
}
