using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fist : AdvanceWeapon
{
    public bool OnlyRaycastShot = false;
    protected override IEnumerator Shot(System.Func<bool> input)
    {
        StartCoroutine(FireRateDelay());
        ShotStart?.Invoke();
        yield return null;
        FistImpact();
        effectsController.SoundPlay(weaponData.TriggerSound);
        ShotProcess?.Invoke();
        ShotEnd?.Invoke();
        while (!readyToShoot)
        {
            yield return null;
        }
    }

    private void FistImpact()
    {
        Ray ray = new Ray(Vision.position, Vision.forward);
        if (Physics.Raycast(ray, out RaycastHit hit1, WeaponData.MaxRaycastDistance, ~WeaponData.Bullet.IgnoreMask))
        {
            effectsController.SoundPlay(weaponData.RandomShotSound);
            if (hit1.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(weaponData.Bullet.Damage);
            }
            if (hit1.transform.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddForceAtPosition(Vision.forward * weaponData.Bullet.ImpactForce, hit1.point, ForceMode.VelocityChange);
            }
            return;
        }
        if (OnlyRaycastShot) return;
        if (Physics.BoxCast(
            Vision.position,
            new Vector3(0.25f, 0.25f, 0.25f),
            Vision.forward,
            out RaycastHit hit2,
            Quaternion.identity,
            WeaponData.MaxRaycastDistance,
            ~WeaponData.Bullet.IgnoreMask))
        {
            effectsController.SoundPlay(weaponData.RandomShotSound);
            if (hit2.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(weaponData.Bullet.Damage);
            }
            if (hit2.transform.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddForceAtPosition(Vision.forward * weaponData.Bullet.ImpactForce, hit2.point, ForceMode.VelocityChange);
            }
        }
    }
}
