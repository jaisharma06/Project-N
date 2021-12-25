using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectN.Characters.Nick.States
{
    public class PlayerAbilityState : PlayerState
    {
        protected bool isAbilityDone;
        protected bool dontSwitchToAnotherState;

        private bool isGrounded;

        public PlayerAbilityState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();

            isGrounded = player.CheckIfGrounded();
        }

        public override void Enter()
        {
            base.Enter();

            isAbilityDone = false;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (isAbilityDone)
            {
                if (dontSwitchToAnotherState)
                {
                    stateMachine.ChangeState(stateMachine.CurrentState);
                }
                else
                {
                    if (isGrounded && player.CurrentVelocity.y < 0.01f)
                    {
                        stateMachine.ChangeState(player.IdleState);
                    }
                    else
                    {
                        stateMachine.ChangeState(player.InAirState);
                    }
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
