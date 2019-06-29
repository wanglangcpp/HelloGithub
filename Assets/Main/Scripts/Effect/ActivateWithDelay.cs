﻿using System;
using System.Collections;
using UnityEngine;

namespace Genesis.GameClient
{
    public class ActivateWithDelay : MonoBehaviour
    {
        [SerializeField]
        private float m_Delay = 0f;

        [SerializeField]
        private GameObject m_Target = null;

        [SerializeField]
        private bool m_ForceInactiveOnEnable = true;

        private void Awake()
        {
            if (m_Target == gameObject)
            {
                throw new Exception("Target cannot be the same as THIS game object.");
            }
        }

        private void OnEnable()
        {
            if (m_ForceInactiveOnEnable) m_Target.SetActive(false);
            StartCoroutine(WaitAndActivate());
        }

        private IEnumerator WaitAndActivate()
        {
            yield return new WaitForSeconds(m_Delay);

            if (m_Target != null)
            {
                m_Target.SetActive(true);
            }
        }
    }
}
