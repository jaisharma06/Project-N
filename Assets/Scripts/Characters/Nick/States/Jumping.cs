using System;
using Anvarat.Architecture;
using UnityEngine;

namespace Characters.Nick.States
{
    public class Jumping : State
    {
        private NickController _owner;
        
        private Vector2 _jumpForce;

        public Jumping(NickController owner)
        {
            _owner = owner;
        }

        public override void OnEnter()
        {
            if (_owner.pIsJumping)
            {
                _owner.pIsJumping = false;
                AddJumpForce();
            }
        }

        public override Type Tick()
        {
            if (!_owner.pGroundChecker.pIsGrounded)
            {
                return typeof(Jumping);
            }
            else
            {
                return typeof(Idle);
            }
        }

        public override void OnExit()
        {
            var fallingVelocity = _owner.pRigidbody.velocity.y;
            Debug.Log($"falling velocity {fallingVelocity}");
        }

        private void AddJumpForce()
        {
            _jumpForce.x = _owner.pCurrentSpeed;
            _jumpForce.y = _owner.nickTraits.jumpForce;
            
            _owner.pRigidbody.velocity = Vector2.zero;
            _owner.pRigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }   
}