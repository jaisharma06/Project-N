using System.Threading.Tasks;
using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using UnityEngine;

namespace ProjectN.Characters.Nick.States
{
    public class PlayerJumpState : PlayerAbilityState
    {
        private int amountOfJumpsLeft;
        
        #region JumpToPosition
        private Vector2 startPosition = Vector2.zero;
        private Vector2 endPosition = Vector2.zero;
        #endregion

        public PlayerJumpState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
            amountOfJumpsLeft = playerData.amountOfJumps;
        }

        public override void Enter()
        {
            base.Enter();

            PlayerJumpType playerJumpType = player.GetJumpType();
            
            Debug.Log("JumpType: " + playerJumpType);

            player.Anim.SetInteger("jumpType", ((int)playerJumpType));

            if (player.jumpType != PlayerJumpType.NONE)
            {
                var animationType = Random.Range(0, 2);
                player.Anim.SetInteger("smallJumpType", animationType);
            }

            switch (playerJumpType)
            {
                case PlayerJumpType.NONE:
                    break;
                case PlayerJumpType.SMALL:
                    
                    break;
                case PlayerJumpType.MEDIUM:
                    player.LandState.landingAnimationName = "MediumLanding";
                    break;
                case PlayerJumpType.LARGE:
                    player.LandState.landingAnimationName = "LargeLanding";
                    break;
                case PlayerJumpType.LEDGE:
                    player.LandState.landingAnimationName = "";
                    break;
            }
            
            player.InputHandler.UseJumpInput();
            if (player.jumpType == PlayerJumpType.NONE || player.jumpType == PlayerJumpType.LEDGE)
            {
                player.SetVelocityY(playerData.jumpVelocity);
            }
            else
            {
                    startPosition = player.transform.position;
                    endPosition = player.jumpEndPosition;
                    Vector3 velocity =  GetJumpVelocity(startPosition, endPosition, 4f);
                    player.SetVelocityX(velocity.x / 20.0f);
                    player.SetVelocityY(velocity.y * 2.5f);
                    Debug.Log("Velocity: " + velocity.magnitude + "Player Velocity: " + player.RB.velocity.magnitude);
            }
            
            isAbilityDone = true;
            amountOfJumpsLeft--;
            player.isGrabbingMovable = false;
            player.InAirState.SetIsJumping();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            Debug.Log("Player Velocity: " + player.RB.velocity.magnitude);
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

        private Vector3 GetJumpVelocity(Vector3 startPosition, Vector3 target, float height)
        {
            float displacementY = target.y - startPosition.y;
            Vector3 displacementXZ = new Vector3(target.x - startPosition.x, 0, target.z - startPosition.z);
            float time = Mathf.Sqrt(-2 * height / Physics2D.gravity.y) +
                         Mathf.Sqrt(2 * (displacementY - height) / Physics2D.gravity.y);
            Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * Physics2D.gravity.y * height);
            Vector3 velocityXZ = displacementXZ / time;
            Vector3 initialVelocity = velocityXZ + velocityY;
            DrawPath(time, initialVelocity);
            return initialVelocity;
        }

        private void DrawPath(float time, Vector3 initialVelocity)
        {
            Vector3 previousDrawPoint = player.transform.position;

            int resolution = 30;
            for (int i = 1; i <= resolution; i++)
            {
                float simulationTime = i / (float) resolution * time;
                Vector3 displacement = initialVelocity * simulationTime +
                                       Vector3.up * Physics2D.gravity.y * simulationTime * simulationTime / 2f;
                Vector3 drawPoint = player.transform.position + displacement;
                Debug.DrawLine(previousDrawPoint, drawPoint, Color.green, 2f);
                previousDrawPoint = drawPoint;
            }
        }

        public void ResetAmountOfJumpsLeft() => amountOfJumpsLeft = playerData.amountOfJumps;

        public void DecreaseAmountOfJumpsLeft() => amountOfJumpsLeft--;
    }
}
