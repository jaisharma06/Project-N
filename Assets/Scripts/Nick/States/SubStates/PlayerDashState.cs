using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using ProjectN.Characters.Nick.Utils;
using UnityEngine;

namespace ProjectN.Characters.Nick.States
{
    public class PlayerDashState : PlayerAbilityState
    {
        public bool CanDash { get; private set; }
        private bool isHolding;
        private bool dashInputStop;

        private float lastDashTime;
        private float dashSlideStartTime;
        private bool isDashComplete;

        private Vector2 dashDirection;
        private Vector2 dashDirectionInput;
        private Vector2 lastAIPos;

        public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }
        public override void Enter()
        {
            base.Enter();

            CanDash = false;
            player.InputHandler.UseDashInput();

            isHolding = true;
            dashDirection = Vector2.right * player.FacingDirection;

            Time.timeScale = playerData.holdTimeScale;
            startTime = Time.unscaledTime;
            isDashComplete = false;
            //player.DashDirectionIndicator.gameObject.SetActive(true);

        }

        public override void Exit()
        {
            base.Exit();

            if (player.CurrentVelocity.y > 0)
            {
                player.SetVelocityY(player.CurrentVelocity.y * playerData.dashEndYMultiplier);
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {

                player.Anim.SetFloat("yVelocity", player.CurrentVelocity.y);
                player.Anim.SetFloat("xVelocity", Mathf.Abs(player.CurrentVelocity.x));


                if (isHolding)
                {
                    dashDirectionInput = player.InputHandler.DashDirectionInput;
                    dashInputStop = player.InputHandler.DashInputStop;

                    if (dashDirectionInput != Vector2.zero)
                    {
                        dashDirection = dashDirectionInput;
                        dashDirection.Normalize();
                    }

                    float angle = Vector2.SignedAngle(Vector2.right, dashDirection);

                    if (dashInputStop || Time.unscaledTime >= startTime + playerData.maxHoldTime)
                    {
                        isHolding = false;
                        Time.timeScale = 1f;
                        startTime = Time.time;
                        player.CheckIfShouldFlip(Mathf.RoundToInt(dashDirection.x));
                        player.RB.drag = playerData.drag;
                        player.SetVelocity(playerData.dashVelocity, dashDirection);
                        PlaceAfterImage();
                    }
                }
                else
                {
                    player.SetVelocity(playerData.dashVelocity, dashDirection);
                    CheckIfShouldPlaceAfterImage();

                    if (Time.time >= startTime + playerData.dashTime)
                    {
                        player.RB.drag = playerData.defaultDrag;
                        if (!isDashComplete)
                        {
                            isDashComplete = true;
                            dashSlideStartTime = Time.time;
                            player.Anim.SetBool(animBoolName, false);
                            player.Anim.SetBool("dashSlide", true);
                        }
                    }

                    if (isDashComplete)
                    {
                        if (!player.CheckIfGrounded())
                        {
                            player.Anim.SetBool("dashSlide", false);
                            isAbilityDone = true;
                            lastDashTime = Time.time;
                            return;
                        }

                        player.SetVelocityX(dashDirection.x * playerData.dashSlideSpeed);

                        if (Time.time > dashSlideStartTime + playerData.dashSlideTime)
                        {
                            player.Anim.SetBool("dashSlide", false);
                            isAbilityDone = true;
                            lastDashTime = Time.time;
                        }
                    }
                }
            }
        }

        private void CheckIfShouldPlaceAfterImage()
        {
            if (Vector2.Distance(player.transform.position, lastAIPos) >= playerData.distBetweenAfterImages)
            {
                PlaceAfterImage();
            }
        }

        private void PlaceAfterImage()
        {
            PlayerAfterImagePool.Instance.GetFromPool();
            lastAIPos = player.transform.position;
        }

        public bool CheckIfCanDash()
        {
            return CanDash && Time.time >= lastDashTime + playerData.dashCooldown;
        }

        public void ResetCanDash() => CanDash = true;
        public void ResetLastDashTime() => lastDashTime = Time.time - playerData.dashCooldown;
    }

}
