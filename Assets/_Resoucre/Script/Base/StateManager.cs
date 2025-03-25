using UnityEngine;

public class StateManager
{
    private IState currentState;
    public IState CurrentState { get => currentState; }

    public void InitState(IState stateInit)
    {
        Debug.Log("init state");
        currentState = stateInit;
        currentState.EnterState();
    }

    public void ChangeState(IState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}
