using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIFindWeaponState : AIBaseState
{
    private float findSpeed;
    private AdvanceWeapon targerWeapon;
    public AIFindWeaponState(AIEnemy AIEnemy, float findSpeed) : base(AIEnemy) 
    {
        this.findSpeed = findSpeed;
    }
    public override void Enter()
    {
        base.Enter();
        AIEnemy.Agent.speed = findSpeed;
        AIEnemy.Move(targerWeapon.transform.position);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if(targerWeapon.Equipped || targerWeapon == null)
        {

        }
    }

    public override void Update()
    {
        base.Update();
        if(Vector3.Distance(AIEnemy.transform.position, targerWeapon.transform.position) <= 0.15f)
        {
            AIEnemy.Equipment.PickUp(targerWeapon);
            AIEnemy.stateMachine.ChangeState(AIEnemy.PatrolState);
        }
    }
}
