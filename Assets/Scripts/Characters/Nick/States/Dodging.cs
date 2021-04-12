using System;
using Anvarat.Architecture;
using UnityEngine;

namespace Characters.Nick.States
{
    public class Dodging : State
    {
        private NickController _owner;
        private float dodgeTimer;
        private float cooldownAfterDodgeTimer;
        private int direction;
        private bool isSliding;

        public Dodging(NickController owner)
        {
            _owner = owner;
        }

        public override void OnEnter()
        {
            dodgeTimer = 0;
            cooldownAfterDodgeTimer = 0f;
            _owner.pIsDodging = false;
            isSliding = false;
            direction = (int)_owner.pDodgeDirection;
            _owner.pAnimationController.SetIsDashing(true);
            _owner.LookInDirection((Direction)direction);
        }

        public override Type Tick()
        {
            dodgeTimer += Time.deltaTime;
            if (cooldownAfterDodgeTimer >= _owner.nickTraits.moveCooldownTimeAfterDodge)
            {
                return typeof(Idle);
            }
            else if(dodgeTimer >= _owner.nickTraits.dodgeTime)
            {
                cooldownAfterDodgeTimer += Time.deltaTime;
                if (!isSliding)
                {
                    isSliding = true;
                    _owner.pAnimationController.SetIsDashing(false);
                    _owner.pAnimationController.SetSliding(true);
                }
                SetSlidingVelocity();
            }
            else
            {
                SetVelocity();
            }
            return typeof(Dodging);
        }
        
        private void SetVelocity()
        {
            _owner.pRigidbody.velocity = Vector2.right * direction * _owner.nickTraits.dodgeSpeed;
        }

        private void SetSlidingVelocity()
        {
            _owner.pRigidbody.velocity = Vector2.right * direction * _owner.nickTraits.slidingVelocity;
        }

        public override void OnExit()
        {
            _owner.pDodgeCooldownTimer = _owner.nickTraits.dodgeCooldownTime;
            _owner.pAnimationController.SetSliding(false);
        }
    }
}
