using UnityEngine.Rendering;

public class PlayerAttackState : EntityState<Player>
{
    public PlayerAttackState(StateManager stateManager, Player entity) : base(stateManager, entity)
    {
    }

    public override void EnterState()
    {
        entity.OnSysnAnimation(constant.ATTACK , true);
        entity.AttackHander();
    }

    public override void ExcuteState()
    {
        if (entity.IsTrigger)
        {
            SwitchState(entity.factory.GetState(PlayerState.IDLE));
        }
    }

    public override void ExitState()
    {
        entity.OnSysnAnimation(constant.ATTACK, false);
        entity.AnimTriggerFalse();
    }
}
