using System.Diagnostics;
using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;

namespace ProjectN.Characters.Nick.States
{
    public class PlayerIdleState : PlayerGroundedState
    {
        public PlayerIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
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

            if (!isExitingState)
            {
                UnityEngine.Debug.Log(isGrabbingMovable);
                if (isGrabbingMovable){
                    if (xInput != 0){
                        stateMachine.ChangeState(player.PushingMoveState);
                    }else{
                        stateMachine.ChangeState(player.PushingIdleState);
                    }
                }

                if (xInput != 0){ 
                    stateMachine.ChangeState(player.MoveState);
                }else if (yInput == -1){
                    //stateMachine.ChangeState(player.CrouchIdleState);
                }
            }

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
