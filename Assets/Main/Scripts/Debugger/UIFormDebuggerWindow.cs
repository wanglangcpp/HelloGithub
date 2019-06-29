using GameFramework.Debugger;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public class UIFormDebuggerWindow : IDebuggerWindow
    {
        private const float DefaultButtonHeight = 30f;

        public void Initialize(params object[] args)
        {

        }

        public void Shutdown()
        {

        }

        public void OnEnter()
        {

        }

        public void OnLeave()
        {

        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {

        }

        public void OnDraw()
        {
            GUILayout.BeginVertical();
            {
                if (GUILayout.Button("View rewards", GUILayout.Height(DefaultButtonHeight)))
                {
                    ShowRewards();
                }

                if (GUILayout.Button("Room Break", GUILayout.Height(DefaultButtonHeight)))
                {
                    GameEntry.Network.GetNetworkChannel(Constant.Network.RoomNetworkChannelName).Close();
                }

                if (GUILayout.Button("Enter instance for resource 1", GUILayout.Height(DefaultButtonHeight)))
                {
                    GameEntry.LobbyLogic.EnterInstanceForResource(1);
                }

                if (GameEntry.Uwa.TargetGO != null)
                {
                    if (GUILayout.Button(GameEntry.Uwa.TargetGO.activeSelf ? "Deactivate UWA panel" : "Activate UWA panel", GUILayout.Height(DefaultButtonHeight)))
                    {
                        GameEntry.Uwa.TargetGO.SetActive(!GameEntry.Uwa.TargetGO.activeSelf);
                    }
                }
            }
            GUILayout.EndVertical();
        }

        private static void ShowRewards()
        {
            var userData = new ReceivedGeneralItemsViewData();
            userData.AddItem(202101, 1);
            userData.AddItem(202202, 2);
            userData.AddItem(202303, 3);
            userData.AddItem(202404, 4);
            userData.AddItem(201403, 100);
            userData.AddOtherGoods(111001);
            var hero = new PBLobbyHeroInfo { Type = 1, Level = 1, };
            hero.SkillLevels.AddRange(Constant.DefaultHeroSkillLevels);
            userData.AddFakeGeneralItems(
                new List<PBItemInfo>
                {
                    new PBItemInfo { Type = 209401, Count = 1 },
                    new PBItemInfo { Type = 209402, Count = 1 },
                }
                , null
            );

            GameEntry.RewardViewer.RequestShowRewards(userData, false);
        }
    }
}
