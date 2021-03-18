using System;
using System.Collections.Generic;
using Anvarat.Architecture;
using Characters.Enemies.Common.State;
using UnityEngine;

namespace Characters.Enemies.Common
{
    public class EnemyController : MonoBehaviour
    {
        public EnemyTraits traits;
        private StateMachine _stateMachine;

        private void Awake()
        {
            InitializeStateMachine();
        }

        private void InitializeStateMachine()
        {
            _stateMachine = GetComponent<StateMachine>();
            var states = new Dictionary<Type, Anvarat.Architecture.State>()
            {
                { typeof(Idle), new Idle(this)}
            };

            var parallelState = new ParallelState();
            parallelState.SetContext(() =>
            {
                
            });
            
            _stateMachine.SetParallelState(parallelState);
            _stateMachine.SetStates(states);
        }
    }   
}
