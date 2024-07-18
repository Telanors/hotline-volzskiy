using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Events;
public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI ammoCounter;
    protected PlayerCameraManager playerCamera;
    public PlayerCameraManager PlayerCamera { get => playerCamera; }
    [SerializeField] protected WeaponData weaponData;
    public WeaponData WeaponData { get => weaponData; }
    [SerializeField] protected WeaponDummy weaponDummy;
    public WeaponDummy WeaponDummy { get => weaponDummy; }

    protected WeaponEffectsController weaponEffectsController;
    protected AudioSource audioSource;
    protected Animator animator;
    protected RaycastHit hitRaycast;
    protected int currentAmmo;
    protected bool readyToShoot = true;
    
    public UnityEvent ShotStart = new UnityEvent();
    public UnityEvent ShotProcess = new UnityEvent();
    public UnityEvent ShotEnd = new UnityEvent();

    public string Shot_Trigger = "Shot";
    public string Empty_Bool = "Empty";
    public string Trigger_Bool = "Trigger";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        weaponEffectsController = GetComponent<WeaponEffectsController>();
        audioSource = GetComponentInParent<AudioSource>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    protected abstract IEnumerator Shot(System.Func<bool> input);
    protected virtual void ShotImpact()
    {
        //bool madeImpact = true;
        //float maxDistance = WeaponData.Bullet.MaxDistance;
        //int bulletCount = weaponData.Bullet.SpawnCount;
        //RaycastHit[] hits = new RaycastHit[bulletCount];
        //for (int i = 0; i < bulletCount; i++)
        //{
        //    float spread = weaponData.Bullet.Spread;
        //    float x = Random.Range(-spread, spread);
        //    float y = Random.Range(-spread, spread);
        //    Vector3 totalSpread = playerCamera.transform.TransformDirection(new Vector3(x, y, 0.0f));
        //    Vector3 direction = (playerCamera.forward + totalSpread).normalized;
        //    Ray ray = new Ray(playerCamera.position, direction);
        //    if (!Physics.Raycast(ray, out hits[i], maxDistance, weaponData.Bullet.HitMask, QueryTriggerInteraction.Ignore)) 
        //    {
        //        madeImpact = false;
        //        hits[i].point = ray.GetPoint(maxDistance);
        //    }
        //    else
        //    {
        //        if (hits[i].transform.TryGetComponent(out IDamageable damageable))
        //        {
        //            madeImpact = false;
        //            damageable.Damage(weaponData.Damage);
        //        }
        //        if (hits[i].transform.TryGetComponent(out Rigidbody rigidbody))
        //        {
        //            rigidbody.AddForceAtPosition(playerCamera.forward * 20.0f, hits[i].point, ForceMode.VelocityChange);
        //        }
        //    }
        //}
        //weaponEffectsController.BulletSpawn(weaponData.Bullet, hits, madeImpact);
        //weaponEffectsController.SoundPlay(audioSource, WeaponData.RandomShotSound);
        //weaponEffectsController.ShellSpawn();
        //weaponEffectsController.MuzzleFlashSpawn();
    }
    public bool IsEmpty() => currentAmmo <= 0.0f;
    protected void AmmoCounterUpdate() => ammoCounter.text = currentAmmo.ToString();
    protected IEnumerator FireRateDelay()
    {
        readyToShoot = false;
        yield return new WaitForSeconds(1 / weaponData.FireRate);
        readyToShoot = true;
    }
    public void MakeShot(System.Func<bool> input)
    {
        if (!readyToShoot) return;
        StartCoroutine(Shot(input));
    }
    public void OnDrop(WeaponDummy dummy)
    {
        ShotEnd.RemoveAllListeners();
        ShotProcess.RemoveAllListeners();
        ShotStart.RemoveAllListeners();
        dummy.SetParametrs(currentAmmo);
    }
    public virtual void OnPickUp(WeaponDummy dummy, PlayerCameraManager playerCamera)
    {
        this.playerCamera = playerCamera;
        currentAmmo = dummy.currentAmmo;
        animator.SetBool(Empty_Bool, currentAmmo == 0.0f);
        AmmoCounterUpdate();
    }  
}
