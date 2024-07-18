using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PPL_M_Cheglock : AdvanceWeapon
{
    protected override IEnumerator Shot(Func<bool> input)
    {
        animator.SetBool(Trigger_Bool, true);
        effectsController.SoundPlay(weaponData.TriggerSound);
        ShotStart?.Invoke();
        do
        {
            if (readyToShoot)
            {
                StartCoroutine(FireRateDelay());
                animator.SetTrigger(Shot_Trigger);
                if (!IsEmpty())
                {
                    SubMagazineAmmo();
                    if (IsEmpty()) effectsController.SoundPlay(weaponData.ShutterDelaySound);
                    BulletSpawner.Spawn(Vision.position, Vision.forward, muzzle, bullet);
                    effectsController.MuzzleFlashSpawn(weaponData.MuzzleFlashPrefab);
                    effectsController.ShellSpawn(weaponData.ShellPrefab);
                    effectsController.SoundPlay(weaponData.RandomShotSound);
                    ShotProcess?.Invoke();
                }
            }
            yield return null;
        } while (input.Invoke());
        ShotEnd?.Invoke();
        animator.SetBool(Trigger_Bool, false);
    }
}
