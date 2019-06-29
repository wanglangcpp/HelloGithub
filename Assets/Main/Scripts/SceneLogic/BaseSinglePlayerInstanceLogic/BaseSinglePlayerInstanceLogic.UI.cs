namespace Genesis.GameClient
{
    public partial class BaseSinglePlayerInstanceLogic
    {
        public bool IsShowingBossAlert()
        {
            if (GameEntry.SceneLogic.BattleForm == null)
            {
                return false;
            }

            return GameEntry.SceneLogic.BattleForm.IsShowingBossAlert();
        }

        public bool ShowBossAlert(float keepTime,string BossNameKey)
        {
            if (GameEntry.SceneLogic.BattleForm == null)
            {
                return false;
            }

            return GameEntry.SceneLogic.BattleForm.ShowBossAlert(keepTime,BossNameKey);
        }

        public bool ShowBossHPBar()
        {
            return GameEntry.SceneLogic.BattleForm.ShowBossHPPanelIfValid();
        }

        public void HideBossHPBar()
        {
            GameEntry.SceneLogic.BattleForm.HideBossHPPanel();
        }
    }
}
