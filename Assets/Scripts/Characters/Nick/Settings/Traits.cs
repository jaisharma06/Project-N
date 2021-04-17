using UnityEngine;

namespace Characters.Nick.Settings
{
    [CreateAssetMenu(fileName = "NickSettings", menuName = "Characters/Settings/Nick")]
    public class Traits : ScriptableObject
    {
        public float maxSlope = 45f;
        public int amountOfJumps = 1;
        public float jumpForce = 4f;
        public float fallMultiplier = 2.5f;
        public float airDragMultiplier = 0.95f;
        public float movementForceInAir = 1f;
        [Range(0,1)]
        public float variableJumpHeightMultiplier = 0.5f;
        public float walkSpeed = 4f;
        public float dodgeSpeed = 10f;
        public float dodgeTime = 1f;
        public float slidingSpeed = 5f;
        public float moveCooldownTimeAfterDodge = 0.2f;
        public float dodgeCooldownTime = 1f;
        public float maxHealth = 1f;
        public string characterName = "Nick";

        [Header("Wall Jumping")]
        public Vector2 wallHopDirection;
        public Vector2 wallJumpDirection;
        public float wallHopForce;
        public float wallJumpForce;
        public float wallSlideSpeed;
        public float wallSlideStartTime = 0.5f;
    }   
}
