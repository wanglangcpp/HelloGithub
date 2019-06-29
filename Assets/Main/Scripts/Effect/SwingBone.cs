// Physics - Swing Bone Component
// Ray@raymix.net

using UnityEngine;
using System.Collections.Generic;

namespace Genesis.GameClient
{

    [ExecuteInEditMode]
    [AddComponentMenu("Physics/Swing Bone")]
    public class SwingBone : MonoBehaviour
    {
        private bool IsRoot = true;
        private Transform baseTransform;

        private Vector3 m_LastPos = Vector3.zero;
        private Quaternion m_LastRot = Quaternion.identity;

        private List<SwingBone> m_Bones = new List<SwingBone>();


        [Range(0.01f, 100.0f)]
        public float drag = 50;

        [Range(0.01f, 100.0f)]
        public float angelDrag = 50;

        public bool affectChild = true;


        // Use this for initialization
        [ExecuteInEditMode]
        void Start()
        {
            baseTransform = this.transform;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                Transform child = this.transform.GetChild(i);
                SwingBone bone = child.GetComponent<SwingBone>();
                if (bone == null)
                {
                    bone = child.gameObject.AddComponent<SwingBone>();
                    bone.drag = this.drag;
                    bone.angelDrag = this.angelDrag;
                }
                bone.IsRoot = false;
                m_Bones.Add(bone);
            }
        }

        [ExecuteInEditMode]
        void LateUpdate()
        {
            if (this.IsRoot)
                this.UpdateBone();
        }

        void UpdateBone()
        {
            if (baseTransform == null)
                return;
            if (m_LastRot != Quaternion.identity)
            {
                baseTransform.rotation = Quaternion.Lerp(m_LastRot, baseTransform.rotation, Mathf.Clamp01(angelDrag * Time.deltaTime));
            }

            if (m_LastRot != baseTransform.rotation)
            {
                if (m_LastPos != Vector3.zero && m_LastPos != baseTransform.position)
                {
                    Vector3 a = baseTransform.position - this.transform.parent.position;
                    Vector3 b = m_LastPos - this.transform.parent.position;
                    Vector3 v = a + (b - a) / 2;

                    m_LastRot = Quaternion.FromToRotation(a, b) * baseTransform.rotation;
                    baseTransform.rotation = Quaternion.Lerp(m_LastRot, baseTransform.rotation, Mathf.Clamp01(drag * Time.deltaTime));
                }
                else
                    m_LastRot = baseTransform.rotation;
            }

            m_LastPos = baseTransform.position;

            for (int i = 0; i < this.m_Bones.Count; i++)
            {
                if (affectChild)
                {
                    this.m_Bones[i].affectChild = this.affectChild;
                    this.m_Bones[i].drag = this.drag;
                    this.m_Bones[i].angelDrag = this.angelDrag;
                }
                this.m_Bones[i].UpdateBone();
            }
        }
    }
}
