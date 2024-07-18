using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : AirborneState
{
    private float jumpTimer;
    public JumpState(Player player, StateMachine stateMachine, InputsController inputsController) : base(player, stateMachine, inputsController)
    {
    }
    public override void Enter()
    {
        base.Enter();
        jumpTimer = 0.0f;
        Player.RigidBody.velocity = Player.HorizontalVelocity;
        Jump(Vector3.up, Player.JumpForce);
        Player.RightArmController.Animator.SetTrigger("Jump");
    }

    public override void Exit()
    {
        base.Exit();
        Player.Coyote.Disable();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Vector3 moveDirection = GetFallMoveDirection();
        Move(moveDirection, Player.desiredMoveSpeed);
    }

    public override void Update()
    {
        base.Update();
        jumpTimer += Time.deltaTime;
        if(!isGrounded  || jumpTimer > Player.JumpTime)
        {
            StateMachine.ChangeState(Player.FreeFallState);
        } 
    }
    private void Jump(Vector3 jumpDirection, float jumpForce)
    {
        if (Player.RigidBody.velocity.y > 1.0f)
        {
            jumpDirection *= Player.RigidBody.velocity.y;
        }
        Player.RigidBody.AddForce(jumpDirection * jumpForce, ForceMode.VelocityChange);
    }
}
