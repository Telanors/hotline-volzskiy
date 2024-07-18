using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AIPatrolState : AIBaseState
{
    private Vector3[] patrolPoints;
    private float patrolSpeed = 1.75f;
    private Vector3 targetPoint;
    private int targetPointIndex = 0;
    public AIPatrolState(AIEnemy AIEnemy, Vector3[] patrolPoints, float patrolSpeed): base(AIEnemy)
    {
        AIEnemy.OnNoise += SetTargetPoint;
        this.patrolPoints = patrolPoints;
        this.patrolSpeed = patrolSpeed;
    }
    public override void Enter()
    {
        base.Enter();
        AIEnemy.Agent.speed = patrolSpeed;
        if (patrolPoints.Length <= 0)
        {
            patrolPoints = new Vector3[1] { AIEnemy.transform.position };
        }
        targetPoint = FindNearestPatrolPoint();
        AIEnemy.Move(targetPoint);
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (patrolPoints.Length <= 1) return;
        if (Vector3.Distance(AIEnemy.transform.position, targetPoint) >= 1.0f) return;
        SetTargetPoint(NextPatrolPoint(targetPointIndex));
    }
    public override void Update()
    {
        base.Update();
        if (isExiting) return;
        if (Vector3.Distance(AIEnemy.transform.position, AIEnemy.Player.transform.position) <= AIEnemy.distanceDetectionZone)
        {
            if (Vector3.Angle(AIEnemy.Player.transform.position - AIEnemy.transform.position, AIEnemy.transform.forward) <= AIEnemy.angleDetectionZone * 0.5f)
            {
                Vector3 rayDirection = AIEnemy.PlayerCenterPosition - AIEnemy.transform.position;
                if (Physics.Raycast(AIEnemy.transform.position, rayDirection, out RaycastHit hit, AIEnemy.distanceDetectionZone))
                {
                    if (hit.transform.CompareTag("Player"))
                    {
                        AIEnemy.stateMachine.ChangeState(AIEnemy.ChasingState);
                    }
                }
            }
        }
    }
    public void SetTargetPoint(Vector3 point)
    {
        if (!AIEnemy.Agent.enabled) return;
        NavMeshPath path = new NavMeshPath();
        if (AIEnemy.Agent.CalculatePath(point, path) && path.status == NavMeshPathStatus.PathComplete)
        {
            targetPoint = point;
            AIEnemy.Move(path);
        }
    }
    private Vector3 NextPatrolPoint(int index)
    {
        targetPointIndex = (index + 1) % patrolPoints.Length;
        return patrolPoints[targetPointIndex];
    }
    private Vector3 FindNearestPatrolPoint()
    {
        targetPointIndex = 0;
        Vector3 targetPoint = patrolPoints[targetPointIndex];
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            if (Vector3.Distance(AIEnemy.transform.position, targetPoint) > Vector3.Distance(AIEnemy.transform.position, patrolPoints[i]))
            {
                targetPoint = patrolPoints[i];
                targetPointIndex = i;
            }
        }
        return targetPoint;
    }
}
