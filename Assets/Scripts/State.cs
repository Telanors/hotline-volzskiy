 using UnityEngine;
public abstract class State
{
    public bool isExiting;
    public abstract void Enter();
    public abstract void Update();
    public abstract void PhysicsUpdate();
    public abstract void LateUpdate();
    public abstract void Exit();
}