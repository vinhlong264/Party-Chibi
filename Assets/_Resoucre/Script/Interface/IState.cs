public interface IState
{
    void EnterState();
    void ExcuteState();
    void ExitState();
    void SwitchState(IState newState);
}
