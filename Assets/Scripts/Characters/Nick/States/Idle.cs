using System;
using Anvarat.Architecture;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Characters.Nick.States
{
    public class Idle : State
    {
        private NickController _owner;
        private Vector2 _velocity;
        private float angle;
        
        public Idle(NickController owner)
        {
            _owner = owner;
        }

        public override Type Tick()
        {
            if (!_owner.pGroundChecker.pIsGrounded || _owner.pIsJumping)
            {
                return typeof(Jumping);
            }
            
            if (_owner.pIsDodging)
            {
                return typeof(Dodging);
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
                GetVerticalVelocity();
                return typeof(Idle);
            }
        }

        private void GetVerticalVelocity()
        {
           angle = Vector2.Angle(Vector2.right, _owner.pGroundChecker.pGroundNormal);
        }
    }   
}
