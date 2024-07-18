using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCameraManager : MonoBehaviour, IEntityVision
{
    public float desiredYPosition;
    [Range(0.0f, 150.0f)] public float mouseSensibility;
    [Range(0.0f, 360.0f)] public float maxAngle;

    [SerializeField] private CinemachineVirtualCamera cmCamera;
    [SerializeField] private InputsController inputsController;
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private Transform orientation;
    [SerializeField] private Vector3 defaultPosition;
    private Vector3 rotation;
    private Vector2 mouseDelta;

    public Vector2 Rotation
    { 
        get => rotation;
        set => rotation = value;
    }
    public float XRotation
    {
        get => rotation.x;
        set => rotation.x = value;
    }
    public float YRotation
    {
        get => rotation.y;
        set => rotation.y = value;
    }
    public float ZRotation
    {
        get => rotation.z;
        set => rotation.z = value;
    }
    public Transform visionTransform { get => mainCamera; }
    public Vector3 forward { get => mainCamera.forward; }
    public Vector3 right { get => mainCamera.right; }
    public Vector3 up { get => mainCamera.up; }
    public Vector3 position { get => mainCamera.position; }
    public Vector3 rootPosition { get => cameraRoot.localPosition; set => cameraRoot.localPosition = value; }
    public Vector2 MouseDelta { get => mouseDelta; }
    public CinemachineVirtualCamera CMCamera { get => cmCamera; }
    public Vector3 DefaultPosition { get => defaultPosition; }
    [Header("Sway parametrs")]
    public float swayMoveMultiply;
    public float swaySmooth;

    private void Start()
    {
        desiredYPosition = cameraRoot.localPosition.y;
        defaultPosition = cameraRoot.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void LateUpdate()
    {
        CameraMove();
        CameraMoveSway(inputsController.InputDirection.x);
    }
    private void CameraMove()
    {
        mouseDelta = inputsController.MouseDelta * mouseSensibility;
        rotation += new Vector3(mouseDelta.y, mouseDelta.x,0.0f);
        rotation.x = Mathf.Clamp(rotation.x, -maxAngle, maxAngle);

        if (rotation.y >= 360 || rotation.y <= -360) rotation.y = 0;

        cameraRoot.localRotation = Quaternion.Euler(-rotation.x, 0.0f, rotation.z);
        orientation.localRotation = Quaternion.Euler(0.0f, rotation.y, 0.0f);
    }
    private void CameraMoveSway(float xDirection)
    {
        float targetZRotation = xDirection * swayMoveMultiply;
        rotation.z = Mathf.Lerp(rotation.z, targetZRotation, swaySmooth * Time.deltaTime);
    }
    public void AdjustYPosition(float speed)
    {
        cameraRoot.localPosition = new Vector3(cameraRoot.localPosition.x, Mathf.Lerp(cameraRoot.localPosition.y, desiredYPosition, speed * Time.deltaTime), cameraRoot.localPosition.z);
    }
}
