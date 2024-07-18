using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchIdleState : GroundedState
{
    public CrouchIdleState(Player player, StateMachine stateMachine, InputsController inputsController) : base(player, stateMachine, inputsController)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Player.SetGravity(false);
        Player.desiredMoveSpeed = Player.CrouchSpeed;
        Player.DesiredColliderHeight = Player.CrouchColliderHeight;
        Player.CameraManager.desiredYPosition = Player.CrouchCameraYPosition;
        Player.RightArmController.ProceduralAnimator.StateWeightChange("CrouchLayer", Player.CrouchAnimationWeight, Player.CrouchAnimationMultiply);
    }

    public override void Exit()
    {
        base.Exit();
        Player.SetGravity(true);
        Player.DesiredColliderHeight = Player.DefaultColliderHeight;
        Player.CameraManager.desiredYPosition = Player.CameraManager.DefaultPosition.y;
        Player.RightArmController.ProceduralAnimator.StateWeightChange("CrouchLayer", 0.0f, Player.CrouchAnimationMultiply);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    public override void Update()
    {
        base.Update();
        if (isExiting) return;
        Player.RightArmController.ProceduralAnimator.ArmResetPosition(Vector3.zero);
        if (!InputsController.crouchAction.IsPressed())
        {
            StateMachine.ChangeState(Player.IdleState);
        }
        else if (InputsController.InputDirection.sqrMagnitude != 0.0f)
        {
            StateMachine.ChangeState(Player.CrouchState);
        }
    }
}
