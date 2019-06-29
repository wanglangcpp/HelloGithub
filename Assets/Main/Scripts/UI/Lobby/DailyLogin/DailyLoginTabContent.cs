using System;
using GameFramework.Event;
using UnityEngine;

namespace Genesis.GameClient
{
    public class DailyLoginTabContent : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_SignedCountLabel = null;

        [SerializeField]
        private UIButton m_SignButton = null;

        [SerializeField]
        private DailyLoginScrollView m_ItemScrollView = null;

        private DailyLoginItem m_TodayItem = null;

        private void Start()
        {
            UIUtility.ReplaceDictionaryTextForLabels(m_SignButton.gameObject);
        }

        private void OnEnable()
        {
            GameEntry.Event.Subscribe(EventId.DailyLogined, OnLogined);
            GameEntry.Event.Subscribe(EventId.DailyLoginAcrossDay, OnAcrossDay);

            m_ItemScrollView.InitDragEvent();

            SetContents();
            SetHintsAndButton();

            GetComponent<UIPanel>().depth += 1;

            m_SignButton.onClick.Clear();
            m_SignButton.onClick.Add(new EventDelegate(() => { OnLoginClicked(); }));
        }

        private void OnAcrossDay(object sender, GameEventArgs e)
        {
            SetContents();
            SetHintsAndButton();
        }

        private void SetContents()
        {
            var itemCount = GameEntry.Data.DailyLogin.DailyLoginConfig.Count;

            for (int i = 0; i < itemCount; ++i)
            {
                var item = m_ItemScrollView.GetOrCreateItem(i);
                item.SetItemData(GameEntry.Data.DailyLogin.GetDailyLoginRow(i));

                if (item.IsToday)
                    m_TodayItem = item;
            }
        }

        private void OnLoginClicked()
        {
            GameEntry.LobbyLogic.DoDailyLogin();
        }

        private void OnDisable()
        {
            GameEntry.Event.Unsubscribe(EventId.DailyLogined, OnLogined);
            GameEntry.Event.Unsubscribe(EventId.DailyLoginAcrossDay, OnAcrossDay);
            m_ItemScrollView.DiposeDragEvent();

            // Lua实例化出来的物体每次打开都会实例化一个新的
        }

        private void OnLogined(object sender, GameEventArgs e)
        {
            var showData = e as DailyLoginedEventArgs;
            if (showData == null)
                return;

            SetHintsAndButton();

            if (m_TodayItem != null)
                m_TodayItem.RefreshIconState();

            GameEntry.RewardViewer.RequestShowRewards(showData.ShowRewards.ReceiveGoodsData, true);
        }

        private void SetHintsAndButton()
        {
            m_SignedCountLabel.text = GameEntry.Localization.GetString("UI_SIGN_THIS_MONTH_SIGNED", GameEntry.Data.DailyLogin.LoginedDayCount);
            m_SignButton.isEnabled = !GameEntry.Data.DailyLogin.IsLoginedToday;
        }

        [Serializable]
        private class DailyLoginScrollView : UIScrollViewCache<DailyLoginItem>
        {
            public void InitDragEvent()
            {
                m_ScrollView.onDragStarted = () =>
                {
                    for (int i = 0; i < m_ItemParent.transform.childCount; i++)
                    {
                        var child = m_ItemParent.transform.GetChild(i).GetComponent<DailyLoginItem>();
                        child.ShowEffect(false);
                    }
                };

                m_ScrollView.onDragFinished = () =>
                {
                    if (m_ItemParent.transform.childCount <= 0)
                        return;

                    float yOffset = m_ScrollView.panel.clipOffset.y;

                    float yButtom = m_ScrollView.panel.baseClipRegion.w;

                    float perHeight = NGUIMath.CalculateRelativeWidgetBounds(m_ItemParent.transform.GetChild(0).transform).size.y;
                    int scrollLine = yOffset > 0 ? (int)Math.Floor((yOffset) / perHeight) : (int)Math.Ceiling(Math.Abs(yOffset) / perHeight);
                    int perLineCount = m_ItemParent.GetComponent<UIGrid>().maxPerLine;
                    int showingLine = (int)Math.Floor(yButtom / perHeight);

                    if ((Math.Abs(yOffset) % perHeight) < (perHeight / 2))
                    {
                        showingLine--;
                    }

                    for (int i = 0; i < m_ItemParent.transform.childCount; i++)
                    {
                        var child = m_ItemParent.transform.GetChild(i).GetComponent<DailyLoginItem>();
                        if (i < scrollLine * perLineCount)
                        {
                            child.ShowEffect(false);
                        }
                        else
                        {
                            if (i >= (scrollLine + showingLine) * perLineCount)
                            {
                                child.ShowEffect(false);
                            }
                            else
                            {
                                child.RefreshIconState();
                            }
                        }
                    }
                };

            }

            public void DiposeDragEvent()
            {
                m_ScrollView.onDragStarted = null;
                m_ScrollView.onDragFinished = null;
            }
        }
    }
}