public abstract class EntityState<T> : IState
{
    protected StateManager stateManager;
    protected T entity;

    protected EntityState(StateManager stateManager, T entity)
    {
        this.stateManager = stateManager;
        this.entity = entity;
    }

    public virtual void EnterState()
    {

    }
    public virtual void ExcuteState()
    {

    }
    public virtual void ExitState()
    {

    }
    public virtual void SwitchState(IState newState)
    {
        stateManager.ChangeState(newState);
    }
}
