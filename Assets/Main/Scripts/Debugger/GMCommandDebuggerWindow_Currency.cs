using GameFramework.Debugger;
using UnityEngine;

namespace Genesis.GameClient
{
    public class GMCommandDebuggerWindow_Currency : IDebuggerWindow
    {
        private Vector2 m_ScrollPosition = Vector2.zero;

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
            if (!(GameEntry.Procedure.CurrentProcedure is ProcedureMain && !GameEntry.OfflineMode.OfflineModeEnabled))
            {
                DrawTips();
                return;
            }

            m_ScrollPosition = GUILayout.BeginScrollView(m_ScrollPosition);
            {
                DrawSectionMoneyCommand();
                DrawSectionCoinCommand();
                DrawSectionEnergyCommand();
                DrawSectionSpiritCommand();
                DrawSectionMeridianEnergyCommand();
                DrawSectionArenaTokenCommand();
                DrawSectionPvpTokenCommand();
                DrawSectionActivenessTokenCommand();
            }
            GUILayout.EndScrollView();
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

        private void DrawSectionMoneyCommand()
        {
            GUILayout.Label("<b>Money</b>");
            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 8; i++)
                    {
                        if (GUILayout.Button(string.Format("+{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddMoney;
                            request.Params.Add(unit.ToString());
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 8; i++)
                    {
                        if (GUILayout.Button(string.Format("-{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddMoney;
                            request.Params.Add(string.Format("-{0}", unit.ToString()));
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void DrawSectionCoinCommand()
        {
            GUILayout.Label("<b>Coin</b>");
            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 8; i++)
                    {
                        if (GUILayout.Button(string.Format("+{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddCoin;
                            request.Params.Add(unit.ToString());
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 8; i++)
                    {
                        if (GUILayout.Button(string.Format("-{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddCoin;
                            request.Params.Add(string.Format("-{0}", unit.ToString()));
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void DrawSectionEnergyCommand()
        {
            GUILayout.Label("<b>Energy</b>");
            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        if (GUILayout.Button(string.Format("+{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddEnergy;
                            request.Params.Add(unit.ToString());
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        if (GUILayout.Button(string.Format("-{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddEnergy;
                            request.Params.Add(string.Format("-{0}", unit.ToString()));
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void DrawSectionSpiritCommand()
        {
            GUILayout.Label("<b>Spirit Token</b>");
            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        if (GUILayout.Button(string.Format("+{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddSpiritToken;
                            request.Params.Add(unit.ToString());
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        if (GUILayout.Button(string.Format("-{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddSpiritToken;
                            request.Params.Add(string.Format("-{0}", unit.ToString()));
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void DrawSectionMeridianEnergyCommand()
        {
            GUILayout.Label("<b>Meridian Token</b>");
            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        if (GUILayout.Button(string.Format("+{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddMeridianToken;
                            request.Params.Add(unit.ToString());
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        if (GUILayout.Button(string.Format("-{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddMeridianToken;
                            request.Params.Add(string.Format("-{0}", unit.ToString()));
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void DrawSectionArenaTokenCommand()
        {
            GUILayout.Label("<b>Arena Token</b>");
            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        if (GUILayout.Button(string.Format("+{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddArenaToken;
                            request.Params.Add(unit.ToString());
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        if (GUILayout.Button(string.Format("-{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddArenaToken;
                            request.Params.Add(string.Format("-{0}", unit.ToString()));
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void DrawSectionPvpTokenCommand()
        {
            GUILayout.Label("<b>Pvp Token</b>");
            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        if (GUILayout.Button(string.Format("+{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddPvpToken;
                            request.Params.Add(unit.ToString());
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        if (GUILayout.Button(string.Format("-{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddPvpToken;
                            request.Params.Add(string.Format("-{0}", unit.ToString()));
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }

        private void DrawSectionActivenessTokenCommand()
        {
            GUILayout.Label("<b>Activeness Token</b>");
            GUILayout.BeginVertical("box");
            {
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        if (GUILayout.Button(string.Format("+{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddActivenessToken;
                            request.Params.Add(unit.ToString());
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    int unit = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        if (GUILayout.Button(string.Format("-{0}", unit.ToString()), GUILayout.Height(30)))
                        {
                            CLGMCommand request = new CLGMCommand();
                            request.Type = (int)GMCommandType.AddActivenessToken;
                            request.Params.Add(string.Format("-{0}", unit.ToString()));
                            GameEntry.Network.Send(request);
                        }
                        unit *= 10;
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
    }
}
