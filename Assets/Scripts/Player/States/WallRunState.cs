using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunState : AirborneState
{
    public WallRunState(Player player, StateMachine stateMachine, InputsController inputsController) : base(player, stateMachine, inputsController)
    {
        
    }
    //public override void Enter()
    //{
    //    base.Enter();
    //    Player.SetGravity(false);
    //    Player.RigidBody.velocity = new Vector3(Player.RigidBody.velocity.x, 0.0f, Player.RigidBody.velocity.z);
    //}
    //public override void Update()
    //{
    //    if (isGrounded)
    //    {
    //        StateMachine.ChangeState(Player.WalkState);
    //    }
    //    else if (InputsController.jumpAction.triggered)
    //    {
    //        StateMachine.ChangeState(Player.JumpState);
    //    }
    //}

    //public override void PhysicsUpdate()
    //{
    //    base.PhysicsUpdate();
    //    Vector3 direction = GetDirection();
    //    Player.RigidBody.AddForce(direction * Player.RunSpeed * Player.MoveForce, ForceMode.Acceleration);
    //    Vector3 horizontalVelocity = new Vector3(Player.RigidBody.velocity.x, 0.0f, Player.RigidBody.velocity.z);
    //    if(horizontalVelocity.magnitude > Player.RunSpeed)
    //    {
    //        Player.RigidBody.velocity = horizontalVelocity.normalized * Player.RunSpeed + new Vector3(0.0f, Player.RigidBody.velocity.y, 0.0f);
    //    }
    //    Player.RigidBody.AddForce(Vector3.down * 5f, ForceMode.Acceleration);
    //    Player.desiredMoveSpeed = Player.RigidBody.velocity.magnitude;
    //}
    //public override void Exit()
    //{
    //    base.Exit();
    //    Player.SetGravity(true);
    //}

    //public Vector3 GetDirection()
    //{
    //    Vector3 cross = Vector3.Cross(wallRayHit.normal, Vector3.up);
    //    if ((Player.Orientation.forward - cross).magnitude > (Player.Orientation.forward - -cross).magnitude)
    //    {
    //        return -cross;
    //    }
    //    return cross;
    //}
}
