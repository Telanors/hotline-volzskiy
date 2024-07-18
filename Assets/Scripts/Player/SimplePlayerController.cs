using UnityEngine.InputSystem;
using System.Collections;
using System;
using UnityEngine;
using Cinemachine;
using TMPro;
public class SimplePlayerController : MonoBehaviour
{
    private Vector2 rotation;
    public float sensetivity;
    public float maxSpeed;
    public float moveForce;
    public float pickUpDistance;
    public Transform mainCamera;
    public Transform orientation;
    public Rigidbody rb;
    public InputsController inputsController;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Locked;
        inputsController = GetComponent<InputsController>();
    }
    private void LateUpdate()
    {
        CameraRotate();
    }
    
    private void CameraRotate()
    {
        rotation -= inputsController.mouseAction.ReadValue<Vector2>() * sensetivity;
        rotation.y = Mathf.Clamp(rotation.y, -90, 90);
        mainCamera.localRotation = Quaternion.Euler(rotation.y, 0.0f, 0.0f);
        orientation.localRotation = Quaternion.Euler(0.0f, -rotation.x, 0.0f);
    }
}
