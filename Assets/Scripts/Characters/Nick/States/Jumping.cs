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

            _owner.pAnimationController.SetIsInTheAir(true);
            _owner.pAnimationController.SetWalkSpeed(Mathf.Abs(_owner.pRigidbody.velocity.x));
        }

        public override Type Tick()
        {
            if (!_owner.pGroundChecker.pIsGrounded)
            {
                _owner.pAnimationController.SetVerticalSpeed(_owner.pRigidbody.velocity.y);
                if(_owner.pRigidbody.velocity.y < 0){
                    _owner.pRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (_owner.nickTraits.fallMultiplier - 1) * Time.deltaTime;
                }else if(_owner.pRigidbody.velocity.y > 0)
                {
                    _owner.pRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (_owner.nickTraits.lowJumpMultiplier - 1) * Time.deltaTime;
                }
                return typeof(Jumping);
            }
            else
            {
                return typeof(Idle);
            }
        }

        public override void OnExit()
        {
            _owner.pAnimationController.SetVerticalSpeed(0);
            _owner.pAnimationController.SetIsInTheAir(false);
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