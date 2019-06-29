using GameFramework;

namespace Genesis.GameClient
{
    public partial class NGUIAtlasComponent
    {
        /// <summary>
        /// 用于加载图集的用户数据类。
        /// </summary>
        public class LoadAtlasData
        {
            /// <summary>
            /// 加载成功的回调。
            /// </summary>
            public GameFrameworkAction<string, object, object> OnLoadAtlasSuccess { get; private set; }

            /// <summary>
            /// 加载失败的回调。
            /// </summary>
            public GameFrameworkAction<string, string, object> OnLoadAtlasFailure { get; private set; }

            /// <summary>
            /// 其他用户数据。
            /// </summary>
            public object UserData { get; private set; }

            /// <summary>
            /// 构造器。
            /// </summary>
            /// <param name="onLoadAtlasSuccess">加载成功的回调。</param>
            /// <param name="onLoadAtlasFailure">加载失败的回调。</param>
            /// <param name="userData">其他用户数据。</param>
            public LoadAtlasData(GameFrameworkAction<string, object, object> onLoadAtlasSuccess,
                GameFrameworkAction<string, string, object> onLoadAtlasFailure, object userData)
            {
                OnLoadAtlasSuccess = onLoadAtlasSuccess;
                OnLoadAtlasFailure = onLoadAtlasFailure;
                UserData = userData;
            }
        }
    }
}
