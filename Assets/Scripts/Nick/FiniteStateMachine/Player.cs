using System;
using Assets.Scripts.Nick.States.SubStates;
using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.Input;
using ProjectN.Characters.Nick.States;
using ProjectN.Characters.Nick.Weapons.Inventory;
using UnityEngine;

namespace ProjectN.Characters.Nick.FiniteStateMachine
{
    public enum PlayerJumpType
    {
        NONE = 0,
        SMALL = 1,
        MEDIUM = 2,
        LARGE = 3,
        LEDGE = 4
    }

    public class Player : MonoBehaviour, IHealth
    {
        #region State Variables

        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerInAirState InAirState { get; private set; }
        public PlayerLandState LandState { get; private set; }
        public PlayerWallSlideState WallSlideState { get; private set; }
        public PlayerWallGrabState WallGrabState { get; private set; }
        public PlayerWallClimbState WallClimbState { get; private set; }
        public PlayerWallJumpState WallJumpState { get; private set; }
        public PlayerLedgeClimbState LedgeClimbState { get; private set; }
        public PlayerDashState DashState { get; private set; }
        public PlayerCrouchIdleState CrouchIdleState { get; private set; }
        public PlayerCrouchMoveState CrouchMoveState { get; private set; }
        public PlayerAttackState PrimaryAttackState { get; private set; }
        public PlayerPushBlockMoveState PushingMoveState { get; private set; }
        public PlayerPushBlockIdleState PushingIdleState { get; private set; }
        public PlayerJumpDownPlatformState JumpDownPlatformState { get; private set; }
        public PlayerJumpUpPlatformState JumpUpPlatformState { get; private set; }
        public PlayerLeaveBlockState LeaveBlockState { get; private set; }
        public PlayerHoldBlockState HoldBlockState { get; private set; }

        [SerializeField] private PlayerData playerData;
        public AnimationCurve jumpCurve;

        #endregion

        #region Components

        public Animator Anim { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }

        public Rigidbody2D RB { get; private set; }

        //public Transform DashDirectionIndicator { get; private set; }
        public BoxCollider2D MovementCollider { get; private set; }
        public Collider2D playerCollider { get; private set; }
        public PlayerInventory Inventory { get; private set; }

        #endregion

        #region Check Transforms

        [SerializeField] private Transform groundCheck;
        [SerializeField] private Transform wallCheck;
        [SerializeField] private Transform ledgeCheck;
        [SerializeField] private Transform ceilingCheck;
        [SerializeField] private Transform wallHeightCheck;

        #endregion

        #region Other Variables

        public Vector2 CurrentVelocity { get; private set; }
        public int FacingDirection { get; private set; }
        public Movable GrabbedMovable { get; private set; }

        private Vector2 workspace;

        public float health { get; private set; }
        public float maxHealth { get; set; }
        public bool isDead { get; set; }
        public bool isGrabbingMovable { get; set; }

        public float feetPosition
        {
            get { return playerCollider.bounds.min.y; }
        }

        public float jumpDuration
        {
            get => playerData.playerJumpDuration;
        }

        public PlayerJumpType jumpType;
        [NonSerialized] public Vector2 jumpEndPosition;

        #endregion

        #region Unity Callback Functions

        private void Awake()
        {
            StateMachine = new PlayerStateMachine();

            IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
            MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
            JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
            InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
            LandState = new PlayerLandState(this, StateMachine, playerData, "land");
            WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
            WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
            WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
            WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
            LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
            DashState = new PlayerDashState(this, StateMachine, playerData, "dash");
            CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
            CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
            PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
            PushingIdleState = new PlayerPushBlockIdleState(this, StateMachine, playerData, "pushingBlock");
            PushingMoveState = new PlayerPushBlockMoveState(this, StateMachine, playerData, "pushingBlock");
            JumpDownPlatformState = new PlayerJumpDownPlatformState(this, StateMachine, playerData, "jumpDownPlatform");
            JumpUpPlatformState = new PlayerJumpUpPlatformState(this, StateMachine, playerData, "jumpUpPlatform");
            LeaveBlockState = new PlayerLeaveBlockState(this, StateMachine, playerData, "leavingBlock");
            HoldBlockState = new PlayerHoldBlockState(this, StateMachine, playerData, "pushingBlock");
        }

