using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : GroundedState
{
    public IdleState(Player player, StateMachine stateMachine, InputsController inputsController) : base(player, stateMachine, inputsController)
    {

    }
    public override void Enter()
    {
        base.Enter();
        Player.SetGravity(false);
    }

    public override void Exit()
    {
        base.Exit();
        Player.SetGravity(true);
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
        if (InputsController.crouchAction.IsPressed())
        {
            StateMachine.ChangeState(Player.CrouchIdleState);
        }
        else if(InputsController.InputDirection.sqrMagnitude != 0.0f)
        {
            StateMachine.ChangeState(Player.WalkState);
        }
    }
}
