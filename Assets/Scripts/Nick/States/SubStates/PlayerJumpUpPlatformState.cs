using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using ProjectN.Characters.Nick.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectN.Characters.Nick.States
{
    public class PlayerJumpUpPlatformState : PlayerGroundedState
    {
        public PlayerJumpUpPlatformState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
        {

        }

        public override void DoChecks()
        {
            base.DoChecks();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
    }
}
