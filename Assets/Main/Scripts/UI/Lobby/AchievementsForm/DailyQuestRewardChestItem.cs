using GameFramework;
using UnityEngine;

namespace Genesis.GameClient
{
    public class DailyQuestRewardChestItem : MonoBehaviour
    {
        [SerializeField]
        private GameObject OpenChest = null;

        [SerializeField]
        private GameObject CloseChest = null;

        [SerializeField]
        private UILabel ActivenessDegree = null;

        public int Activeness { get; set; }

        public void Refresh(int maxActiveness, int activeness, bool isOpen)
        {
            CloseChest.SetActive(!isOpen);
            OpenChest.SetActive(isOpen);
            ActivenessDegree.text = activeness.ToString();
            Activeness = activeness;
        }
    }
}
