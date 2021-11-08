using System.Collections;
using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using ProjectN.Characters.Nick.States;
using UnityEngine;

namespace Assets.Scripts.Nick.States.SubStates
{
    public class PlayerPushBlockIdleState : PlayerGroundedState
    {
        public PlayerPushBlockIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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
            if (player.GrabbedMovable != null) {
                player.transform.position = player.GrabbedMovable.GetPlayerPosition(player.transform.position);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.Anim.SetFloat("xVelocity", 0);

            player.GrabbedMovable?.SetVelocity(0);
            if (!isExitingState) {
                if (!isGrabbingMovable) {
                    stateMachine.ChangeState(player.LeaveBlockState);
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