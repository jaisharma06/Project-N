using System.Collections;
using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using UnityEngine;

namespace ProjectN.Characters.Nick.States
{
    public class PlayerPushingMoveState : PlayerGroundedState
    {
        public PlayerPushingMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.CheckIfShouldFlip(xInput);

            player.SetVelocityX(playerData.movementVelocity * xInput);

            if (!isExitingState) {

                if (!isGrabbingMovable) {
                    if (xInput != 0) {
                        stateMachine.ChangeState(player.MoveState);
                    } else {
                        stateMachine.ChangeState(player.IdleState);
                    }
                }

                if (xInput == 0) {
                    stateMachine.ChangeState(player.PushingIdleState);
                }
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}