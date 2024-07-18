public class StateMachine
{
    public State CurrentState { get; private set; }
    public void Initialization(State state)
    {
        CurrentState = state;
        CurrentState.Enter();
    }
    public void ChangeState(State state)
    {
        CurrentState.isExiting = true;
        CurrentState.Exit();
        CurrentState = state;
        CurrentState.Enter();
        CurrentState.isExiting = false;
    }
    public void Update()
    {
        CurrentState.Update();
    }
    public void PhysicsUpdate()
    {
        CurrentState.PhysicsUpdate();
    }
    public void LateUpdate()
    {
        CurrentState.LateUpdate();
    }
}
