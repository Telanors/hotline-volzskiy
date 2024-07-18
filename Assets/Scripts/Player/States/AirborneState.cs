using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AirborneState : PlayerBaseState
{
    public AirborneState(Player player, StateMachine stateMachine, InputsController inputsController) : base(player, stateMachine, inputsController)
    {

    }
    public override void Enter()
    {
        base.Enter();
        Player.SetDrag(Player.FallDrag);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Update()
    {
        base.Update();
    }
    protected void Move(Vector3 moveDirection, float maxSpeed)
    {
        Rigidbody rigidbody = Player.RigidBody;
        rigidbody.AddForce(moveDirection * maxSpeed * Player.AirForce, ForceMode.Acceleration);
        Vector3 horizontalVelocity = Player.HorizontalVelocity;
        if (horizontalVelocity.magnitude > maxSpeed)
        {
            rigidbody.velocity = horizontalVelocity.normalized * maxSpeed + new Vector3(0.0f, rigidbody.velocity.y, 0.0f);
        }
    }
    protected Vector3 GetFallMoveDirection()
    {
        Vector2 moveDelta = InputsController.InputDirection;
        return Player.Orientation.TransformDirection(new Vector3(moveDelta.x, 0.0f, moveDelta.y)).normalized;
    }
}
