using UnityEngine;

namespace Genesis.GameClient
{
    public class PlayerPortrait : MonoBehaviour
    {
        public UISprite PortraitSprite = null;
        public UISprite PortraitBorderSprite = null;
        /// <summary>
        /// 是否区分在线和离线的状态
        /// </summary>
        public bool DistinguishOnlineStatus = false;

        public UILabel PlayerNameLabel = null;

        /// <summary>
        /// 名字后面是否显示玩家等级
        /// </summary>
        public bool ShowLevelInName = true;

        public UILabel VipLabel = null;

        public UILabel LevelLabel = null;

        /// <summary>
        /// 是否显示战斗力
        /// </summary>
        public bool ShowMight = false;
        
        public UILabel MightLabel = null;

        /// <summary>
        /// 使用Prefab上设置的数据设置头像
        /// </summary>
        /// <param name="playerData">玩家数据</param>
        public void SetPortrait(PlayerData playerData)
        {
            PortraitSprite.LoadAsync(UIUtility.GetPlayerPortraitIconId(playerData.PortraitType));

            if (DistinguishOnlineStatus)
                PortraitSprite.color = playerData.IsOnline ? Color.white : Color.gray;

            if (ShowLevelInName)
                PlayerNameLabel.text = GameEntry.Localization.GetString("UI_TEXT_NOTICE_CHATTOWORLD", playerData.Name, playerData.Level);
            else
                PlayerNameLabel.text = playerData.Name;

            if (LevelLabel != null)
                LevelLabel.text = playerData.Level.ToString();

            if (ShowMight && MightLabel != null)
                MightLabel.text = playerData.TeamMight.ToString();

            VipLabel.text = playerData.VipLevel.ToString();
        }

        /// <summary>
        /// 根据自己的需求设置头像
        /// </summary>
        /// <param name="playerData">玩家数据</param>
        /// <param name="distinguishOnlineStatus">是否区分在线离线状态</param>
        /// <param name="showLevelInName">名字后面加不加等级标签</param>
        /// <param name="showMight">是否显示战斗力</param>
        public void SetPortrait(PlayerData playerData, bool distinguishOnlineStatus, bool showLevelInName, bool showMight)
        {
            DistinguishOnlineStatus = distinguishOnlineStatus;
            ShowLevelInName = showLevelInName;
            ShowMight = showMight;

            SetPortrait(playerData);
        }
    }
}