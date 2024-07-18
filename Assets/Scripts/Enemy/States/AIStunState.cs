using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStunState : AIBaseState
{
    private float stunTime = 5.0f;
    private float stunTimer;
    public AIStunState(AIEnemy AIEnemy) : base(AIEnemy)
    {
    }
    public override void Enter()
    {
        base.Enter();
        stunTimer = 0.0f;
        AIEnemy.Agent.ResetPath();
        AIEnemy.Agent.enabled = false;
        AIEnemy.animator.SetBool("Stun", true);
    }
    public override void Update()
    {
        base.Update();
        stunTimer += Time.deltaTime;
        if(stunTimer > stunTime)
        {
            AIEnemy.Agent.enabled = true;
            AIEnemy.animator.SetBool("Stun", false);
            AIEnemy.stateMachine.ChangeState(AIEnemy.ChasingState);
        }
    }
}
