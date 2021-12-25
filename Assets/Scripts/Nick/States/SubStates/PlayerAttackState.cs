using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using ProjectN.Characters.Nick.Weapons;
using UnityEngine;

namespace ProjectN.Characters.Nick.States
{
    public class PlayerAttackState : PlayerAbilityState
    {
        private Weapon weapon;

        private int xInput;

        private float velocityToSet;

        private bool setVelocity;
        private bool shouldCheckFlip;

        public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            setVelocity = false;
            dontSwitchToAnotherState = false;

            weapon.EnterWeapon();
        }

        public override void Exit()
        {
            base.Exit();

            weapon.ExitWeapon();

            if (player.InputHandler.LastPrimaryAttackInput)
            {
                dontSwitchToAnotherState = true;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            xInput = player.InputHandler.NormInputX;

            if (shouldCheckFlip)
            {
                player.CheckIfShouldFlip(xInput);
            }


            if (setVelocity)
            {
                player.SetVelocityX(velocityToSet * player.FacingDirection);
            }
        }

        public void SetWeapon(Weapon weapon)
        {
            this.weapon = weapon;
            this.weapon.InitializeWeapon(this);
        }

        public void SetPlayerVelocity(float velocity)
        {
            player.SetVelocityX(velocity * player.FacingDirection);

            velocityToSet = velocity;
            setVelocity = true;
        }

        public void SetFlipCheck(bool value)
        {
            shouldCheckFlip = value;
        }

        #region Animation Triggers

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            isAbilityDone = true;
            if (player.InputHandler.PrimaryAttackInputBuffer.Count > 0)
                player.InputHandler.PrimaryAttackInputBuffer.RemoveAt(0);
            if (!player.InputHandler.LastPrimaryAttackInput)
            {
                weapon.attackCounter = 0;
            }
        }

        #endregion
    }
}
