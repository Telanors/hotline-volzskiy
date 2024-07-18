using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIBaseState : State
{
    protected AIEnemy AIEnemy;
    public AIBaseState(AIEnemy AIEnemy)
    {
        this.AIEnemy = AIEnemy;
    }
    public override void Enter()
    {
 
    }

    public override void Exit()
    {

    }

    public override void PhysicsUpdate()
    {

    }
    public override void LateUpdate() 
    { 
    
    }
    public override void Update()
    {
        
    }
}
