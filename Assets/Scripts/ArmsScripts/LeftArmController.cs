using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftArmController : MonoBehaviour
{
    [SerializeField] private PlayerCameraManager playerCamera;
    [SerializeField] private InputsController inputs;
    [SerializeField] private AdvanceWeapon currentWeapon;
    [SerializeField] private Transform weaponAnchor;

    private bool actionAnimationDone = true;
    private Animator animator;
    private Coroutine weaponCoroutine;

    public string Attack_Trigger = "Attack";
    public int UseWeapon_Action = "UseWeapon_Action".GetHashCode();
    private void Start()
    {
        inputs.meleeAction.started += context =>
        {
            UseWeapon(currentWeapon);
        };
        SetupWeapon(currentWeapon);
        animator = GetComponent<Animator>();
    }
    public void ActivateTrigger(string name)
    {
        animator.SetTrigger(name);
    }
    public void UseWeapon(AdvanceWeapon weapon)
    {
        if (weaponCoroutine != null) return;
        weaponCoroutine = StartCoroutine(WaitingForAction(() => weapon?.MakeShot(inputs.fireAction.IsPressed)));
    }
    public void SetupWeapon(AdvanceWeapon weapon)
    {
        var targetWeapon = GameObject.Find(weapon.name);
        if (targetWeapon == null)
        {
            weapon = Instantiate(weapon, weaponAnchor.position, weaponAnchor.rotation, weaponAnchor);
        }
        currentWeapon = weapon;
        currentWeapon.ToggleColleders(false);
        currentWeapon.SetRigidBodyParametrs(true, RigidbodyInterpolation.None, CollisionDetectionMode.Discrete);
        currentWeapon.SetParentTransform(weaponAnchor);
        currentWeapon.SetLayer(gameObject.layer);
        currentWeapon.Vision = playerCamera;
        currentWeapon.SetWeaponBullet(currentWeapon.WeaponData.Bullet);
        currentWeapon.OnEquip?.Invoke();
        currentWeapon.ShotStart.AddListener(() => ActivateTrigger(Attack_Trigger));
    }
    public void AnimationEndEvent() => actionAnimationDone = true;
    public void AnimationStartEvent() => actionAnimationDone = false;
    private IEnumerator WaitingForAction(Action action)
    {
        do
        {
            yield return null;
        }
        while (!actionAnimationDone);
        action.Invoke();
        weaponCoroutine = null;
    }
}
