using System;
using System.Numerics;
using Anvarat.Architecture;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Characters.Nick.States
{
    public class Idle : State
    {
        private NickController _owner;
        private Vector2 _velocity;
        
        public Idle(NickController owner)
        {
            _owner = owner;
        }

        public override Type Tick()
        {
            if (!_owner.pGroundChecker.pIsGrounded)
            {
                return typeof(Jumping);
            }
            
            if (Mathf.Abs(_owner.pCurrentSpeed) > 0)
            {
                return typeof(Moving);
            }
            else
            {
                _velocity.x = 0;
                _velocity.y = _owner.pRigidbody.velocity.y;
                _owner.pRigidbody.velocity = _velocity; 
                return typeof(Idle);
            }
        }
    }   
}
