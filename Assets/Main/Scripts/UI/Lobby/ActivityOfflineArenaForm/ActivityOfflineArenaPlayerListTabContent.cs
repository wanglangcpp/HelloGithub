using GameFramework.Event;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    /// <summary>
    /// 离线竞技主界面 -- 对手列表标签页。
    /// </summary>
    public class ActivityOfflineArenaPlayerListTabContent : MonoBehaviour
    {
        //[SerializeField]
        //private ChallangeablePlayer[] m_ChallengeablePlayers = null;

        //private ActivityOfflineArenaForm m_Form = null;
        //private bool m_ShouldRefreshDataOnEnable = false;

        //// Called by NGUI via reflection.
        //public void OnClickChallengeButton(int index)
        //{
        //   // GameEntry.LobbyLogic.OfflineArenaGetOpponentDetails(m_Form.SrcData.OtherPlayers.Data[index].Player.Id);
        //}

        //internal void ClearChallengeablePlayers()
        //{
        //    for (int i = 0; i < m_ChallengeablePlayers.Length; ++i)
        //    {
        //        m_ChallengeablePlayers[i].PlayerId = 0;
        //        m_ChallengeablePlayers[i].Self.gameObject.SetActive(false);
        //    }
        //}

        //internal void UpdateRefreshDataOnEnable()
        //{
        //    m_ShouldRefreshDataOnEnable = true;
        //}

        //internal void RefreshChallangeablePlayers()
        //{
        //    var data = m_Form.SrcData.OtherPlayers.Data;

        //    for (int i = 0; i < m_ChallengeablePlayers.Length; ++i)
        //    {
        //        var playerDisplay = m_ChallengeablePlayers[i];
        //        if (i < data.Count)
        //        {
        //            playerDisplay.PlayerId = data[i].Key;
        //            playerDisplay.Name.text = data[i].Player.Name;
        //            playerDisplay.Might.text = data[i].Player.TeamMight.ToString();
        //            playerDisplay.Level.text = data[i].Player.Level.ToString();
        //            playerDisplay.Rank.SetRank(data[i].Rank);
        //            playerDisplay.Portrait.LoadAsync(UIUtility.GetPlayerPortraitIconId(data[i].Player.PortraitType));
        //        }
        //        else
        //        {
        //            playerDisplay.PlayerId = 0;
        //        }

        //        playerDisplay.Self.gameObject.SetActive(playerDisplay.PlayerId != 0);
        //    }
        //}

        //#region MonoBehaviour

        //private void Awake()
        //{
        //    UIUtility.ReplaceDictionaryTextForLabels(gameObject);
        //    m_Form = GetComponentInParent<ActivityOfflineArenaForm>();
        //    InitRankDisplays();
        //}

        //private void InitRankDisplays()
        //{
        //    for (int i = 0; i < m_ChallengeablePlayers.Length; ++i)
        //    {
        //        var playerDisplay = m_ChallengeablePlayers[i];
        //        playerDisplay.Rank.Init();
        //    }
        //}

        //private void OnEnable()
        //{
        //    SubscribeEvents();

        //    if (m_ShouldRefreshDataOnEnable)
        //    {
        //        m_ShouldRefreshDataOnEnable = false;
        //        ClearChallengeablePlayers();
        //        RequestNormalData();
        //    }
        //}

        //private void OnDisable()
        //{
        //    UnsubscribeEvents();
        //}

        //private void OnDestroy()
        //{
        //    m_Form = null;
        //}

        //#endregion MonoBehaviour

        //private void RequestNormalData()
        //{
        //    //GameEntry.LobbyLogic.OfflineArenaGetData();
        //}

        //private void SubscribeEvents()
        //{
        //    GameEntry.Event.Subscribe(EventId.OfflineArenaOpponentDataChanged, OnOpponentDataChanged);
        //    GameEntry.Event.Subscribe(EventId.OfflineArenaPlayerListChanged, OnPlayerListRefreshed);
        //}

        //private void UnsubscribeEvents()
        //{
        //    GameEntry.Event.Unsubscribe(EventId.OfflineArenaOpponentDataChanged, OnOpponentDataChanged);
        //    GameEntry.Event.Unsubscribe(EventId.OfflineArenaPlayerListChanged, OnPlayerListRefreshed);
        //}

        //private void OnOpponentDataChanged(object sender, GameEventArgs e)
        //{
        //    GameEntry.UI.OpenUIForm(UIFormId.PvpOfflineArenaForm, new OfflineArenaPrepareData { IsRevenge = false });
        //}

        //private void OnPlayerListRefreshed(object sender, GameEventArgs e)
        //{
        //    m_ShouldRefreshDataOnEnable = false;
        //    RefreshChallangeablePlayers();
        //    m_Form.StartRefreshCD();
        //}

        //[Serializable]
        //private class ChallangeablePlayer
        //{
        //    public int PlayerId = -1;
        //    public Transform Self = null;
        //    public RankDisplay Rank = null;
        //    public UISprite Portrait = null;
        //    public UILabel Level = null;
        //    public UILabel Name = null;
        //    public UILabel Might = null;
        //    public UIButton ChallengeButton = null;
        //}
    }
}
