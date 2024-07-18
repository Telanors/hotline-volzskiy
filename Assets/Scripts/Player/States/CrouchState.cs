using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : GroundedState
{
    public CrouchState(Player player, StateMachine stateMachine, InputsController inputsController) : base(player, stateMachine, inputsController)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Player.desiredMoveSpeed = Player.CrouchSpeed;
        Player.DesiredColliderHeight = Player.CrouchColliderHeight;
        Player.CameraManager.desiredYPosition = Player.CrouchCameraYPosition;
        Player.RightArmController.ProceduralAnimator.StateWeightChange("CrouchLayer", Player.CrouchAnimationWeight, Player.CrouchAnimationMultiply);
    }

    public override void Exit()
    {
        base.Exit();
        Player.DesiredColliderHeight = Player.DefaultColliderHeight;
        Player.CameraManager.desiredYPosition = Player.CameraManager.DefaultPosition.y;
        Player.RightArmController.ProceduralAnimator.StateWeightChange("CrouchLayer", 0.0f, Player.CrouchAnimationMultiply);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        Vector3 moveDirection = GetMoveDirection();
        Move(moveDirection, Player.CrouchSpeed, Player.MoveForce);
    }

    public override void Update()
    {
        base.Update();
        if (isExiting) return;
        Player.SoundController.FootStepPlay();
        Player.RightArmController.ProceduralAnimator.ArmBobbing(Player.crouchBobbingFrequency, Player.crouchBobbingAmplitude);
        if (InputsController.InputDirection.sqrMagnitude <= 0.0f)
        {
            StateMachine.ChangeState(Player.CrouchIdleState);
        }
        else if (!InputsController.crouchAction.IsPressed())
        {
            StateMachine.ChangeState(Player.IdleState);
        }
    }
}
