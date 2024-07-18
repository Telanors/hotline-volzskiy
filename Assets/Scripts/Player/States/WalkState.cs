using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : GroundedState
{
    public WalkState(Player player, StateMachine stateMachine, InputsController inputsController) : base(player, stateMachine, inputsController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.desiredMoveSpeed = Player.WalkSpeed;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Vector3 moveDirection = GetMoveDirection();
        Move(moveDirection, Player.WalkSpeed, Player.MoveForce);
    }

    public override void Update()
    {
        base.Update();
        if (isExiting) return;
        Player.SoundController.FootStepPlay();
        Player.RightArmController.ProceduralAnimator.ArmBobbing(Player.walkBobbingFrequency, Player.walkBobbingAmplitude);
        if (InputsController.InputDirection.sqrMagnitude <= 0.0f)
        {
            StateMachine.ChangeState(Player.IdleState);
        }
        else if (InputsController.runAction.IsPressed())
        {
            StateMachine.ChangeState(Player.RunState);
        }
        else if (InputsController.crouchAction.IsPressed())
        {
            StateMachine.ChangeState(Player.CrouchState);
        }
    }
}
