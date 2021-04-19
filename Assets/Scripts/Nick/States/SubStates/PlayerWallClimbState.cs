using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using ProjectN.Characters.Nick.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectN.Characters.Nick
{
    public class PlayerWallClimbState : PlayerTouchingWallState
    {
        public PlayerWallClimbState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isExitingState)
            {
                player.SetVelocityY(playerData.wallClimbVelocity);

                if (yInput != 1)
                {
                    stateMachine.ChangeState(player.WallGrabState);
                }
            }


        }
    }
}