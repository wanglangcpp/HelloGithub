using GameFramework;

namespace Genesis.GameClient
{
    public partial class TextureComponent
    {
        /// <summary>
        /// 用于加载贴图的用户数据类。
        /// </summary>
        private class LoadTextureData
        {
            /// <summary>
            /// 加载成功的回调。
            /// </summary>
            public GameFrameworkAction<string, object, object> LoadTextureSuccessCallback
            {
                get;
                private set;
            }

            /// <summary>
            /// 加载失败的回调。
            /// </summary>
            public GameFrameworkAction<string, string, object> LoadTextureFailureCallback
            {
                get;
                private set;
            }

            /// <summary>
            /// 其他用户数据。
            /// </summary>
            public object UserData
            {
                get;
                private set;
            }

            /// <summary>
            /// 构造器。
            /// </summary>
            /// <param name="loadTextureSuccessCallback">加载成功的回调。</param>
            /// <param name="loadTextureFailureCallback">加载失败的回调。</param>
            /// <param name="userData">其他用户数据。</param>
            public LoadTextureData(GameFrameworkAction<string, object, object> loadTextureSuccessCallback, GameFrameworkAction<string, string, object> loadTextureFailureCallback, object userData)
            {
                LoadTextureSuccessCallback = loadTextureSuccessCallback;
                LoadTextureFailureCallback = loadTextureFailureCallback;
                UserData = userData;
            }
        }
    }
}
