using System;
using System.Collections.Generic;
using Anvarat.Architecture;
using Characters.Nick.Settings;
using Characters.Nick.States;
using UnityEngine;

namespace Characters.Nick
{
    [RequireComponent(typeof(StateMachine))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class NickController : MonoBehaviour
    {
        #region SERIALIZED_FIELDS
        public Traits nickTraits;
        public InputMaster controls;
        #endregion
        
        #region PROPERTIES
        public float pCurrentSpeed { get; set; }

        private StateMachine _stateMachine;
        public Collider2D pCollider { get; private set; }
        public Rigidbody2D pRigidbody { get; private set; }
        public GroundChecker pGroundChecker { get; private set; }
        public NickHealthController pHealthController { get; private set; }
        public AttackHandler pAttackHandler { get; private set; }
        public AnimationController pAnimationController { get; private set; }
        public EdgeDetector pEdgeDetector { get; private set; }
        public bool pIsJumping { get; set; }
        public bool pIsDodging { get; set; }
        public bool pIsWallSliding { get; set; }
        public bool pIsTouchingWall { get => pGroundChecker.pIsTouchingWall; }
        public bool pIsGrounded { get => pGroundChecker.pIsGrounded; }
        public bool pCanJump { get => CheckIfCanJump();  }
        public MovementDirection pFacingDirection { get; private set; } = MovementDirection.RIGHT;
        public MovementDirection pDodgeDirection { get; private set; }
        public int pAmountOfJumpsLeft { get; private set; } = 1;
        public float pDodgeCooldownTimer { get; set; }
        public bool pIsHoldingJump { get; private set; }
        #endregion

        #region UNITY_BUILT_IN_METHODS
        private void Awake()
        {
            pRigidbody = GetComponent<Rigidbody2D>();
            pCollider = GetComponent<Collider2D>();
            controls = new InputMaster();
            pGroundChecker = GetComponent<GroundChecker>();
            pHealthController = GetComponent<NickHealthController>();
            pAttackHandler = GetComponent<AttackHandler>();
            pAnimationController = GetComponent<AnimationController>();
            pEdgeDetector = GetComponentInChildren<EdgeDetector>();

            nickTraits.wallHopDirection.Normalize();
            nickTraits.wallJumpDirection.Normalize();

            pHealthController.SetMaxHealth(nickTraits.maxHealth);
            InitializeStateMachine();
            SetupPlayerInput();
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }
        #endregion

        private void InitializeStateMachine()
        {
            _stateMachine = GetComponent<StateMachine>();
            var states = new Dictionary<Type, State>()
            {
                { typeof(Idle), new Idle(this)},
                { typeof(Moving), new Moving(this)},
                { typeof(Jumping), new Jumping(this)},
                { typeof(Dodging), new Dodging(this)},
                { typeof(WallSliding), new WallSliding(this)}
            };

            var parallelState = new ParallelState();
            parallelState.SetContext(() =>
            {
                if (pDodgeCooldownTimer > 0)
                {
                    pDodgeCooldownTimer -= Time.deltaTime;
                }

                CheckIfWallSliding();
            });
            
            _stateMachine.SetParallelState(parallelState);
            _stateMachine.SetStates(states);
        }

        private void SetupPlayerInput()
        {
            controls.Nick.Movement.performed += ctx => UpdateSpeed(ctx.ReadValue<float>());
            controls.Nick.Movement.canceled += _ => UpdateSpeed(0);
            controls.Nick.Jumping.performed += _ => Jump();
            controls.Nick.Jumping.canceled += _ => JumpKeyReleased();
            controls.Nick.Dodge.performed += _ => Dodge();
            controls.Nick.Attack.performed += _ => pAttackHandler.DamageEnemy(); 
        }

        #region INPUT_HANDLERS

        private void UpdateSpeed(float speed)
        {
            pCurrentSpeed = speed;
        }

        private void Jump()
        {
            if (pCanJump)
            {
                pIsJumping = true;
                pIsHoldingJump = true;
                pAmountOfJumpsLeft--;
            }
            else if(pIsWallSliding && pCurrentSpeed == 0 && pCanJump)//Wall Hop
            {
                pIsWallSliding = false;
                pAmountOfJumpsLeft--;
            }
            else if((pIsWallSliding || pIsTouchingWall) && pCurrentSpeed != 0 && pCanJump)
            {
                pIsWallSliding = false;
                pAmountOfJumpsLeft--;
            }
        }

        private void JumpKeyReleased()
        {
            pIsHoldingJump = false;
        }

        private void Dodge()
        {
            if (pGroundChecker.pIsGrounded && !pIsDodging && pDodgeCooldownTimer <= 0)
            {
                pIsDodging = true;
                pDodgeDirection = (pCurrentSpeed > 0) ? MovementDirection.RIGHT : MovementDirection.LEFT;
            }
        }

        #endregion

        public void LookInDirection(MovementDirection direction)
        {
            var localScale = transform.localScale;
            localScale.x = (int)direction * Mathf.Abs(localScale.x);
            transform.localScale = localScale;
            pFacingDirection = direction;
        }

        public void LookInDirection(float speed)
        {
            var direction = (MovementDirection)Mathf.Sign(speed);
            LookInDirection(direction);
        }

        private bool CheckIfCanJump()
        {
            if((pGroundChecker.pIsGrounded && pRigidbody.velocity.y <= 0) || pIsWallSliding)
            {
                pAmountOfJumpsLeft = nickTraits.amountOfJumps;
            }

            if(pAmountOfJumpsLeft <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void CheckIfWallSliding()
        {
            if (pIsTouchingWall && !pIsGrounded && pRigidbody.velocity.y < 0)
            {
                pIsWallSliding = true;
            }
            else
            {
                pIsWallSliding = false;
            }
        }
    }
}