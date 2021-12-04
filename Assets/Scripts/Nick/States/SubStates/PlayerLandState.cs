using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;

namespace ProjectN.Characters.Nick.States
{
    public class PlayerLandState : PlayerGroundedState
    {
        public string landingAnimationName { get; set; }
        public PlayerLandState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            if (string.IsNullOrEmpty(landingAnimationName))
            {
                SwitchToLocomotionState();
            }
            else
            {
                 player.Anim.SetBool(landingAnimationName, true);
            }
        }

        public override void Exit()
        {
            base.Exit();
            if (!string.IsNullOrEmpty(landingAnimationName))
            {
                player.Anim.SetBool(landingAnimationName, false);
            } 
            landingAnimationName = string.Empty;
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            SwitchToLocomotionState();
        }

        internal void SwitchToLocomotionState()
        {
            if (!isExitingState)
            {
                if (xInput != 0)
                {
                    stateMachine.ChangeState(player.MoveState);
                }
                else
                {
                    stateMachine.ChangeState(player.IdleState);
                }
            }
        }
    }
}