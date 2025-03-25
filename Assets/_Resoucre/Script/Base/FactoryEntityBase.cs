public abstract class FactoryEntityBase<T>
{
    protected T context;
    protected StateManager stateManager;

    public T Context { get => context;}
    public StateManager StateManager { get => stateManager;}

    public FactoryEntityBase( StateManager stateManager ,T context)
    {
        this.stateManager = stateManager;
        this.context = context;
    }
}
