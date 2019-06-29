using System;
using GameFramework.Debugger;
using UnityEngine;

namespace Genesis.GameClient
{
    public class GMCommandDebuggerWindow_Other : IDebuggerWindow
    {
        private const int MaxStar = 3;
        private Vector2 m_ScrollPosition = Vector2.zero;

        public void Initialize(params object[] args)
        {

        }

        public void Shutdown()
        {

        }

        public void OnEnter()
        {
            autoFight = GameEntry.Setting.GetBool("AUTO_FIGHT", false);
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
                DrawCreateTestAccount();
                DrawSetAllInstanceStars();
                DrawSendSystemWorldChat();
                DrawSendMail();
                DrawAddTask();
                DrawOpenFunction();
                DrawNoviceGuide();
                DrawAutoFight();
            }
            GUILayout.EndScrollView();
        }

        private void DrawCreateTestAccount()
        {
            GUILayout.BeginVertical("box");
            {
                GUILayout.Label("<b>CreateTestAccount->重启->addAllHeroLevel30->等待几秒</b>");
                GUILayout.Label("<b>添加英雄后无视错误指令提示并重启，才能看到新加入英雄</b>");
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("CreateTestAccount"))
                    {
                        SendTestRequest();

                    }
                    if (GUILayout.Button("addAllHeroLevel30"))
                    {
                        var config = GameEntry.ClientConfig.ClinetBuildAccountConfig;
                        SendTestRequest_HeroLevel(config.MaxUseHeroLevel);
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void SendTestRequest()
        {
            var config = GameEntry.ClientConfig.ClinetBuildAccountConfig;
            SendTestRequest_MyLevel(config.MyMaxLevel);
            SendTestRequset_MyMoney(config.AddMoney);
            SendTestRequset_MyCoin(config.AddCoin);
            SendTestRequest_Star(config.MaxStar);
            SendTestRequest_AddHero(config.AddHeroIDList);
            SendTestRequest_AddItem(config.AddItemIDList);
        }

        private void SendTestRequset_MyCoin(int number)
        {
            CLGMCommand request = new CLGMCommand();
            request.Type = (int)GMCommandType.AddCoin;
            request.Params.Add(number.ToString());
            GameEntry.Network.Send(request);
        }

        private void SendTestRequset_MyMoney(int number)
        {
            CLGMCommand request = new CLGMCommand();
            request.Type = (int)GMCommandType.AddMoney;
            request.Params.Add(number.ToString());
            GameEntry.Network.Send(request);
        }

        private void SendTestRequest_MyLevel(int maxLevel)
        {
            int myLevel = GameEntry.Data.Player.Level;
            int addLevel = maxLevel - myLevel;
            CLGMCommand request = new CLGMCommand();
            request.Type = (int)GMCommandType.PlayerLevelUp;
            request.Params.Add(addLevel.ToString());
            GameEntry.Network.Send(request);
        }

        private void SendTestRequest_Star(int startCount)
        {
            CLGMCommand request = new CLGMCommand();
            request.Type = (int)GMCommandType.SetAllInstanceStars;
            request.Params.Add(startCount.ToString());
            GameEntry.Network.Send(request);
        }

        private void SendTestRequest_AddHero(int[] heroList)
        {
            for (int i = 0; i < heroList.Length; i++)
            {
                CLGMCommand request = new CLGMCommand();
                request.Type = (int)GMCommandType.AddHero;
                request.Params.Add(heroList[i].ToString());
                GameEntry.Network.Send(request);
            }
        }

        private void SendTestRequest_AddItem(int[] itemList)
        {
            int count = 1;
            for (int i = 0; i < itemList.Length; i++)
            {
                CLGMCommand request = new CLGMCommand();
                request.Type = (int)GMCommandType.AddItem;
                request.Params.Add(itemList[i].ToString());
                request.Params.Add(count.ToString());
                GameEntry.Network.Send(request);
            }
        }

        private void SendTestRequest_HeroLevel(int maxLevel)
        {
            int level = 1;
            if (GameEntry.Data != null &&
                GameEntry.Data.LobbyHeros != null &&
                GameEntry.Data.LobbyHeros.Data != null &&
                GameEntry.Data.LobbyHeros.Data.Count > 0)
            {
                var heroData = GameEntry.Data.LobbyHeros.Data;
                for (int i = 0; i < heroData.Count; i++)
                {
                    level = maxLevel - heroData[i].Level;
                    CLGMCommand request = new CLGMCommand();
                    request.Type = (int)GMCommandType.HeroLevelUp;
                    request.Params.Add(heroData[i].Key.ToString());
                    request.Params.Add(level.ToString());
                    GameEntry.Network.Send(request);
                }

            }
        }

        private void DrawSetAllInstanceStars()
        {
            GUILayout.BeginVertical("box");
            {
                GUILayout.Label("<b>Refresh all instance stars</b>");
                GUILayout.BeginHorizontal();
                {
                    for (int starCount = 0; starCount <= MaxStar; starCount++)
                    {
                        if (GUILayout.Button(starCount.ToString()))
                        {
                            var request = new CLGMCommand { Type = (int)GMCommandType.SetAllInstanceStars };
                            request.Params.Add(starCount.ToString());
                            GameEntry.Network.Send(request);
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
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

        private void DrawSendSystemWorldChat()
        {
            if (GUILayout.Button("Send Test System World Chat", GUILayout.Height(30)))
            {
                CLGMCommand request = new CLGMCommand();
                request.Type = (int)GMCommandType.SendSystemWorldChat;
                request.Params.Add("测试系统聊天");
                GameEntry.Network.Send(request);
            }
        }

        private string m_Receiver = "Receiver id. Default value is send for yourself.";
        private string m_Sender = "Sender name.";
        private string m_Title = "Title";
        private string m_Priotity = "Mail Priotity. The bigger, the top. Default value is 0.";
        private string m_Message = "Here is the mail content.";

        private System.Collections.Generic.Dictionary<int, int> m_Attachments = new System.Collections.Generic.Dictionary<int, int>();
        private string m_AttachmentId = "0";
        private string m_AttachmentCount = "0";
        private string m_TaskId = "0";
        private string m_CurrentAttachment = string.Empty;
        private string m_Recorder = string.Empty;
        private SenderRecorder m_SenderRecorder = new SenderRecorder();
        private class SenderRecorder
        {
            private const int Captain = 10;
            private System.Collections.Generic.LinkedList<CLGMCommand> m_SendRecorder = new System.Collections.Generic.LinkedList<CLGMCommand>();

            public void Add(CLGMCommand command)
            {
                if (m_SendRecorder.Count == Captain)
                    m_SendRecorder.RemoveFirst();

                m_SendRecorder.AddLast(command);
            }

            public void Clear()
            {
                m_SendRecorder.Clear();
            }

            public override string ToString()
            {
                if (m_SendRecorder.Count == 0)
                    return "No recorder.";

                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                int i = 0;
                foreach (var current in m_SendRecorder)
                {
                    sb.AppendLine(string.Empty.PadRight(100, '#'));
                    sb.AppendFormat("No.{0}:", ++i);
                    sb.AppendLine();
                    if (current.Params[0].Equals("0"))
                        sb.AppendFormat("Receiver:{0}", "Your self");
                    else
                        sb.AppendFormat("Receiver ID:{0}", current.Params[0]);
                    sb.AppendFormat(" Sender:{0}", current.Params[1]);
                    sb.AppendFormat(" Priotity:{0}", current.Params[4]);
                    sb.AppendLine();
                    sb.AppendFormat("Title:{0}", current.Params[2]);
                    sb.AppendLine();
                    sb.AppendLine(current.Params[3]);
                    sb.AppendLine("Attachments:");
                    if (current.Params.Count == 5)
                        sb.AppendLine("==Nothing.==");
                    else
                    {
                        for (int j = 6; j < current.Params.Count; j++)
                            sb.AppendLine(current.Params[j]);
                    }
                }

                return sb.ToString();
            }
        }

        private void ResetData()
        {
            m_Receiver = "Receiver id. Default value is send for yourself.";
            m_Sender = "Sender name.";
            m_Title = "Title";
            m_Priotity = "Mail Priotity. The bigger, the top. Default value is 0.";
            m_Message = "Here is the mail content.";
            m_Attachments.Clear();
            m_AttachmentId = "0";
            m_AttachmentCount = "0";
            m_TaskId = "0";
            m_CurrentAttachment = string.Empty;
        }
        private void DrawSendMail()
        {
            GUILayout.Label("<b><color=lime><size=20>Send a test mail.</size></color></b>");
            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal();
                {
                    m_Receiver = GUILayout.TextField(m_Receiver, GUILayout.Width(120));
                    m_Sender = GUILayout.TextField(m_Sender, GUILayout.Width(120));
                    m_Title = GUILayout.TextField(m_Title, GUILayout.Width(120));
                    m_Priotity = GUILayout.TextField(m_Priotity);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("ID", GUILayout.Width(15));
                    m_AttachmentId = GUILayout.TextField(m_AttachmentId, 10, GUILayout.Width(50));
                    m_AttachmentId = System.Text.RegularExpressions.Regex.Replace(m_AttachmentId, "[^0-9]", string.Empty);

                    GUILayout.Label("Count", GUILayout.Width(35));
                    m_AttachmentCount = GUILayout.TextField(m_AttachmentCount, 10, GUILayout.Width(50));
                    m_AttachmentCount = System.Text.RegularExpressions.Regex.Replace(m_AttachmentCount, "[^0-9]", string.Empty);

                    if (GUILayout.Button("Attachement", GUILayout.Height(22), GUILayout.Width(100)))
                    {
                        m_Attachments[int.Parse(m_AttachmentId)] = int.Parse(m_AttachmentCount);
                        m_Attachments.Remove(0);

                        System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        int counter = 0;
                        foreach (var kvp in m_Attachments)
                        {
                            sb.Append(string.Format("ID={0},Count={1}", kvp.Key, kvp.Value));
                            counter++;
                            if (counter != m_Attachments.Count)
                                sb.AppendLine();
                        }
                        m_CurrentAttachment = sb.ToString();
                        m_AttachmentId = string.Empty;
                        m_AttachmentCount = string.Empty;
                    }
                }

                GUILayout.EndHorizontal();
                GUILayout.Label("If you want to add multiple attachments, after input <color=lime>ID </color>And <color=lime>Count, </color>Then click<color=yellow> Attachement </color>button");

                GUILayout.BeginVertical();
                {
                    m_Message = GUILayout.TextArea(m_Message);
                    if (!m_CurrentAttachment.Equals(string.Empty))
                        GUILayout.Label(m_CurrentAttachment);
                }
                GUILayout.EndVertical();

                if (GUILayout.Button("Send A Test Mail", GUILayout.Height(30)))
                {
                    CLGMCommand request = new CLGMCommand();
                    request.Type = (int)GMCommandType.SendMail;

                    int receiveId = 0;
                    int.TryParse(m_Receiver, out receiveId);
                    receiveId = Mathf.Max(0, receiveId - 1000000);

                    request.Params.Add(receiveId.ToString());
                    request.Params.Add(m_Sender);
                    request.Params.Add(m_Title);
                    request.Params.Add(m_Message);

                    int p = 0;
                    int.TryParse(m_Priotity, out p);
                    request.Params.Add(p.ToString());

                    foreach (var kvp in m_Attachments)
                    {
                        if (kvp.Key > 0 && kvp.Value > 0)
                            request.Params.Add(string.Format("{0}={1}", kvp.Key, kvp.Value));
                    }

                    GameEntry.Network.Send(request);

                    m_SenderRecorder.Add(request);
                    ResetData();
                }
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Show Recorder", GUILayout.Width(100)))
                    {
                        m_Recorder = m_SenderRecorder.ToString();
                    }
                    if (GUILayout.Button("Hide Recorder", GUILayout.Width(100)))
                    {
                        m_Recorder = string.Empty;
                    }
                    if (GUILayout.Button("Clear Recorder", GUILayout.Width(100)))
                    {
                        m_SenderRecorder.Clear();
                        m_Recorder = string.Empty;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Label(m_Recorder);
            }

            GUILayout.EndVertical();
        }

        private void DrawAddTask()
        {
            GUILayout.BeginVertical("box");
            {
                GUILayout.Label("<b><color=lime>Add Task.</color></b>");
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("TaskID", GUILayout.Width(50));
                    m_TaskId = GUILayout.TextField(m_TaskId, 10, GUILayout.Width(50));
                    if (GUILayout.Button("AddTask", GUILayout.Height(22), GUILayout.Width(100)))
                    {
                        int taskId = 0;
                        if (int.TryParse(m_TaskId, out taskId))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddTask;
                            request.Params.Add(taskId.ToString());
                            GameEntry.Network.Send(request);
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndHorizontal();
        }

        private void DrawOpenFunction()
        {
            GUILayout.BeginVertical("box");
            {
                GUILayout.Label("<b>功能开放，默认开，点击后重启游戏即可</b>");
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("On OpenFunction"))
                    {
                        GameEntry.OpenFunction.SwtichOpenFunction(true);
                    }
                    if (GUILayout.Button("Off OpenFunction"))
                    {
                        GameEntry.OpenFunction.SwtichOpenFunction(false);
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private string GroupID="0";

        private void DrawNoviceGuide()
        {
            GUILayout.BeginVertical("box");
            {
                GUILayout.Label("<b>新手引导，默认开，点击后重启游戏即可</b>");
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("On NoviceGuide"))
                    {
                        GameEntry.NoviceGuide.SwitchNoviceGuide(true);
                    }
                    if (GUILayout.Button("Off NoviceGuide"))
                    {
                        GameEntry.NoviceGuide.SwitchNoviceGuide(false);
                    }
                    GroupID = GUILayout.TextField(GroupID, GUILayout.Width(120));
                    if (GUILayout.Button("Jump GroupID"))
                    {
                        GameEntry.NoviceGuide.ResetTo(int.Parse(GroupID));
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        bool autoFight = false;
        void DrawAutoFight()
        {
            GUILayout.BeginVertical("box");
            bool newValue = GUILayout.Toggle(autoFight, "打开自动战斗功能");
            if (newValue!=autoFight)
            {
                GameEntry.Setting.SetBool("AUTO_FIGHT", newValue);
                autoFight = newValue;
                if (GameEntry.SceneLogic.BattleForm)
                    GameEntry.SceneLogic.BattleForm.SetAutoFightBtn(autoFight);
            }
            GUILayout.EndVertical();
        }
    }
}
