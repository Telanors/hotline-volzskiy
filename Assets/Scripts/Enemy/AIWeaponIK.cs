using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class HumanBone
{
    public Transform bone;
    [Range(0, 1)]
    public float weight = 1.0f;
}
public class AIWeaponIK : MonoBehaviour
{
    public float maxAngle = 75;
    public float maxDistance = 1.5f;
    public int iteration = 10;
    [Range(0, 1)]
    public float weight = 1.0f;
    public float coroutineMultiply = 2.0f;
    public float turnSpeed = 0.5f;

    [SerializeField] private Transform target;
    [SerializeField] private System.Func<Vector3> targetPositionOffset;

    [SerializeField] private Transform visionAim;
    [SerializeField] private HumanBone[] boneTransforms;

    private Coroutine aimCoroutine;
    private void LateUpdate()
    {
        if (target == null || visionAim == null) return;
        Vector3 targetPosition = GetTargetPosition();
        for (int i = 0; i < iteration; i++)
        {
            for (int j = 0; j < boneTransforms.Length; j++)
            {
                AimToTarget(boneTransforms[j].bone, visionAim, targetPosition, boneTransforms[j].weight * weight);
            }
        }
    }
    private Vector3 GetTargetPosition()
    {
        Vector3 targetPosition = target.position;
        if (targetPositionOffset != null) targetPosition += targetPositionOffset.Invoke();
        Vector3 aimDirection = visionAim.forward;
        Vector3 targetDirection = targetPosition - visionAim.position;
        float blendOut = 0.0f;
        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if (targetAngle > maxAngle)
        {
            blendOut += (targetAngle - maxAngle) / 50.0f;
            AimToTargetLerp(transform, transform, new Vector3(targetPosition.x, transform.position.y, targetPosition.z), 1.0f, coroutineMultiply);
        }
        float targeDistance = targetDirection.magnitude;
        if(targeDistance < maxDistance)
        {
            blendOut += maxDistance - targeDistance;
        }
        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return visionAim.position + direction;
    }
    private void AimToTarget(Transform bone, Transform aim, Vector3 target, float weight)
    {
        Vector3 aimDirection = aim.forward;
        Vector3 targetDirection = target - aim.position;
        Quaternion aimToward = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendRotation = Quaternion.Slerp(Quaternion.identity, aimToward, weight);
        bone.rotation = blendRotation * bone.rotation;
    }
    private void AimToTargetLerp(Transform bone, Transform aim, Vector3 target, float weight, float multyply)
    {
        if (aimCoroutine != null) return;
        aimCoroutine = StartCoroutine(AimToTargetCoroutine(bone, aim, target, weight, multyply));
    }
    private IEnumerator AimToTargetCoroutine(Transform bone, Transform aim, Vector3 target, float weight, float multyply)
    {
        float timer = 0.0f;
        Vector3 aimDirection = aim.forward;
        Vector3 targetDirection = target - aim.position;
        Quaternion aimToward = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendRotation = Quaternion.Slerp(Quaternion.identity, aimToward, weight);
        Quaternion targetRotation = bone.rotation * blendRotation;
        while (timer < 1.0f) 
        { 
            timer += Time.deltaTime * multyply;
            bone.rotation = Quaternion.Slerp(bone.rotation, targetRotation, timer);
            yield return null;
        }
        aimCoroutine = null;
    }
    public void SetTarget(Transform target, System.Func<Vector3> targetPositionOffset) 
    { 
        this.target = target; 
        this.targetPositionOffset = targetPositionOffset;
    }
    public void SetAim(Transform visionAim) => this.visionAim = visionAim;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(visionAim.position, visionAim.forward * 100);
    }
}
