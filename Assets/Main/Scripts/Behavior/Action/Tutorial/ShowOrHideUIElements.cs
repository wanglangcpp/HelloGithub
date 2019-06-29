using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Genesis.GameClient.Behavior
{
    [TaskCategory("Game/Tutorial")]
    public class ShowOrHideUIElements : Action
    {
        [SerializeField]
        private string m_UIFormName = null;

        [SerializeField]
        private UIFormId m_UIFormId = UIFormId.Main;

        [SerializeField]
        private string[] m_UIElementPaths = new string[0];

        [SerializeField]
        private bool m_ShouldHide = true;

        [SerializeField]
        private bool m_RecoverOnClose = true;

        public override TaskStatus OnUpdate()
        {
            if (m_UIElementPaths == null || m_UIElementPaths.Length == 0)
            {
                return TaskStatus.Failure;
            }

            var uiFormId = m_UIFormId;
            if (!string.IsNullOrEmpty(m_UIFormName))
            {
                uiFormId = (UIFormId)System.Enum.Parse(typeof(UIFormId), m_UIFormName);
            }

            NGUIForm uiForm = UIUtility.GetUIForm(uiFormId);
            if (uiForm == null)
            {
                return TaskStatus.Failure;
            }

            bool shouldFail = false;
            for (int i = 0; i < m_UIElementPaths.Length; ++i)
            {
                var path = m_UIElementPaths[i];

                if (string.IsNullOrEmpty(path))
                {
                    shouldFail = true;
                    continue;
                }

                var trans = uiForm.CachedTransform.Find(path);
                if (trans == null)
                {
                    shouldFail = true;
                }

                if (m_RecoverOnClose)
                {
                    uiForm.CacheActivenessForRecoveryOnClose(path);
                }

                if (trans != null)
                {
                    trans.gameObject.SetActive(!m_ShouldHide);
                }
            }

            return shouldFail ? TaskStatus.Failure : TaskStatus.Success;
        }
    }
}
