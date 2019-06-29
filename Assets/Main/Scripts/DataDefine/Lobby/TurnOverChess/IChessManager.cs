using GameFramework;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    /// <summary>
    /// 翻翻棋管理器接口。
    /// </summary>
    public interface IChessManager
    {
        /// <summary>
        /// 是否已经获取到数据。
        /// </summary>
        bool HasData { get; }

        /// <summary>
        /// 获得的星能。
        /// </summary>
        int StarEnergyObtained { get; }

        /// <summary>
        /// 获得的金币。
        /// </summary>
        int CoinObtained { get; }

        /// <summary>
        /// 获得的宝石。
        /// </summary>
        int MoneyObtained { get; }

        /// <summary>
        /// 获得的物品。
        /// </summary>
        KeyValuePair<int, int>[] GoodsObtained { get; }

        /// <summary>
        /// 剩余重置棋盘次数。
        /// </summary>
        int RemainingResetCount { get; }

        /// <summary>
        /// 剩余翻棋次数。
        /// </summary>
        int RemainingChanceCount { get; }

        /// <summary>
        /// 棋盘列数。
        /// </summary>
        int ChessBoardColCount { get; }

        /// <summary>
        /// 棋盘行数。
        /// </summary>
        int ChessBoardRowCount { get; }

        /// <summary>
        /// 棋子个数。
        /// </summary>
        int ChessBoardCount { get; }

        /// <summary>
        /// 可免费翻的灰色棋子个数。
        /// </summary>
        int FreeFieldCount { get; }

        /// <summary>
        /// 是否翻完所有灰色棋子。
        /// </summary>
        bool IsComplete { get; }

        /// <summary>
        /// 获取棋子数据。
        /// </summary>
        /// <param name="index">棋子在棋盘中的索引号。</param>
        /// <returns>棋子数据。</returns>
        ChessField this[int index] { get; }

        /// <summary>
        /// 刷新棋盘数据成功事件。
        /// </summary>
        event GameFrameworkAction OnChessBoardRefreshSuccess;

        /// <summary>
        /// 刷新期盼数据失败事件。
        /// </summary>
        event GameFrameworkAction OnChessBoardRefreshFailure;

        /// <summary>
        /// 打开棋子成功事件。
        /// </summary>
        event GameFrameworkAction<IList<int>, ReceivedGeneralItemsViewData> OnChessFieldOpenSuccess;

        /// <summary>
        /// 打开棋子失败事件。
        /// </summary>
        event GameFrameworkAction OnChessFieldOpenFailure;

        /// <summary>
        /// 使用炸弹成功事件。
        /// </summary>
        event GameFrameworkAction<IList<int>> OnBombSuccess;

        /// <summary>
        /// 使用炸弹失败事件。
        /// </summary>
        event GameFrameworkAction OnBombFailure;

        /// <summary>
        /// 获取对手数据成功事件。
        /// </summary>
        event GameFrameworkAction<int> OnGetEnemyDataSuccess;

        /// <summary>
        /// 获取对手数据失败事件。
        /// </summary>
        event GameFrameworkAction<int> OnGetEnemyDataFailure;

        /// <summary>
        /// 初始化。
        /// </summary>
        /// <param name="eventComponent">事件组件。</param>
        /// <param name="enemyDataGetter">获取对手数据的委托。</param>
        void Init(EventComponent eventComponent, GameFrameworkFunc<ChessBattleEnemyData> enemyDataGetter);

        /// <summary>
        /// 关闭。
        /// </summary>
        void ShutDown();

        /// <summary>
        /// 请求刷新棋盘。
        /// </summary>
        void RefreshChessBoard();

        /// <summary>
        /// 请求重置棋盘。
        /// </summary>
        void ResetChessBoard();

        /// <summary>
        /// 请求获取对手数据。
        /// </summary>
        /// <param name="chessFieldIndex">棋子索引号。</param>
        void GetEnemyData(int chessFieldIndex);

        /// <summary>
        /// 请求翻开普通棋子。
        /// </summary>
        /// <param name="index">棋子索引号。</param>
        void OpenNormalChessField(int index);

        /// <summary>
        /// 请求翻开彩色棋子。
        /// </summary>
        /// <param name="index">棋子索引号。</param>
        /// <param name="won">是否赢得了战斗。</param>
        void OpenBattleChessField(int index, bool won);

        /// <summary>
        /// 请求快速翻棋。
        /// </summary>
        void PerformQuickOpen();

        /// <summary>
        /// 请求使用炸弹。
        /// </summary>
        void Bomb();
    }
}
