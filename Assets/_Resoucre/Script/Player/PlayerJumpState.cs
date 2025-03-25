
public class PlayerJumpState : EntityState<Player>
{
    public PlayerJumpState(StateManager stateManager, Player entity) : base(stateManager, entity)
    {
    }

    public override void EnterState()
    {
        entity.OnSysnAnimation(constant.JUMP , true);
        entity.JumpHandler();
    }

    public override void ExcuteState()
    {
        entity.GravityHandler();
        if (entity.GroundCheck() && entity.Velocity.y < 0)
        {
            SwitchState(entity.factory.GetState(PlayerState.IDLE));
        }
    }

    public override void ExitState()
    {
        entity.OnSysnAnimation(constant.JUMP, false);
    }
}
