using GameFramework;
using System;
using UnityEngine;

namespace Genesis.GameClient
{
    public class AttachEffect : MonoBehaviour
    {
        [SerializeField]
        private AttachedEffect[] m_AttachedEffects = null;

        private void Awake()
        {
            var colorChanger = GetComponent<ColorChanger>();

            for (int i = 0; i < m_AttachedEffects.Length; i++)
            {
                AttachedEffect attachedEffect = m_AttachedEffects[i];
                if (attachedEffect.AttachEffect == null)
                {
                    Log.Warning("Attached effect on '{0}' is invalid.", gameObject.name);
                    continue;
                }

                GameObject go = Instantiate(attachedEffect.AttachEffect);
                Transform transform = go.GetComponent<Transform>();
                transform.parent = attachedEffect.AttachPoint;
                transform.localPosition = attachedEffect.Offset;
                transform.localRotation = Quaternion.Euler(attachedEffect.Rotation);
                transform.localScale = attachedEffect.Scale;

                if (colorChanger == null)
                {
                    continue;
                }

                var renderers = go.GetComponentsInChildren<Renderer>();
                for (int j = 0; j < renderers.Length; j++)
                {
                    colorChanger.AddRenderer(renderers[j]);
                }
            }
        }

        [Serializable]
        private class AttachedEffect
        {
            [SerializeField]
            private Transform m_AttachPoint = null;

            [SerializeField]
            private GameObject m_AttachEffect = null;

            [SerializeField]
            private Vector3 m_Offset = Vector3.zero;

            [SerializeField]
            private Vector3 m_Rotation = Vector3.zero;

            [SerializeField]
            private Vector3 m_Scale = Vector3.one;

            public Transform AttachPoint
            {
                get
                {
                    return m_AttachPoint;
                }
            }

            public GameObject AttachEffect
            {
                get
                {
                    return m_AttachEffect;
                }
            }

            public Vector3 Offset
            {
                get
                {
                    return m_Offset;
                }
            }

            public Vector3 Rotation
            {
                get
                {
                    return m_Rotation;
                }
            }

            public Vector3 Scale
            {
                get
                {
                    return m_Scale;
                }
            }
        }
    }
}