        private void Start()
        {
            Anim = GetComponentInChildren<Animator>();
            InputHandler = GetComponent<PlayerInputHandler>();
            RB = GetComponent<Rigidbody2D>();
            //DashDirectionIndicator = transform.Find("DashDirectionIndicator");
            MovementCollider = GetComponent<BoxCollider2D>();
            Inventory = GetComponent<PlayerInventory>();
            playerCollider = GetComponent<Collider2D>();

            FacingDirection = 1;
            PrimaryAttackState.SetWeapon(Inventory.weapons[(int) CombatInputs.primary]);

            maxHealth = playerData.maxHealth;
            health = maxHealth;

            StateMachine.Initialize(IdleState);
        }

        private void Update()
        {
            CurrentVelocity = RB.velocity;
            StateMachine.CurrentState.LogicUpdate();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        #endregion

        #region Set Functions

        public void SetVelocityZero()
        {
            RB.velocity = Vector2.zero;
            CurrentVelocity = Vector2.zero;
        }

        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            workspace.Set(angle.x * velocity * direction, angle.y * velocity);
            RB.velocity = workspace;
            CurrentVelocity = workspace;
        }

        public void SetVelocity(float velocity, Vector2 direction)
        {
            workspace = direction * velocity;
            RB.velocity = workspace;
            CurrentVelocity = workspace;
        }

        public void SetVelocityX(float velocity)
        {
            workspace.Set(velocity, CurrentVelocity.y);
            RB.velocity = workspace;
            CurrentVelocity = workspace;
        }

        public void SetVelocityY(float velocity)
        {
            workspace.Set(CurrentVelocity.x, velocity);
            RB.velocity = workspace;
            CurrentVelocity = workspace;
        }

        #endregion

        #region Check Functions

        public bool CheckForCeiling()
        {
            return Physics2D.OverlapCircle(ceilingCheck.position, playerData.groundCheckRadius, playerData.groundLayer);
        }

        public bool CheckIfGrounded()
        {
            var groundLayerMask = playerData.groundLayer | playerData.ledgeLayer;
            return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, groundLayerMask);
        }

        public bool CheckIfOnLedge()
        {
            return Physics2D.OverlapCircle(groundCheck.position, playerData.groundCheckRadius, playerData.ledgeLayer);
        }

