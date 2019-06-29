namespace Genesis.GameClient
{
    /// <summary>
    /// <see cref="Genesis.GameClient.NGUIForm"/> 使用的用户自定义数据的基类。
    /// </summary>
    public abstract class UIFormBaseUserData
    {
        private bool m_ShouldOpenImmediately = false;

        public bool ShouldOpenImmediately
        {
            get
            {
                return m_ShouldOpenImmediately;
            }
            set
            {
                m_ShouldOpenImmediately = value;
            }
        }
    }
}
