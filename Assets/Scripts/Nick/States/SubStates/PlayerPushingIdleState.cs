using System.Collections;
using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using ProjectN.Characters.Nick.States;
using UnityEngine;

namespace Assets.Scripts.Nick.States.SubStates
{
    public class PlayerPushingIdleState : PlayerGroundedState
    {
        public PlayerPushingIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            player.SetVelocityX(0f);
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState) {
                if (!isGrabbingMovable) {
                    if (xInput != 0) {
                        stateMachine.ChangeState(player.MoveState);
                    } else {
                        stateMachine.ChangeState(player.IdleState);
                    }
                }

                if (xInput != 0) {
                    stateMachine.ChangeState(player.PushingMoveState);
                }
            }

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}