using System.Collections.Generic;
using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CameraShakingConfig : ScriptableObject
    {
        public const int MaxLayerCount = 8;

        [SerializeField]
        private List<Config> m_Configs = new List<Config>();

        public Config this[int index]
        {
            get
            {
                return m_Configs[index];
            }
        }

        public int Count
        {
            get
            {
                return m_Configs.Count;
            }
        }
    }
}
