using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeFallState : AirborneState
{
    public FreeFallState(Player player, StateMachine stateMachine, InputsController inputsController) : base(player, stateMachine, inputsController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.Coyote.PlayTimer();
    }

    public override void Exit()
    {
        base.Exit();
        Player.RightArmController.Animator.SetTrigger("Land");
        Player.Coyote.ResetTimer();
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
        Player.RightArmController.ProceduralAnimator.ArmResetPosition(Vector3.zero);
        if (isGrounded)
        {
            if (InputsController.InputDirection.sqrMagnitude != 0)
            {
                if (InputsController.runAction.IsPressed())
                {
                    StateMachine.ChangeState(Player.RunState);
                }
                else
                {
                    StateMachine.ChangeState(Player.WalkState);
                }
            }
            else
            {
                Player.desiredMoveSpeed = Player.WalkSpeed;
                StateMachine.ChangeState(Player.IdleState);
            }
        }
        else if (InputsController.jumpAction.triggered && Player.Coyote.Allowed)
        {
            StateMachine.ChangeState(Player.JumpState);
        }
    }
    
}
