using UnityEngine;

namespace ProjectN.Characters.Nick.Data
{
    [CreateAssetMenu(fileName = "NewNickData", menuName = "Data/Nick Data/Base Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Move State")]
        public float movementVelocity = 10f;

        public float defaultDrag = 2f;

        [Header("Jump State")]
        public float jumpVelocity = 15f;
        public int amountOfJumps = 1;

        [Header("Wall Jump State")]
        public float wallJumpVelocity = 20;
        public float wallJumpTime = 0.4f;
        public Vector2 wallJumpAngle = new Vector2(1, 2);
        public float maxHorizontalJumpDistance = 1f;
        public float maxWallHeightDistance = 1f;
        public float smallWallJumpDistance = 1f;
        public float mediumWallJumpDistance = 2f;
        public float playerFeetThreshold = 1f;

        [Header("In Air State")]
        public float coyoteTime = 0.2f;
        public float variableJumpHeightMultiplier = 0.5f;

        [Header("Wall Slide State")]
        public float wallSlideVelocity = 3f;
        public float slideStartTime = 0.5f;

        [Header("Wall Climb State")]
        public float wallClimbVelocity = 3f;

        [Header("Ledge Climb State")]
        public Vector2 startOffset;
        public Vector2 stopOffset;

        [Header("Dash State")]
        public float dashCooldown = 0.5f;
        public float maxHoldTime = 1f;
        public float holdTimeScale = 0.25f;
        public float dashTime = 0.2f;
        public float dashVelocity = 30f;
        public float drag = 10f;
        public float dashEndYMultiplier = 0.2f;
        public float distBetweenAfterImages = 0.5f;
        public float dashSlideTime = 0.2f;
        public float dashSlideSpeed = 5f;

        [Header("Crouch States")]
        public float crouchMovementVelocity = 5f;
        public float crouchColliderHeight = 0.8f;
        public float standColliderHeight = 1.6f;

        [Header("Check Variables")]
        public float groundCheckRadius = 0.3f;
        public float wallCheckDistance = 0.5f;
        public LayerMask ledgeLayer;
        public LayerMask groundLayer;

        [Header("Ledge Jump")]
        public float ledgeFallColliderTime = 0.5f;

        [Header("Movables")] public LayerMask movablesLayer;
        public float pushVelocity = 12f;
        public float pullVelocity = 8f;
        public float blockCheckDistance = 4f;

        [Header("Health")]
        public float maxHealth = 100f;
    }
}
