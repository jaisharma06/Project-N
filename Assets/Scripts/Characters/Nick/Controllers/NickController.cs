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
        public Direction pDodgeDirection { get; private set; }
        public float pDodgeCooldownTimer { get; set; }
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
                { typeof(Dodging), new Dodging(this)}
            };

            var parallelState = new ParallelState();
            parallelState.SetContext(() =>
            {
                if (pDodgeCooldownTimer > 0)
                {
                    pDodgeCooldownTimer -= Time.deltaTime;
                }
            });
            
            _stateMachine.SetParallelState(parallelState);
            _stateMachine.SetStates(states);
        }

        private void SetupPlayerInput()
        {
            controls.Nick.Movement.performed += ctx => UpdateSpeed(ctx.ReadValue<float>());
            controls.Nick.Movement.canceled += _ => UpdateSpeed(0);
            controls.Nick.Jumping.performed += _ => Jump();
            controls.Nick.Dodge.performed += _ => Dodge();
            controls.Nick.Attack.performed += _ => pAttackHandler.DamageEnemy(); 
        }

        #region INPUT_HANDLERS

        private void UpdateSpeed(float speed)
        {
#if UNITY_EDITOR
            Debug.Log($"Player Speed: {speed}");
#endif
            pCurrentSpeed = speed;
        }

        private void Jump()
        {
#if UNITY_EDITOR
            Debug.Log($"Jumping");
#endif
            if (pGroundChecker.pIsGrounded)
            {
                pIsJumping = true;
            }
        }

        private void Dodge()
        {
#if UNITY_EDITOR
            Debug.Log($"Dodging");
#endif
            if (pGroundChecker.pIsGrounded && !pIsDodging && pDodgeCooldownTimer <= 0)
            {
                pIsDodging = true;
                pDodgeDirection = (pCurrentSpeed > 0) ? Direction.RIGHT : Direction.LEFT;
            }
        }

        #endregion

        public void LookInDirection(Direction direction)
        {
            var localScale = transform.localScale;
            localScale.x = (int)direction * Mathf.Abs(localScale.x);
            transform.localScale = localScale;
        }
    }
}