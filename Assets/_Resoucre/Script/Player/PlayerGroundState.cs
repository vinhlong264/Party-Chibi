using UnityEngine;

public class PlayerGroundState : EntityState<Player>
{
    public PlayerGroundState(StateManager stateManager, Player entity) : base(stateManager, entity)
    {
    }

    public override void ExcuteState()
    {
        if(entity.GetInput().isJump && entity.GroundCheck())
        {
            SwitchState(entity.factory.GetState(PlayerState.JUMP));
            entity._Input.ResetInput();
            return;
        }

        if (entity.GetInput().isAttack)
        {
            Debug.Log("Attack change");
            SwitchState(entity.factory.GetState(PlayerState.ATTACK));
            entity._Input.ResetInput();
            return;
        }
    }
}
