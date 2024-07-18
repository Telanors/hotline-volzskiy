using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class PPL_Weapon : Weapon
{
    protected override IEnumerator Shot(System.Func<bool> input)
    {
        animator.SetBool(Trigger_Bool, true);
        ShotStart?.Invoke();
        //weaponEffectsController.SoundPlay(audioSource, WeaponData.TriggerSound);
        do
        {
            if (readyToShoot)
            {
                StartCoroutine(FireRateDelay());
                animator.SetTrigger(Shot_Trigger);
                if (!IsEmpty())
                {
                    ShotProcess?.Invoke();
                    ShotImpact();
                    if (WeaponData.MaxMagazineSize > 0)
                    {
                        currentAmmo--;
                        if (IsEmpty())
                        {
                            animator.SetBool(Empty_Bool, true);
                            //weaponEffectsController.SoundPlay(audioSource, WeaponData.ShutterDelaySound);
                        }
                        AmmoCounterUpdate();
                    }
                }
            }
            yield return null;
        } while (input.Invoke());
        ShotEnd?.Invoke();
        animator.SetBool(Trigger_Bool, false);
    }
}
