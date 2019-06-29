using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    public class GearInfoPanel : MonoBehaviour
    {
        [SerializeField]
        private UILabel m_GearTypeText = null;

        [SerializeField]
        private UILabel m_GearNameText = null;

        [SerializeField]
        private UILabel m_GearLvText = null;

        [SerializeField]
        private UILabel m_DressBtnText = null;

        [SerializeField]
        private UISprite m_GearIcon = null;

        [SerializeField]
        private GameObject m_DressBtn = null;

        [SerializeField]
        private UILabel m_UndressBtnText = null;

        [SerializeField]
        private GameObject m_UndressBtn = null;

        [SerializeField]
        private UIGrid m_GearAttributeList = null;

        [SerializeField]
        private GameObject m_GearAttribute = null;

        [SerializeField]
        private GameObject m_WholeScreenBtn = null;

        [SerializeField]
        private UISprite[] m_StrenghtenStars = null;

        private int m_HeroId = 0;
        private GearData m_Gear = null;
        private DressedGearItem m_GearItem = null;

        private void Awake()
        {

        }

        private void OnDisable()
        {

        }

        private void OnEnable()
        {

        }

        private void Start()
        {
            GameObject parent = transform.parent.gameObject;
            if (parent.GetComponent<GearPacketWithoutRecommendationPanel>() != null || parent.GetComponent<GearPacketPanel>() != null)
            {
                m_WholeScreenBtn.SetActive(false);
            }
            else
            {
                m_WholeScreenBtn.SetActive(true);
            }
        }

        public void RefreshData(GearData gear, bool isDress, bool isUnDress, int heroId, bool isMyHero, DressedGearItem gearItem = null)
        {
            m_GearItem = gearItem;
            m_HeroId = heroId;
            m_Gear = gear;
            m_GearIcon.LoadAsync(gear.IconId);
            if (isDress && isMyHero)
            {
                m_UndressBtn.SetActive(false);
                m_DressBtn.SetActive(true);
                m_DressBtnText.text = GameEntry.Localization.GetString("UI_BUTTON_DRESS");
            }
            else if (isUnDress && isMyHero)
            {
                m_UndressBtn.SetActive(true);
                m_DressBtn.SetActive(false);
                m_UndressBtnText.text = GameEntry.Localization.GetString("UI_BUTTON_UNDRESS");
            }
            else
            {
                m_UndressBtn.SetActive(false);
                m_DressBtn.SetActive(false);
            }

            m_GearTypeText.text = GameEntry.Localization.GetString(Constant.Gear.GearTypeNameDics[gear.Position]);
            m_GearNameText.text = GameEntry.Localization.GetString(gear.Name);
            m_GearNameText.color = ColorUtility.GetColorForQuality(gear.Quality);
            m_GearLvText.text = GameEntry.Localization.GetString("UI_TEXT_USERLEVELNUMBER", gear.Level.ToString());
            UIUtility.SetStarLevel(m_StrenghtenStars, gear.Level - 1);

            StartCoroutine(ClearAndShowAttributesCo(gear));
        }

        private IEnumerator ClearAndShowAttributesCo(GearData gear)
        {
            var children = m_GearAttributeList.GetChildList();
            for (int i = 0; i < children.Count; ++i)
            {
                Destroy(children[i].gameObject);
            }

            yield return null;

            ShowAttributes(gear);
            m_GearAttributeList.Reposition();
        }

        // Called by NGUI via reflection
        public void OnClickDress()
        {
            GameObject parent = transform.parent.gameObject;
            if (parent.GetComponent<GearPacketWithoutRecommendationPanel>() != null)
            {
                Destroy(parent);
            }
            if (parent.GetComponent<GearPacketPanel>() != null)
            {
                parent.SetActive(false);
            }
            Destroy(this.gameObject);
            if (m_HeroId > 0)
            {
//                 CLChangeGear request = new CLChangeGear();
//                 request.PutOnHeroId = m_HeroId;
//                 request.PutOnGearId = m_Gear.Id;
//                 GameEntry.Network.Send(request);
            }
            else
            {
                //var allHeroes = UIUtility.GetLobbyHeroesIncludingUnpossessed();
            }
        }

        public void OnClickUndress()
        {
            GameObject parent = transform.parent.gameObject;
            if (parent.GetComponent<GearPacketWithoutRecommendationPanel>() != null)
            {
                Destroy(parent);
            }
            if (parent.GetComponent<GearPacketPanel>() != null)
            {
                parent.SetActive(false);
            }
            Destroy(this.gameObject);
//             CLChangeGear request = new CLChangeGear();
//             request.TakeOffHeroId = m_HeroId;
//             request.PutOnGearId = 0;
//             request.TakeOffGearId = m_Gear.Id;
//             GameEntry.Network.Send(request);
        }

        public void OnClickWholeScreenBtn()
        {
            if (m_GearItem != null)
            {
                m_GearItem.SetSelect(false);
            }
            Destroy(this.gameObject);
        }

        private void ShowAttributes(GearData gear)
        {
            var displayer = new GearAttributeDisplayer<GearInfoAttributeItem>(gear, GearAttributeNewValueMask.Default);
            displayer.GetItemDelegate += CreateItem;
            displayer.SetNameAndCurrentValueDelegate += SetAttribute;
            displayer.Run();
        }

        private GearInfoAttributeItem CreateItem(int index)
        {
            var go = NGUITools.AddChild(m_GearAttributeList.gameObject, m_GearAttribute);
            var script = go.GetComponent<GearInfoAttributeItem>();
            return script;
        }

        private void SetAttribute(GearInfoAttributeItem script, string name, string value)
        {
            script.Name = name;
            script.Value = value;
        }

        public void OnClickChangeBtn()
        {
            GameEntry.UI.OpenUIForm(UIFormId.InventoryForm, new InventoryDisplayData
            {
                InventoryType = InventoryForm.InventoryType.GearChange,
                HeroType = m_HeroId,
                ChangeGearId = m_Gear.Id,
                ChangeGearPosition = m_Gear.Position,
            });
            Destroy(this.gameObject);
        }
    }
}
