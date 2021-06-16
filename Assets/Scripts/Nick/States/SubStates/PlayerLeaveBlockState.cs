using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using ProjectN.Characters.Nick.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerLeaveBlockState : PlayerGroundedState
{
    private bool isAnimationComplete;
    public PlayerLeaveBlockState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        isAnimationComplete = false;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        isAnimationComplete = true;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!isExitingState && isAnimationComplete) {
            if (xInput != 0) {
                stateMachine.ChangeState(player.MoveState);
            } else {
                stateMachine.ChangeState(player.IdleState);
            }
        }
    }
}
