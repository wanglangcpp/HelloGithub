using GameFramework;
using GameFramework.Event;
using System.Collections.Generic;

namespace Genesis.GameClient
{
    public partial class ChessBattleInstanceLogic
    {
        protected override AbstractInstanceSuccess CreateSuccessResult()
        {
            return new InstanceSuccess();
        }

        protected override AbstractInstanceFailure CreateFailureResult(bool shouldOpenUI)
        {
            return new InstanceFailure(shouldOpenUI);
        }

        private class InstanceSuccess : PvpaiInstanceSuccess
        {
            public override void Init(BaseInstanceLogic instanceLogic, InstanceSuccessReason reason, GameFrameworkAction onComplete)
            {
                base.Init(instanceLogic, reason, onComplete);
                GameEntry.ChessManager.OnChessFieldOpenSuccess += OnChessFieldOpenSuccess;
                GameEntry.ChessManager.OnChessFieldOpenFailure += OnChessFieldOpenFailure;
            }

            public override void ShutDown()
            {
                if (GameEntry.IsAvailable)
                {
                    GameEntry.ChessManager.OnChessFieldOpenSuccess -= OnChessFieldOpenSuccess;
                    GameEntry.ChessManager.OnChessFieldOpenFailure -= OnChessFieldOpenFailure;
                }

                base.ShutDown();
            }

            protected override void SendLeaveInstanceRequest()
            {
                GameEntry.ChessManager.OpenBattleChessField(GameEntry.Data.ChessBattleEnemy.ChessFieldIndex, true);
            }

            protected override void ShowResultUI(UIFormBaseUserData data)
            {
                GameEntry.UI.OpenUIForm(UIFormId.InstanceResultForm);
            }

            private void OnChessFieldOpenSuccess(IList<int> changeList, ReceivedGeneralItemsViewData receiveGoodsData)
            {
                OnReceiveLeaveInstanceResponse(null, null);
            }

            private void OnChessFieldOpenFailure()
            {
                OnReceiveLeaveInstanceResponse(null, null);
            }
        }

        private class InstanceFailure : PvpaiInstanceFailure
        {
            private InstanceFailureReason m_FailureReason = InstanceFailureReason.Unknown;

            public InstanceFailure(bool shouldOpenUI) : base(shouldOpenUI)
            {

            }

            public override void Init(BaseInstanceLogic instanceLogic, InstanceFailureReason reason, GameFrameworkAction onComplete)
            {
                base.Init(instanceLogic, reason, onComplete);
                m_FailureReason = reason;
                GameEntry.ChessManager.OnChessFieldOpenSuccess += OnChessFieldOpenSuccess;
                GameEntry.ChessManager.OnChessFieldOpenFailure += OnChessFieldOpenFailure;
            }

            public override void ShutDown()
            {
                if (GameEntry.IsAvailable)
                {

                    GameEntry.ChessManager.OnChessFieldOpenSuccess -= OnChessFieldOpenSuccess;
                    GameEntry.ChessManager.OnChessFieldOpenFailure -= OnChessFieldOpenFailure;
                }

                base.ShutDown();
            }

            protected override UIFormBaseUserData PopulateData(GameEventArgs e)
            {
                return new InstanceFailureData { FailureReason = m_FailureReason };
            }

            protected override void SendLeaveInstanceRequest()
            {
                GameEntry.ChessManager.OpenBattleChessField(GameEntry.Data.ChessBattleEnemy.ChessFieldIndex, false);
            }

            protected override void ShowResultUI(UIFormBaseUserData data)
            {
                GameEntry.UI.OpenUIForm(UIFormId.ChessBattleInstanceFailureForm, data);
            }

            private void OnChessFieldOpenSuccess(IList<int> changeList, ReceivedGeneralItemsViewData receiveGoodsData)
            {
                OnReceiveLeaveInstanceResponse(null, null);
            }

            private void OnChessFieldOpenFailure()
            {
                OnReceiveLeaveInstanceResponse(null, null);
            }
        }
    }
}
