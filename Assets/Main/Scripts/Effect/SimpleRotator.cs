using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class SimpleRotator : MonoBehaviour
    {
        [SerializeField]
        private float m_AngularSpeed = 30f;

        [SerializeField]
        private Vector3 m_Axis = Vector3.up;

        [SerializeField]
        private Transform m_Target = null;

        private void Awake()
        {
            if (m_Target == transform)
            {
                throw new Exception("Target cannot be the same as THIS game object.");
            }
        }

        private void Update()
        {
            if (m_Target != null)
            {
                m_Target.Rotate(m_Axis, m_AngularSpeed * Time.deltaTime, Space.Self);
            }
        }
    }
}
