using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIChasingState : AIBaseState
{
    private float chasingSpeed = 4.5f;
    private float distanceToTarget; 
    private float chaseTimer;
    private bool chanceForPlayer = false;
    public AIChasingState(AIEnemy AIEnemy, float chasingSpeed) : base(AIEnemy)
    {
        this.chasingSpeed = chasingSpeed;
    }
    public override void Enter()
    {
        base.Enter();
        AIEnemy.WeaponIK.SetTarget(AIEnemy.Player.transform, () => AIEnemy.Player.Collider.center);
        AIEnemy.WeaponIK.SetAim(AIEnemy.visionTransform);
        AIEnemy.StateWeightChange("Attack Layer", 1.0f, 1.0f);
        chaseTimer = 0.0f;
        AIEnemy.attackTimer = 0.0f;
        AIEnemy.Agent.speed = chasingSpeed;
        ChaseTarget(AIEnemy.Player.transform.position);
    }

    public override void Exit()
    {
        base.Exit();
        AIEnemy.WeaponIK.SetTarget(null, null);
        AIEnemy.WeaponIK.SetAim(null);
        AIEnemy.StateWeightChange("Attack Layer", 0.0f, 1.0f);
        StopChaseTarget(false);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        distanceToTarget = Vector3.Distance(AIEnemy.transform.position, AIEnemy.Player.transform.position);
        chaseTimer += Time.fixedDeltaTime;
        if (chaseTimer > 0.5f)
        {
            chaseTimer = 0.0f;
            ChaseTarget(AIEnemy.Player.transform.position);
        }
        if (distanceToTarget <= AIEnemy.kitingDistance && !Physics.Raycast(AIEnemy.Vision.position, AIEnemy.Vision.forward, distanceToTarget, AIEnemy.enviromentMask))
        {
            StopChaseTarget(true);
        }
        else
        {
            StopChaseTarget(false);
        }
    }
    public override void LateUpdate()
    {
        base.LateUpdate();
    }
    public override void Update()
    {
        base.Update();
        if (isExiting) return;
        AIEnemy.attackTimer += Time.deltaTime;
        if (AIEnemy.Player.Health > 0.0f && AIEnemy.attackTimer >= AIEnemy.attackDelay)
        {
            if (!Physics.Raycast(AIEnemy.Vision.position, AIEnemy.Vision.forward, distanceToTarget, AIEnemy.enviromentMask))
            {
                if (chanceForPlayer && distanceToTarget < AIEnemy.Equipment.CurrentWeapon.WeaponData.MaxRaycastDistance + 2.0f)
                {
                    AIEnemy.Equipment.UseWeapon(AIEnemy);
                }
                else
                {
                    AIEnemy.attackTimer = AIEnemy.attackDelay * 0.4f;
                    chanceForPlayer = true;
                }
            }
            else
            {
                chanceForPlayer = false;
            }
        }
        if (distanceToTarget > AIEnemy.distanceDetectionZone * 1.5f)
        {
            AIEnemy.stateMachine.ChangeState(AIEnemy.PatrolState);
        }
    }
    private void ChaseTarget(Vector3 targerPoint)
    {
        AIEnemy.Move(targerPoint);
    }
    private void StopChaseTarget(bool value)
    {
        AIEnemy.Agent.isStopped = value;
    }
}
