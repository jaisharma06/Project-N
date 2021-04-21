using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectN.Characters.Nick.States
{
    public class PlayerWallSlideState : PlayerTouchingWallState
    {
        private float startSlidingTimer;
        public PlayerWallSlideState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void Enter()
        {
            base.Enter();
            startSlidingTimer = 0;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                if(startSlidingTimer < playerData.slideStartTime)
                {
                    startSlidingTimer += Time.deltaTime;
                    player.SetVelocityY(1);
                }
                else
                {
                    player.SetVelocityY(-playerData.wallSlideVelocity);
                }

                if (grabInput && yInput == 0)
                {
                    stateMachine.ChangeState(player.WallGrabState);
                }
            }

        }
    }
}
