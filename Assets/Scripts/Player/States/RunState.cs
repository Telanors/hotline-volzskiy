using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : GroundedState
{
    public RunState(Player player, StateMachine stateMachine, InputsController inputsController) : base(player, stateMachine, inputsController)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Player.desiredMoveSpeed = Player.RunSpeed;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Vector3 moveDirection = GetMoveDirection();
        Move(moveDirection, Player.RunSpeed, Player.MoveForce);
    }

    public override void Update()
    {
        base.Update();
        if (isExiting) return;
        Player.SoundController.FootStepPlay();
        Player.RightArmController.ProceduralAnimator.ArmBobbing(Player.runBobbingFrequency, Player.runBobbingAmplitude);
        if (InputsController.InputDirection.sqrMagnitude <= 0.0f)
        {
            StateMachine.ChangeState(Player.IdleState);
        }
        else if (InputsController.crouchAction.IsPressed())
        {
            StateMachine.ChangeState(Player.SlideState);
        }
        else if (!InputsController.runAction.IsPressed())
        {
            StateMachine.ChangeState(Player.WalkState);
        }
    }
}
