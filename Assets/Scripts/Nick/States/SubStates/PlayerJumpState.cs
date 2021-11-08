using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using UnityEngine;

namespace ProjectN.Characters.Nick.States
{
    public class PlayerJumpState : PlayerAbilityState
    {
        private int amountOfJumpsLeft;

        public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
            amountOfJumpsLeft = playerData.amountOfJumps;
        }

        public override void Enter()
        {
            base.Enter();

            PlayerJumpType playerJumpType = PlayerJumpType.NONE;
            if(Mathf.Abs(player.CurrentVelocity.x) > 0.1f){
                playerJumpType = player.GetJumpType();
            }
            
            Debug.Log("JumpType: " + playerJumpType);

            player.Anim.SetInteger("jumpType", ((int)playerJumpType));

            switch (playerJumpType)
            {
                case PlayerJumpType.NONE:
                    break;
                case PlayerJumpType.SMALL:
                    var animationType = Random.Range(0, 2);
                    player.Anim.SetInteger("smallJumpType", animationType);
                    break;
                case PlayerJumpType.MEDIUM:
                    break;
                case PlayerJumpType.LARGE:
                    break;
                case PlayerJumpType.LEDGE:
                    break;
            }

            player.InputHandler.UseJumpInput();
            player.SetVelocityY(playerData.jumpVelocity);
            isAbilityDone = true;
            amountOfJumpsLeft--;
            player.isGrabbingMovable = false;
            player.InAirState.SetIsJumping();
        }

        public override void Exit()
        {
            base.Exit();
            player.Anim.SetInteger("jumpType", (int)(PlayerJumpType.NONE));
            player.Anim.SetInteger("smallJumpType", 0);
        }

        public bool CanJump()
        {
            if (amountOfJumpsLeft > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = playerData.amountOfJumps;

        public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;

        public void JumpToPosition(Vector2 position)
        {

        }
    }
}
