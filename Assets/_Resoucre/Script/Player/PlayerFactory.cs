using System.Collections.Generic;

public class PlayerFactory : FactoryEntityBase<Player>
{
    private readonly Dictionary<PlayerState, IState> states = new Dictionary<PlayerState, IState>();
    public PlayerFactory(StateManager stateManager, Player context) : base(stateManager, context)
    {
        states[PlayerState.IDLE] = new PlayerIdleState(stateManager , context);
        states[PlayerState.RUN] = new PlayerRunState(stateManager, context);
        states[PlayerState.JUMP] = new PlayerJumpState(stateManager , context);
        states[PlayerState.ATTACK] = new PlayerAttackState(stateManager , context);
    }

    public IState GetState(PlayerState state)
    {
        IState getState;

        if(states.TryGetValue(state , out IState value))
        {
            getState = value;
        }
        else
        {
            getState = null;
        }

        return getState;
    }
}
