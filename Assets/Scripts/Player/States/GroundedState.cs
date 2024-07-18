using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GroundedState : PlayerBaseState
{
    public GroundedState(Player player, StateMachine stateMachine, InputsController inputsController) : base(player, stateMachine, inputsController)
    { }
    public override void Enter()
    {
        base.Enter();
        Player.SetDrag(Player.MoveDrag);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (isGrounded)
        {
            MoveCollider();
        }
    }

    public override void Update()
    {
        base.Update();
        if (!isGrounded)
        {
            StateMachine.ChangeState(Player.FreeFallState);
        }
        else if (InputsController.jumpAction.triggered)
        {
            StateMachine.ChangeState(Player.JumpState);
        }
    }
    public void MoveCollider()
    {
        if (Player.StairMaxHeight <= 0.0f) return;
        if (Physics.SphereCast(Player.GroundCheckPosition, Player.GroundRadius ,Vector3.down, out RaycastHit hit, Player.GroundRayCastMaxDistance + Player.GroundRadius, Player.groundMask))
        {
            float distance = Player.Collider.center.y - hit.distance - Player.GroundRadius;
            if (distance == 0.0f) return;
            float amount = distance * Player.StairSmooth - Player.RigidBody.velocity.y;
            Player.RigidBody.AddForce(new Vector3(0.0f, amount, 0.0f), ForceMode.VelocityChange);
        }
    }
    public void Move(Vector3 moveDirection, float maxSpeed, float moveForce)
    {
        Rigidbody rigidbody = Player.RigidBody;
        rigidbody.AddForce(moveDirection * maxSpeed * moveForce, ForceMode.Acceleration);
        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            Vector3 brakeVelocity = rigidbody.velocity.normalized * (rigidbody.velocity.magnitude - maxSpeed);
            rigidbody.AddForce(-brakeVelocity * moveForce, ForceMode.Acceleration);
        }
    }
}
