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
        public Traits nickTraits;
        public InputMaster controls;
        
        public float pCurrentSpeed { get; set; }

        private StateMachine _stateMachine;
        public Collider2D pCollider { get; private set; }
        public Rigidbody2D pRigidbody { get; private set; }
        public GroundChecker pGroundChecker { get; private set; }

        private void Awake()
        {
            pRigidbody = GetComponent<Rigidbody2D>();
            pCollider = GetComponent<Collider2D>();
            controls = new InputMaster();
            pGroundChecker = GetComponent<GroundChecker>();
            
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

        private void InitializeStateMachine()
        {
            _stateMachine = GetComponent<StateMachine>();
            var states = new Dictionary<Type, State>()
            {
                { typeof(Idle), new Idle(this)},
                { typeof(Moving), new Moving(this)},
                { typeof(Jumping), new Jumping(this)}
            };
            
            _stateMachine.SetStates(states);
        }

        private void SetupPlayerInput()
        {
            controls.Nick.Movement.performed += ctx => UpdateSpeed(ctx.ReadValue<float>());
            controls.Nick.Movement.canceled += ctx => UpdateSpeed(0);
            controls.Nick.Jumping.performed += _ => Jump();
            controls.Nick.Dodge.performed += _ => Dodge();
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
        }

        private void Dodge()
        {
#if UNITY_EDITOR
            Debug.Log($"Dodging");
#endif
        }

        #endregion
    }
}