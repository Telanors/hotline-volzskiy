using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PD_74_Prosvet : AdvanceWeapon
{
    protected override IEnumerator Shot(Func<bool> input)
    {
        StartCoroutine(FireRateDelay());
        ShotStart?.Invoke();
        animator.SetTrigger(Shot_Trigger);
        animator.SetBool(Trigger_Bool, true);
        effectsController.SoundPlay(weaponData.TriggerSound);
        if (!IsEmpty())
        {
            //var impact = ShotImpact();
            //effectsController.BulletSpawn(weaponData.Bullet, hits);
            SubMagazineAmmo();
            BulletSpawner.Spawn(Vision.position, Vision.forward, muzzle, bullet);
            effectsController.MuzzleFlashSpawn(weaponData.MuzzleFlashPrefab);
            effectsController.ShellSpawn(weaponData.ShellPrefab);
            effectsController.SoundPlay(weaponData.RandomShotSound);
            effectsController.SoundPlay(weaponData.ShutterDelaySound);
            ShotProcess?.Invoke();
        }
        while (input.Invoke())
        {
            yield return null;
        }
        ShotEnd?.Invoke();
        animator.SetBool(Trigger_Bool, false);
    }
}
