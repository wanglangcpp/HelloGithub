using UnityEngine;

namespace Genesis.GameClient
{
    public partial class CharacterMotion
    {
        private CharacterLingeringParams m_LingeringParams;

        public class CharacterLingeringParams
        {
            public CharacterLingeringParams(Vector3 targetPosition, float speedFactor, float maxDistance, float angleScope)
            {
                TargetPosition = targetPosition;
                SpeedFactor = speedFactor;
                MaxDistance = maxDistance;
                AngleScope = angleScope;
            }

            public float AngleScope { get; private set; }
            public float MaxDistance { get; private set; }
            public float SpeedFactor { get; private set; }
            public Vector3 TargetPosition { get; private set; }
        }
    }
}
