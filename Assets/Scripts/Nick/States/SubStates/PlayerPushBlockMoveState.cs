using System.Collections;
using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using UnityEngine;

namespace ProjectN.Characters.Nick.States
{
    public class PlayerPushBlockMoveState : PlayerGroundedState
    {
        private float velocity;
        public PlayerPushBlockMoveState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            player.transform.position = player.GrabbedMovable.GetPlayerPosition(player.transform.position);
        }

        public override void Exit()
        {
            base.Exit();
            player.Anim.SetFloat("xVelocity", 0);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            //player.CheckIfShouldFlip(xInput);

            if (Mathf.Sign(xInput) == Mathf.Sign(player.FacingDirection)){
                velocity = playerData.pushVelocity * xInput;
            }else{
                velocity = playerData.pullVelocity * xInput;
            }

            player.Anim.SetFloat("xVelocity", player.FacingDirection * player.CurrentVelocity.x);

            player.SetVelocityX(velocity);
            player.GrabbedMovable?.SetVelocity(velocity);

            if (!isExitingState) {

                if (!isGrabbingMovable) {
                    stateMachine.ChangeState(player.LeaveBlockState);
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