using System;
using Anvarat.Architecture;
using UnityEngine;

namespace Characters.Nick.States
{
    public class Jumping : State
    {
        private NickController _owner;

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

        private void AddJumpForce()
        {
            _owner.pRigidbody.AddForce(Vector2.up * _owner.nickTraits.jumpForce, ForceMode2D.Impulse);
        }
    }   
}