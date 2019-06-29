using UnityEngine;

namespace Genesis.GameClient
{
    public class SelectPubliser : MonoBehaviour
    {

        private void Awake()
        {
            instance = this;
        }

        private static SelectPubliser instance;

        [SerializeField]
        private GameObject border;
        [SerializeField]
        private UIButton outBtn;
        [SerializeField]
        private UILabel outLabel;
        [SerializeField]
        private UIButton inBtn;
        [SerializeField]
        private UILabel inLabel;

        public static void OpenSelect(string title, string outer, string inner, EventDelegate outerAction, EventDelegate innerAction)
        {
            instance.border.gameObject.SetActive(true);
            instance.outLabel.text = outer;
            instance.inLabel.text = inner;
            instance.outBtn.onClick.Add(outerAction);
            instance.inBtn.onClick.Add(innerAction);

            if (instance.outBtn.onClick.Count == 1)
            {
                EventDelegate closeAction = new EventDelegate(
                delegate ()
                {
                    instance.border.gameObject.SetActive(false);
                });
                closeAction.oneShot = true;
                instance.outBtn.onClick.Add(closeAction);
            }

            if (instance.inBtn.onClick.Count == 1)
            {
                EventDelegate closeAction = new EventDelegate(
                delegate ()
                {
                    instance.border.gameObject.SetActive(false);
                });
                closeAction.oneShot = true;
                instance.inBtn.onClick.Add(closeAction);
            }
        }
    }

}
