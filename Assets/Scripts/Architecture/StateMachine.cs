using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Anvarat.Architecture
{
    public class StateMachine : MonoBehaviour
    {
        private Dictionary<Type, State> _availableStates;
        public State pCurrentState { get; private set; }
        public event Action<State> onStateChangedEvent;

        private State _parallelState;

        public void SetStates(Dictionary<Type, State> states)
        {
            _availableStates = states;
        }

        private void Update()
        {
            pCurrentState ??= _availableStates.Values.First();

            var nextState = pCurrentState?.Tick();

            if (nextState != null && nextState != pCurrentState.GetType())
            {
                SwitchToNewState(nextState);
            }

            _parallelState?.Tick();
        }

        private void SwitchToNewState(Type nextState)
        {
            pCurrentState?.OnExit();
            pCurrentState = _availableStates[nextState];
            pCurrentState?.OnEnter();
            onStateChangedEvent?.Invoke(pCurrentState);
        }

        public void SetParallelState(State parallelState)
        {
            _parallelState = parallelState;
        }
    }
}
