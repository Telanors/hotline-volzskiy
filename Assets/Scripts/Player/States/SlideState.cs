using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideState : GroundedState
{
    public SlideState(Player player, StateMachine stateMachine, InputsController inputsController) : base(player, stateMachine, inputsController)
    {
    }
    public override void Enter()
    {
        base.Enter();
        Player.SetDrag(Player.SlideDrag);
        //Player.StairMaxHeight = 0.0f;
        //Player.DesiredColliderHeight = Player.SlideColliderHeight;
        Player.CameraManager.desiredYPosition = Player.SlideCameraYPosition;
        Player.RightArmController.ProceduralAnimator.StateWeightChange("PositionLayer", Player.SlideAnimationWeight, Player.SlideAnimationMultiply);
        Player.RigidBody.AddForce(GetMoveDirection() * Player.SlideForce, ForceMode.VelocityChange);
    }

    public override void Exit()
    {
        base.Exit(); 
        Player.SetDrag(Player.MoveDrag);
        Player.StairMaxHeight = 0.5f;
        Player.DesiredColliderHeight = Player.DefaultColliderHeight;
        Player.CameraManager.desiredYPosition = Player.CameraManager.DefaultPosition.y;
        Player.RightArmController.ProceduralAnimator.StateWeightChange("PositionLayer", 0.0f, Player.SlideAnimationMultiply);
    }

    public override void PhysicsUpdate()
    {
       
        AddSlopeAdditionVelocity();
    }

    public override void Update()
    {
        base.Update();
        if (isExiting) return;
        Player.desiredMoveSpeed = Player.VelocityMagnitude;
        if (InputsController.jumpAction.triggered)
        {
            StateMachine.ChangeState(Player.JumpState);
        }
        else if(InputsController.crouchAction.triggered)
        {
            StateMachine.ChangeState(Player.RunState);
        }
        else if (Player.VelocityMagnitude <= Player.WalkSpeed)
        {
            StateMachine.ChangeState(Player.CrouchState);
        }
    }
    private void AddSlopeAdditionVelocity()
    {
        //float groundAngle = Vector3.Angle(groundRay.normal, Vector3.up);
        //Vector3 horizontalMove = new Vector3(GetMoveDirection().x, 0.0f, 0.0f);
        //Player.RigidBody.AddForce(horizontalMove * Player.VelocityMagnitude, ForceMode.Acceleration);
        ////if (groundAngle >= 10.0f)
        ////{
        ////    if (Player.RigidBody.velocity.y < 0.0f)
        ////    {
        ////        Player.RigidBody.AddForce(Player.RigidBody.velocity.normalized * (groundAngle * 0.25f), ForceMode.Acceleration);
        ////    }
        ////}
    }
 
}
