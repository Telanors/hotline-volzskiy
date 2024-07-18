using UnityEngine;
using System.Linq;
using System.Collections;
using System;
using UnityEngine.Events;

public class AIEquipment : MonoBehaviour
{
    [SerializeField] private Transform weaponAnchor;
    [SerializeField] private AdvanceWeapon currentWeapon;
    public AdvanceWeapon CurrentWeapon => currentWeapon;
    public AdvanceWeapon defaultWeapon;
    public IEntityVision Vision { get; set; }
    public bool HasWeapon => currentWeapon != null;
    private bool AttackReady;
    public UnityEvent ShotStart = new UnityEvent();
    public UnityEvent ShotEnd = new UnityEvent();
    private void OnShotStart()
    {
        AttackReady = true;
        ShotStart?.Invoke();
    }
    private void OnShotEnd()
    {
        AttackReady = false;
        ShotEnd?.Invoke();
    }
    public void UseWeapon(AIEnemy AIEnemy)
    {
        if(currentWeapon == null) return;
        StartCoroutine(MakeFireDelay(AIEnemy));
        if(currentWeapon.currentAmmo <= 0)
        {
            currentWeapon.currentAmmo = currentWeapon.WeaponData.MaxMagazineSize;
        }
        currentWeapon.MakeShot(() => AttackReady);
    }
    public void PickUp(AdvanceWeapon weapon)
    {
        if (currentWeapon != null) return;
        weapon = Instantiate(weapon);
        currentWeapon = weapon;
        currentWeapon.ShotStart.AddListener(OnShotStart);
        currentWeapon.ShotEnd.AddListener(OnShotEnd);
        currentWeapon.ToggleColleders(false);
        currentWeapon.SetRigidBodyParametrs(true, RigidbodyInterpolation.None, CollisionDetectionMode.Discrete);
        currentWeapon.SetParentTransform(weaponAnchor);
        currentWeapon.SetLayer(gameObject.layer);
        currentWeapon.SetWeaponBullet(currentWeapon.WeaponData.AIBullet);
        currentWeapon.Vision = Vision;
        currentWeapon.transform.localScale = Vector3.one;
        currentWeapon.OnEquip.Invoke();
    }
    public void Drop(Vector3 direction)
    {
        if (currentWeapon == null || currentWeapon.GetType() == defaultWeapon.GetType()) return;
        StopAllCoroutines();
        var weapon = currentWeapon;
        currentWeapon = null;
        weapon.ShotStart.RemoveListener(OnShotStart);
        weapon.ShotEnd.RemoveListener(OnShotEnd);
        weapon.ToggleColleders(true);
        weapon.SetRigidBodyParametrs(false, RigidbodyInterpolation.Interpolate, CollisionDetectionMode.ContinuousDynamic);
        weapon.UnParent();
        weapon.SetLayer(weapon.WeaponData.DefaultLayerMask);
        weapon.OnUnEquip?.Invoke();
        weapon.transform.localScale = Vector3.one;
        weapon.RigidBody.AddForce(direction * 10.0f, ForceMode.VelocityChange);
    }
    private IEnumerator MakeFireDelay(AIEnemy AIEnemy)
    {
        yield return new WaitForSeconds(AIEnemy.firingTime);
        AttackReady = false;
        AIEnemy.attackTimer = 0.0f;
    }
}
