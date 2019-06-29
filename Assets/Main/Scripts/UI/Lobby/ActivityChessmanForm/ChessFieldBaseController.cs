using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public abstract class ChessFieldBaseController : MonoBehaviour
    {
        [SerializeField]
        private ActivityChessmanForm m_Form = null;

        [SerializeField]
        private int m_Index = -1;

        [SerializeField]
        private int m_Row = 0;

        [SerializeField]
        private UIPanel[] m_Panels = null;

        public IList<UIPanel> GetPanels()
        {
            if (m_Panels == null)
            {
                return new UIPanel[0];
            }

            return m_Panels;
        }

        protected UIEffectsController m_EffectController = null;

        private const int DeltaDepthPerRow = 3;

        protected ActivityChessmanForm Form
        {
            get
            {
                if (m_Form == null)
                {
                    m_Form = GetComponentInParent<ActivityChessmanForm>();
                }

                return m_Form;
            }
        }

        public virtual void Init(int index, int row)
        {
            m_Index = index;
            m_Row = row;

            var panels = GetPanels();
            for (int i = 0; i < panels.Count; ++i)
            {
                panels[i].depth += m_Row * DeltaDepthPerRow;
            }
        }

        public virtual void OnButtonClick()
        {
            Form.OnClickChessField(m_Index);
        }

        public abstract void RefreshData(ChessField data);

        private void Awake()
        {
            m_EffectController = GetComponent<UIEffectsController>();
        }

        protected virtual void Start()
        {
            var widgets = GetComponentsInChildren<UIWidget>(true);
            for (int i = 0; i < widgets.Length; ++i)
            {
                if (widgets[i].gameObject == gameObject)
                {
                    continue;
                }

                widgets[i].depth += m_Row * DeltaDepthPerRow;
            }
        }

        private void OnEnable()
        {
            if (m_EffectController != null)
            {
                m_EffectController.Resume();
            }
        }

        private void OnDisable()
        {
            if (m_EffectController != null)
            {
                m_EffectController.Pause();
            }
        }
    }
}
