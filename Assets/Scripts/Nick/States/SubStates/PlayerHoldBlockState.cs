using ProjectN.Characters.Nick.Data;
using ProjectN.Characters.Nick.FiniteStateMachine;
using ProjectN.Characters.Nick.States;

public class PlayerHoldBlockState : PlayerGroundedState
{
    public PlayerHoldBlockState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        player.transform.position = player.GrabbedMovable.GetPlayerPosition(player.transform.position);
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        stateMachine.ChangeState(player.PushingIdleState);
    }
}
