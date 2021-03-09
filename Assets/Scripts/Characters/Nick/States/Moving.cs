using System;
using Anvarat.Architecture;
using UnityEngine;

namespace Characters.Nick.States
{
    public class Moving : State
    {
        private NickController _owner;
        private Vector2 _velocity;

        public Moving(NickController owner)
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
                _velocity.x = _owner.pCurrentSpeed * _owner.nickTraits.walkSpeed;
                _velocity.y = _owner.pRigidbody.velocity.y;
                _owner.pRigidbody.velocity = _velocity;
                return typeof(Moving);
            }
            else
            {
                return typeof(Idle);
            }
        }
    }   
}