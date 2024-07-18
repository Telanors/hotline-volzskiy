using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputsController : MonoBehaviour
{
    private PlayerInputs playerInputs;
    public InputAction moveAction { get; private set; }
    public InputAction mouseAction { get; private set; }
    public InputAction fireAction { get; private set; }
    public InputAction jumpAction { get; private set; }
    public InputAction runAction { get; private set; }
    public InputAction pickUpAction { get; private set; }
    public InputAction dropAction { get; private set; }
    public InputAction menuAction { get; private set; }
    public InputAction crouchAction { get; private set; }
    public InputAction meleeAction { get; private set; }
    public Vector3 MouseDelta { get => mouseAction.ReadValue<Vector2>(); }
    public Vector3 InputDirection { get => moveAction.ReadValue<Vector2>(); }
    public bool IsRun { get; private set; }
    private void OnEnable()
    {
        playerInputs.Enable();
        runAction.started += RunSwitch;
    }
    private void OnDisable()
    {
        playerInputs.Disable();
        runAction.started -= RunSwitch;
    }
    private void Awake()
    {
        playerInputs = new PlayerInputs();
        moveAction = playerInputs.CharacterInput.Movement;
        mouseAction = playerInputs.CharacterInput.MouseDelta;
        fireAction = playerInputs.CharacterInput.Fire;
        jumpAction = playerInputs.CharacterInput.Jump;
        runAction = playerInputs.CharacterInput.RunToggle;
        pickUpAction = playerInputs.CharacterInput.PickUp;
        dropAction = playerInputs.CharacterInput.Drop;
        menuAction = playerInputs.CharacterInput.ESC;
        crouchAction = playerInputs.CharacterInput.Crouch;
        meleeAction = playerInputs.CharacterInput.MeleeAttack;
    }
    private void RunSwitch(InputAction.CallbackContext obj) => IsRun = !IsRun;
}

