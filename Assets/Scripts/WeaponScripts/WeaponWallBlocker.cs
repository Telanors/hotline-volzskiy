using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponWallBlocker : MonoBehaviour
{
    private PlayerCameraManager playerCamera;
    private ProceduralArmAnimator armProceduralAnimator;

    private LayerMask ignoreMask;
    private float cameraToMuzzleDistance;
    private void Awake()
    {
        var arm = GetComponent<RightArmController>();
        playerCamera = arm.PlayerCamera;
        armProceduralAnimator = arm.ProceduralAnimator;
        ignoreMask = arm.ignoreMask;
        arm.OnPickUp.AddListener(() => 
        {
            cameraToMuzzleDistance = Vector3.Distance(playerCamera.position, arm.CurrentWeapon.Muzzle.position);
        });
    }
    private void FixedUpdate()
    {
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, cameraToMuzzleDistance + 0.05f, ~ignoreMask))
        {
            if (Vector3.Distance(playerCamera.position, hit.point) <= cameraToMuzzleDistance)
            {
                armProceduralAnimator.StateWeightChange("ClippingLayer", 1.0f, 5.0f);
            }
            else
            {
                armProceduralAnimator.StateWeightChange("ClippingLayer", 0.0f, 5.0f);
            }
        }
        else
        {
            armProceduralAnimator.StateWeightChange("ClippingLayer", 0.0f, 5.0f);
        }
    }

}
