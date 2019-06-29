using UnityEngine;

namespace Genesis.GameClient
{
    public sealed class PreloadPrefabs : MonoBehaviour
    {
#pragma warning disable 0414

        [SerializeField]
        private GameObject[] m_PreloadPrefabs = null;

#pragma warning restore 0414
    }
}
