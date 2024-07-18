using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class ProceduralArmAnimator : MonoBehaviour 
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform recoilAnchor;
    [SerializeField] private Transform bobbingAnchor;
    [SerializeField] private Transform swayAnchor;

    public float swayLookMultiply;
    public float swayMoveMultiply;
    public float swayPositionMultiply;
    public float swaySmooth;

    private Dictionary<int, Coroutine> weightCoroutine = new Dictionary<int, Coroutine>();
    private float bobTimer;
    private float recoilTimer;
    public void ArmBobbing(float bobFrequancy, float bobAmplitude)
    {
        Vector3 bobTarget = Vector3.zero;
        bobTimer += Time.deltaTime * bobFrequancy;
        bobTarget.y += Mathf.Sin(bobTimer) * bobAmplitude;
        bobTarget.x += Mathf.Cos(bobTimer / 2) * bobAmplitude * 2;
        bobbingAnchor.localPosition += bobTarget;
    }
    public void ArmResetPosition(Vector3 defaultArmPosition)
    {
        if (bobbingAnchor.localPosition == defaultArmPosition) return;
        bobbingAnchor.localPosition = Vector3.Lerp(bobbingAnchor.localPosition, defaultArmPosition, 2.0f * Time.deltaTime);
        bobTimer = Mathf.Lerp(bobTimer, 0.0f, 2.0f * Time.deltaTime);
    }
    public void SwayArm(Vector2 mouseDelta, Vector2 inputDirection)
    {
        Vector3 mouseSway = mouseDelta * swayLookMultiply;
        Quaternion rotationX = Quaternion.AngleAxis(-mouseSway.y, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseSway.x, Vector3.up);
        Quaternion rotationZ = Quaternion.AngleAxis(-mouseSway.x, Vector3.forward);

        Vector3 movementDirection = inputDirection * swayMoveMultiply;
        rotationZ *= Quaternion.AngleAxis(movementDirection.x, Vector3.forward);
         
        Vector3 targetPositionSway = new Vector3(inputDirection.x * swayPositionMultiply, swayAnchor.localPosition.y, -inputDirection.y * swayPositionMultiply);

        Quaternion targetRotation = rotationX * rotationY * rotationZ;
        swayAnchor.localRotation = Quaternion.Slerp(swayAnchor.localRotation, targetRotation, swaySmooth * Time.deltaTime);
        swayAnchor.localPosition = Vector3.Lerp(swayAnchor.localPosition, targetPositionSway, swaySmooth * Time.deltaTime);
    }
    public void DoArmRecoil(WeaponData data)
    {
        if (data.RecoilTimeMultiply <= 0.0f) return;
        StartCoroutine(ArmRecoilIntactCoroutine(data));
    }
    private IEnumerator ArmRecoilIntactCoroutine(WeaponData weaponData)
    {
        recoilTimer = 0.0f;
        AnimationCurve YCurve = weaponData.YAnimationCurve;
        AnimationCurve ZCurve = weaponData.ZAnimationCurve;
        float shotTime = 1.0f / weaponData.FireRate;
        float xRotationOffset = 1.0f / weaponData.XRotationRecoil;
        float yPositionOffset = 1.0f / weaponData.YPositionRecoil;
        float zPositionOffset = 1.0f / weaponData.ZPositionRecoil;
        float timeMultiply = weaponData.RecoilTimeMultiply;
        float perlinNoiseMultuply = weaponData.RecoilPerlinNoiseMultuply;
        int perlinSide = Random.Range(0, 2) - 1;

        Vector3 recoilPositionTarget = Vector3.zero;
        Vector3 recoilRotationTarget = Vector3.zero;
        while (recoilTimer <= shotTime)
        {
            recoilPositionTarget.y = YCurve.Evaluate(recoilTimer * (1.0f / shotTime)) / yPositionOffset;
            recoilPositionTarget.z = ZCurve.Evaluate(recoilTimer * (1.0f / shotTime)) / zPositionOffset;

            recoilRotationTarget.x = ZCurve.Evaluate(recoilTimer * (1.0f / shotTime)) / xRotationOffset;
            recoilRotationTarget.y = Mathf.PerlinNoise(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) * perlinNoiseMultuply * perlinSide;
            recoilRotationTarget.z = Mathf.PerlinNoise(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)) * perlinNoiseMultuply * -perlinSide;

            recoilAnchor.localPosition = recoilPositionTarget;
            recoilAnchor.localEulerAngles = recoilRotationTarget;
            recoilTimer += Time.deltaTime * timeMultiply;
            perlinSide = Random.Range(0, 2) - 1;
            yield return null;
        }
        recoilAnchor.localPosition = Vector3.zero;
        recoilAnchor.localEulerAngles = Vector3.zero;
    }
    public void StateWeightChange(string layerName, float targetWeight, float multiply)
    {
        int layerIndex = animator.GetLayerIndex(layerName);
        float currentWeight = animator.GetLayerWeight(layerIndex);
        if (currentWeight == targetWeight) return;
        if(weightCoroutine.ContainsKey(layerIndex))
        {
            if (weightCoroutine[layerIndex] != null)
            { 
                StopCoroutine(weightCoroutine[layerIndex]); 
            }
            weightCoroutine.Remove(layerIndex);
        }
        weightCoroutine.Add(layerIndex, StartCoroutine(StateWeightChangeCoroutine(layerIndex, currentWeight, targetWeight, multiply)));
    }
    public void StateWeightChange(string layerName, float targetWeight)
    {
        int layerIndex = animator.GetLayerIndex(layerName);
        animator.SetLayerWeight(layerIndex, targetWeight);
    }
    private IEnumerator StateWeightChangeCoroutine(int layerIndex, float currentWeight, float targetWeight, float multiply)
    {
        float timer = 0.0f;
        while (timer <= 1.0f)
        {
            timer += Time.deltaTime * multiply;
            currentWeight = Mathf.Lerp(currentWeight, targetWeight, timer);
            animator.SetLayerWeight(layerIndex, currentWeight);
            yield return null;
        }
        weightCoroutine.Remove(layerIndex);
    }
}
