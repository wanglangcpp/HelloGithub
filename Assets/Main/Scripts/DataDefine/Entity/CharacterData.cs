using System;
using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    [Serializable]
    public abstract class CharacterData : TargetableObjectData
    {
        [SerializeField]
        private int m_CharacterId;

        [SerializeField]
        private float m_BaseSpeed;

        [SerializeField]
        private float m_Speed;

        [SerializeField]
        private int m_CameraShakingIndexOnEnter = -1;

        [SerializeField]
        private BaseSteady m_Steady = null;

        private StateForImpactCalc m_StateForImpactCalc;

        public CharacterData(int entityId, CharacterDataModifierType modifierType = CharacterDataModifierType.Offline)
            : base(entityId, modifierType)
        {
            m_StateForImpactCalc = StateForImpactCalc.Normal;

            switch (modifierType)
            {
                case CharacterDataModifierType.Offline:
                    m_Steady = new OffLineSteady();
                    break;
                case CharacterDataModifierType.Online:
                    m_Steady = new OnLineSteady();
                    break;
            }
        }

        public new Character Entity
        {
            get
            {
                return base.Entity as Character;
            }
        }

        /// <summary>
        /// 模型编号（对应 Character 表编号）。
        /// </summary>
        public int CharacterId
        {
            get
            {
                return m_CharacterId;
            }
            set
            {
                m_CharacterId = value;
            }
        }

        /// <summary>
        /// 移动速度。
        /// </summary>
        public float Speed
        {
            get
            {
                return m_Speed;
            }

            set
            {
                m_BaseSpeed = value;
                RefreshSpeed();
            }
        }

        public int CameraShakingIndexOnEnter
        {
            get
            {
                return m_CameraShakingIndexOnEnter;
            }

            set
            {
                m_CameraShakingIndexOnEnter = value;
            }
        }

        /// <summary>
        /// 用于伤害计算的状态。
        /// </summary>
        public override StateForImpactCalc StateForImpactCalc
        {
            get
            {
                return m_StateForImpactCalc;
            }
        }

        public BaseSteady Steady
        {
            get
            {
                if (m_Steady.Owner == null)
                {
                    m_Steady.Owner = GameEntry.Entity.GetGameEntity(Id) as Character;
                }
                return m_Steady;
            }
        }

        public override void RefreshProperties()
        {
            base.RefreshProperties();
            RefreshSpeed();
        }

        public void SetStateForImpactCalc(StateForImpactCalc state)
        {
            m_StateForImpactCalc = state;
        }

        private void RefreshSpeed()
        {
            if (RefreshFloatProperty(new BuffType[] { BuffType.ChangeSpeed }, m_BaseSpeed, ref m_Speed))
            {
                GameEntry.Event.Fire(this, new CharacterPropertyChangeEventArgs(Id));
            }
        }

        public void OnShow()
        {
            m_Steady.OnShow();
        }

        public void OnHide()
        {
            m_Steady.OnHide();
        }

        public override void OnBuffPoolChanged(BuffData added, IList<BuffData> removed)
        {
            base.OnBuffPoolChanged(added, removed);

            if (Entity == null)
            {
                return;
            }

            if (added != null)
            {
                Entity.AddBuffEffects(added);
                OnAddSteadyBuffAdded(added);
            }

            if (removed != null && removed.Count > 0)
            {
                Entity.RemoveBuffEffects(removed);
            }
        }

        private void OnAddSteadyBuffAdded(BuffData buffData)
        {
            if (buffData == null || buffData.BuffType != BuffType.AddSteadyValueByPct)
            {
                return;
            }
            
            m_Steady.AddSteady(m_Steady.MaxSteady * buffData.Params[0]);
        }
    }
}
