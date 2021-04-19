using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;

namespace ProjectN.Characters.Nick.States
{
    public class PlayerCrouchIdleState : PlayerGroundedState
    {
        public PlayerCrouchIdleState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.SetVelocityZero();
            player.SetColliderHeight(playerData.crouchColliderHeight);
        }

        public override void Exit()
        {
            base.Exit();
            player.SetColliderHeight(playerData.standColliderHeight);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if (xInput != 0)
                {
                    stateMachine.ChangeState(player.CrouchMoveState);
                }
                else if (yInput != -1 && !isTouchingCeiling)
                {
                    stateMachine.ChangeState(player.IdleState);
                }
            }
        }
    }
}
