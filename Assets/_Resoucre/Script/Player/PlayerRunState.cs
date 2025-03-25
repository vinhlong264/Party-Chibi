public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(StateManager stateManager, Player entity) : base(stateManager, entity)
    {
    }

    public override void EnterState()
    {
        entity.OnSysnAnimation(constant.RUN, true);
    }

    public override void ExcuteState()
    {
        base.ExcuteState();
        entity.RunHandler();
        if (!entity.CheckInputMove())
        {
            SwitchState(entity.factory.GetState(PlayerState.IDLE));
        }
    }

    public override void ExitState()
    {
        entity.OnSysnAnimation(constant.RUN,false);
    }

    public override void SwitchState(IState newState)
    {
        stateManager.ChangeState(newState);
    }
}
