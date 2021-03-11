using System;
using Anvarat.Architecture;
using UnityEngine;

namespace Characters.Nick.States
{
    public class Dodging : State
    {
        private NickController _owner;
        private float dodgeTimer;
        private int direction;

        public Dodging(NickController owner)
        {
            _owner = owner;
        }

        public override void OnEnter()
        {
            dodgeTimer = 0;
            _owner.pIsDodging = false;
            direction = (int)_owner.pDodgeDirection;
        }

        public override Type Tick()
        {
            dodgeTimer += Time.deltaTime;
            if (dodgeTimer >= _owner.nickTraits.dodgeTime)
            {
                return typeof(Idle);
            }
            SetVelocity();
            return typeof(Dodging);
        }
        
        private void SetVelocity()
        {
            _owner.pRigidbody.velocity = Vector2.right * direction * _owner.nickTraits.dodgeSpeed;
        }

        public override void OnExit()
        {
            _owner.pDodgeCooldownTimer = _owner.nickTraits.dodgeCooldownTime;
        }
    }
}
