using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : State
{
    public bool isExiting;
    protected bool isGrounded;
    protected bool isRoofed;
    protected RaycastHit groundRay;
    public Player Player { get; private set; }
    public StateMachine StateMachine { get; private set; }
    public InputsController InputsController { get; private set; }
    public PlayerBaseState(Player player, StateMachine stateMachine, InputsController inputsController)
    {
        Player = player;
        StateMachine = stateMachine;
        InputsController = inputsController;
    }
    public override void Enter()
    {
        //Debug.Log(this);
    }
    public override void Update()
    {
        isGrounded = GroundCheck();
        Player.RightArmController.ProceduralAnimator.SwayArm(InputsController.MouseDelta, InputsController.InputDirection);
        if (Player.Collider.height != Player.DesiredColliderHeight)
        {
            Player.AdjustColliderHeight();
        }
        if (Player.CameraManager.rootPosition.y != Player.CameraManager.desiredYPosition)
        {
            Player.CameraManager.AdjustYPosition(Player.desiredMoveSpeed);
        }
    }
    public override void PhysicsUpdate() { }
    public override void LateUpdate() { }
    public override void Exit() { }
    public bool GroundCheck()
    {
        return Physics.SphereCast(new Ray(Player.GroundCheckPosition, Vector3.down), Player.GroundRadius, out groundRay, Player.GroundRayCastMaxDistance, Player.groundMask, QueryTriggerInteraction.Ignore);
    }
    public bool RoofCheck()
    {
        return Physics.SphereCast(new Ray(Player.GroundCheckPosition, Vector3.up), Player.GroundRadius, out groundRay, Player.GroundRayCastMaxDistance, Player.groundMask, QueryTriggerInteraction.Ignore);
    }
    public Vector3 GetMoveDirection()
    {
        Vector2 moveDelta = InputsController.InputDirection;
        return Vector3.ProjectOnPlane(Player.Orientation.TransformDirection(new Vector3(moveDelta.x, 0.0f, moveDelta.y)), groundRay.normal).normalized;
    }
}