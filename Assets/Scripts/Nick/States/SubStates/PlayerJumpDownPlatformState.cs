using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using UnityEngine;

namespace ProjectN.Characters.Nick.States
{
    public class PlayerJumpDownPlatformState : PlayerAbilityState
    {
        private bool isFallingFromPlatform;
        private float disableTimer;

        public PlayerJumpDownPlatformState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {

        }

        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Enter()
        {
            base.Enter();
            isFallingFromPlatform = false;
            disableTimer = 0;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (!isExitingState) {
                if (isFallingFromPlatform) {
                    if (player.playerCollider.enabled)
                        player.DisableCollider();
                }

                if (!player.playerCollider.enabled) {
                    disableTimer += Time.deltaTime;
                }

                if(disableTimer >= playerData.ledgeFallColliderTime) {
                    player.EnableCollider();
                    stateMachine.ChangeState(player.InAirState);
                }
            }
        }

        public override void AnimationFinishTrigger()
        {
            base.AnimationFinishTrigger();
            isFallingFromPlatform = true;
            player.Anim.SetBool(animBoolName, false);
            player.Anim.SetBool("inAir", true);
        }
    }
}