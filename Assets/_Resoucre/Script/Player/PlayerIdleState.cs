public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(StateManager stateManager, Player entity) : base(stateManager, entity)
    {
    }

    public override void EnterState()
    {
        entity.OnSysnAnimation(constant.IDLE , true);
    }

    public override void ExcuteState()
    {
        base.ExcuteState();
        if (entity.CheckInputMove())
        {
            SwitchState(entity.factory.GetState(PlayerState.RUN));
        }
    }

    public override void ExitState()
    {
        entity.OnSysnAnimation(constant.IDLE, false);
    }

    public override void SwitchState(IState newState)
    {
        stateManager.ChangeState(newState);
    }
}