        public bool CheckIfTouchingWall()
        {
            return Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance,
                playerData.groundLayer);
        }

        public bool CheckIfTouchingLedge()
        {
            return Physics2D.Raycast(ledgeCheck.position, Vector2.right * FacingDirection, playerData.wallCheckDistance,
                playerData.groundLayer);
        }

        public bool CheckIfTouchingWallBack()
        {
            return Physics2D.Raycast(wallCheck.position, Vector2.right * -FacingDirection, playerData.wallCheckDistance,
                playerData.groundLayer);
        }

        public void CheckIfShouldFlip(int xInput)
        {
            if (xInput != 0 && xInput != FacingDirection)
            {
                Flip();
            }
        }

        public bool CheckIfCanGrabMovable()
        {
            if (!InputHandler.GrabInput)
            {
                GrabbedMovable?.SetDragged(false);
                GrabbedMovable = null;
                return false;
            }

            var hit = Physics2D.Raycast(ceilingCheck.position, Vector2.right * FacingDirection,
                playerData.blockCheckDistance, playerData.movablesLayer);
            if (!hit.collider) return false;
            GrabbedMovable = hit.collider?.GetComponent<Movable>();
            GrabbedMovable?.SetDragged(true);
            return true;
        }

        private float CheckWallHeight()
        {
            Vector2 checkStartPosition = wallHeightCheck.position;
            int totalRaycasts = 10;
            var deltaDistance = playerData.maxHorizontalJumpDistance / totalRaycasts;
            bool isPitBetween = false;
            for (int i = 2; i <= totalRaycasts; i++)
            {
                var raycastStartPosition = checkStartPosition;
                raycastStartPosition.x += (FacingDirection * i * deltaDistance);
                var hit = Physics2D.Raycast(raycastStartPosition, Vector2.down, playerData.maxWallHeightDistance,
                    playerData.groundLayer);
                if (hit.collider)
                {
                    Debug.DrawLine(raycastStartPosition, hit.point, Color.green, 2f);

                    if (feetPosition > hit.point.y)
                    {
                        //Platform is below feet height

                        isPitBetween = true;
                        jumpEndPosition = hit.point;
                    }
                    else
                    {
                        //platform is above feet height
                        isPitBetween = false;
                        return 0;
                    }
                }
                else
                {
                    isPitBetween = true;
                    Debug.Log("---------PIT AHEAD FOUND--------");
                    Debug.DrawLine(raycastStartPosition,
                        raycastStartPosition + Vector2.down * playerData.maxWallHeightDistance, Color.red, 2f);
                }
            }

            if (isPitBetween)
            {
                var playerHalfHeight = transform.position.y - feetPosition;
                //jumpEndPosition.y += playerHalfHeight;
                return (feetPosition - jumpEndPosition.y);
            }

            return 0;
        }

        private GameObject GetWallBelowPlayer()
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.down, 5f, playerData.groundLayer);
            if (hit.collider)
            {
                return hit.collider.gameObject;
            }

            return null;
        }

        public PlayerJumpType GetJumpType()
        {
            float jumpHeight = CheckWallHeight();

            Debug.Log("Jump height: " + jumpHeight);

            if (jumpHeight == 0 || Mathf.Abs(CurrentVelocity.x) < 0.1f)
            {
                jumpType = PlayerJumpType.NONE;
            }
            else if (jumpHeight < 0)
            {
                jumpType = PlayerJumpType.LEDGE;
            }
            else if (
                jumpHeight <= playerData.smallWallJumpDistance) // should be player feet level plus some vertical offset
            {
                jumpType = PlayerJumpType.SMALL;
            }
            else if (jumpHeight <= playerData.maxWallHeightDistance) // fix this @jai
            {
                jumpType = PlayerJumpType.MEDIUM;
            }
            else
            {
                jumpType = PlayerJumpType.LARGE;
            }

            return jumpType;
        }

        #endregion

        #region Other Functions

        public void SetColliderHeight(float height)
        {
            Vector2 center = MovementCollider.offset;
            workspace.Set(MovementCollider.size.x, height);

            center.y += (height - MovementCollider.size.y) / 2;

            MovementCollider.size = workspace;
            MovementCollider.offset = center;
        }

        public Vector2 DetermineCornerPosition()
        {
            RaycastHit2D xHit = Physics2D.Raycast(wallCheck.position, Vector2.right * FacingDirection,
                playerData.wallCheckDistance, playerData.groundLayer);
            float xDist = xHit.distance;
            workspace.Set((xDist + 0.015f) * FacingDirection, 0f);
            RaycastHit2D yHit = Physics2D.Raycast(ledgeCheck.position + (Vector3) (workspace), Vector2.down,
                ledgeCheck.position.y - wallCheck.position.y + 0.015f, playerData.groundLayer);
            float yDist = yHit.distance;

            workspace.Set(wallCheck.position.x + (xDist * FacingDirection), ledgeCheck.position.y - yDist);
            return workspace;
        }

        public void SetFriction(float friction)
        {
            playerCollider.sharedMaterial.friction = friction;
            if (playerCollider.enabled)
            {
                playerCollider.enabled = false;
                playerCollider.enabled = true;
            }
        }

        public void DisableCollider()
        {
            if (!playerCollider.enabled)
                return;
            playerCollider.enabled = false;
        }

        public void EnableCollider()
        {
            if (!playerCollider.enabled)
                playerCollider.enabled = true;
        }

        public void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();

        public void AnimtionFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();

        private void Flip()
        {
            FacingDirection *= -1;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);

            Gizmos.DrawLine(wallCheck.position,
                new Vector3(wallCheck.position.x + playerData.wallCheckDistance, wallCheck.position.y,
                    wallCheck.position.z));
        }

        public void TakeDamage(float damage)
        {
            if (isDead)
            {
                return;
            }

            health -= damage;
            if (health <= 0)
            {
                health = 0;
                Die();
            }
        }

        public void Die()
        {
        }

        public void Dispose()
        {
        }

        #endregion
    }
}