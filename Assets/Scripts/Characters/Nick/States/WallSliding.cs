using Anvarat.Architecture;
using System;
using UnityEngine;

namespace Characters.Nick.States
{
    public class WallSliding : State
    {
        private NickController _owner;
        private Vector2 _wallSlideVelocity = new Vector2();

        private float _wallSlideTimer;
        public WallSliding(NickController owner)
        {
            _owner = owner;
        }

        public override void OnEnter()
        {
            _owner.pAnimationController.SetWallSliding(true);
            _wallSlideTimer = _owner.nickTraits.wallSlideStartTime;
        }
        public override Type Tick()
        {
            if (_owner.pIsWallSliding)
            {
                if (_wallSlideTimer > 0)
                {
                    _wallSlideTimer -= Time.deltaTime;
                    _wallSlideVelocity.x = _owner.pRigidbody.velocity.x;
                    _wallSlideVelocity.y = -0.001f;
                }
                else
                {
                    _wallSlideVelocity.x = _owner.pRigidbody.velocity.x;
                    _wallSlideVelocity.y = -_owner.nickTraits.wallSlideSpeed;
                }

                if (_owner.pRigidbody.velocity.y < -_owner.nickTraits.wallSlideSpeed)
                {
                    _wallSlideVelocity.x = _owner.pRigidbody.velocity.x;
                    _wallSlideVelocity.y = -_owner.nickTraits.wallSlideSpeed;
                }
                _owner.pRigidbody.velocity = _wallSlideVelocity;
                return typeof(WallSliding);
            }
            else
            {
                return typeof(Jumping);
            }
        }

        public override void OnExit()
        {
            _owner.pAnimationController.SetWallSliding(false);
        }
    }
}