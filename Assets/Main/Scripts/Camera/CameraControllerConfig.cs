using GameFramework;
using GameFramework.Fsm;
using GameFramework.Resource;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Genesis.GameClient
{
    public class CameraControllerConfig : MonoBehaviour
    {
        [SerializeField]
        public Vector3 m_CameraOffset = new Vector3(-0.1f, 6.8f, -5f);

        [SerializeField]
        public float m_CenterOffset = 0.4f;

        [SerializeField]
        public float m_SpeedLimit = 15f;

        [SerializeField]
        public float m_ChaseUpTime = 0.2f;

        [SerializeField]
        public float m_LazyThDistance = 0.02f;
    }
}
