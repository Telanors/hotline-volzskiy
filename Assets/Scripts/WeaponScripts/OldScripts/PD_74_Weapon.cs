using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PD_74_Weapon : Weapon
{
    protected override IEnumerator Shot(System.Func<bool> input)
    {
        StartCoroutine(FireRateDelay());
        ShotStart?.Invoke();
        animator.SetTrigger(Shot_Trigger);
        animator.SetBool(Trigger_Bool, true);
        //weaponEffectsController.SoundPlay(audioSource, WeaponData.TriggerSound);
        if (!IsEmpty())
        {
            ShotProcess?.Invoke();
            ShotImpact();
            if (WeaponData.MaxMagazineSize > 0)
            {
                currentAmmo--;
                animator.SetBool(Empty_Bool, IsEmpty());
                //weaponEffectsController.SoundPlay(audioSource, WeaponData.ShutterDelaySound);
                AmmoCounterUpdate();
            }
        }
        while (input.Invoke())
        {
            yield return null;
        }
        ShotEnd?.Invoke();
        animator.SetBool(Trigger_Bool, false);
    }
}
