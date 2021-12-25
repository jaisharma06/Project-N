using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;

namespace ProjectN.Characters.Nick.States
{
    public class PlayerGroundedState : PlayerState
    {
        protected int xInput;
        protected int yInput;

        protected bool isTouchingCeiling;

        private bool JumpInput;
        private bool grabInput;
        private bool isGrounded;
        private bool isTouchingWall;
        private bool isTouchingLedge;
        private bool dashInput;
        protected bool isGrabbingMovable;

        public PlayerGroundedState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void DoChecks()
        {
            base.DoChecks();

            isGrounded = player.CheckIfGrounded();
            isTouchingWall = player.CheckIfTouchingWall();
            isTouchingLedge = player.CheckIfTouchingLedge();
            isTouchingCeiling = player.CheckForCeiling();
            isGrabbingMovable = player.CheckIfCanGrabMovable();
        }

        public override void Enter()
        {
            base.Enter();

            player.JumpState.ResetAmountOfJumpsLeft();
            player.DashState.ResetCanDash();

            player.SetFriction(1f);
        }

        public override void Exit()
        {
            base.Exit();
            player.SetFriction(0f);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            xInput = player.InputHandler.NormInputX;
            yInput = player.InputHandler.NormInputY;
            JumpInput = player.InputHandler.JumpInput;
            grabInput = player.InputHandler.GrabInput;
            dashInput = player.InputHandler.DashInput;

            if (player.InputHandler.LastPrimaryAttackInput && !isTouchingCeiling)
            {
                stateMachine.ChangeState(player.PrimaryAttackState);
            }
            // else if (player.InputHandler.AttackInputs[(int)CombatInputs.secondary] && !isTouchingCeiling)
            // {
            //     //stateMachine.ChangeState(player.SecondaryAttackState);
            // }
            else if (JumpInput && player.JumpState.CanJump())
            {
                if (yInput < 0 && player.CheckIfOnLedge())
                {
                    //Change state to jump down platform state
                    stateMachine.ChangeState(player.JumpDownPlatformState);
                }
                else
                {
                    if (player.playerCollider.enabled)
                        stateMachine.ChangeState(player.JumpState);
                }
            }
            else if (!isGrounded)
            {
                player.InAirState.StartCoyoteTime();
                stateMachine.ChangeState(player.InAirState);
            }
            else if (isTouchingWall && grabInput && isTouchingLedge)
            {
                stateMachine.ChangeState(player.WallGrabState);
            }
            else if (dashInput && player.DashState.CheckIfCanDash() && !isTouchingCeiling && !player.isGrabbingMovable)
            {
                stateMachine.ChangeState(player.DashState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
