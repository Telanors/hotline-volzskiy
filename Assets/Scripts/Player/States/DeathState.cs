using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : PlayerBaseState
{
    public DeathState(Player player, StateMachine stateMachine, InputsController inputsController) : base(player, stateMachine, inputsController) { }
    private float deathTimer;
    public override void Enter()
    {
        base.Enter();
        deathTimer = 0.0f;
        Player.TogglePlayer(false);
        Player.SetDrag(0.0f);
        UIManager.Instance.ScaleAnimation(UIManager.Instance.DeathPanel, 0.0f, 1.0f, null, 0.25f);
        TimeStopper.Instance.TimeScaleLerp(1.0f, 0.1f, 0.25f);
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void Update()
    {
        base.Update();
        deathTimer += Time.unscaledDeltaTime;
        if (deathTimer >= 3.0f) 
        {
            Action action = () => UIManager.Instance.StartButton();
            UIManager.Instance.ScaleAnimation(UIManager.Instance.DeathPanel, 1.0f, 0.0f, action, 0.25f);
        }
    }
}
