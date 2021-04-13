using System;
using Anvarat.Architecture;
using UnityEngine;

namespace Characters.Nick.States
{
    public class Jumping : State
    {
        private NickController _owner;
        
        private Vector2 _jumpForce;
        private Vector2 _horizontalForce;
        private Vector2 _velocity;
        private bool isLowHeightApplied;

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

            isLowHeightApplied = false;
            _horizontalForce = new Vector2();
            _velocity = new Vector2();
            _owner.pAnimationController.SetIsInTheAir(true);
            _owner.pAnimationController.SetWalkSpeed(Mathf.Abs(_owner.pRigidbody.velocity.x));
        }

        public override Type Tick()
        {
            if (!_owner.pGroundChecker.pIsGrounded)
            {
                _owner.pAnimationController.SetVerticalSpeed(_owner.pRigidbody.velocity.y);
                _owner.pRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (_owner.nickTraits.fallMultiplier - 1) * Time.deltaTime;
                
                //Control the character movement during jump
                if(_owner.pCurrentSpeed != 0)
                {
                    _horizontalForce.x = _owner.nickTraits.movementForceInAir * Mathf.Sign(_owner.pCurrentSpeed);
                    _horizontalForce.y = 0;
                    _owner.pRigidbody.AddForce(_horizontalForce);

                    if(Mathf.Abs(_owner.pRigidbody.velocity.x) > _owner.nickTraits.walkSpeed)
                    {
                        _velocity = _owner.pRigidbody.velocity;
                        _velocity.x = _owner.nickTraits.walkSpeed * Mathf.Sign(_owner.pCurrentSpeed);
                        _owner.pRigidbody.velocity = _velocity;
                    }

                    _owner.LookInDirection(_owner.pRigidbody.velocity.x);
                }
                else
                {
                    _velocity = _owner.pRigidbody.velocity;
                    _velocity.x = _owner.pRigidbody.velocity.x * _owner.nickTraits.airDragMultiplier;
                    _owner.pRigidbody.velocity = _velocity;
                }

                if (!isLowHeightApplied && !_owner.pIsHoldingJump)
                {
                    _velocity = _owner.pRigidbody.velocity;
                    _velocity.y = _owner.pRigidbody.velocity.y * _owner.nickTraits.variableJumpHeightMultiplier;
                    _owner.pRigidbody.velocity = _velocity;
                    isLowHeightApplied = true;
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
            _jumpForce.x = _owner.pRigidbody.velocity.x;
            _jumpForce.y = _owner.nickTraits.jumpForce;

            _owner.pRigidbody.velocity = _jumpForce;
        }
    }   
}