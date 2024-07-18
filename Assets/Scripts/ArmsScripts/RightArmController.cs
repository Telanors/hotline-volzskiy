using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using Random = UnityEngine.Random;
[RequireComponent(typeof(Animator))]
public class RightArmController : MonoBehaviour
{
    //private static AdvanceWeapon[] weapons;
    [SerializeField] private Player player;
    [SerializeField] private InputsController inputs;
    [SerializeField] private Transform weaponAnchor;
    [SerializeField] private Vector3 defaultPosition;

    [SerializeField] private PlayerCameraManager playerCamera;
    public PlayerCameraManager PlayerCamera => playerCamera;
    [SerializeField] private ProceduralArmAnimator proceduralAnimator;
    public ProceduralArmAnimator ProceduralAnimator => proceduralAnimator;
    public Animator Animator { get; private set; }
    private AdvanceWeapon currentWeapon;
    public AdvanceWeapon CurrentWeapon => currentWeapon;

    [HideInInspector] public UnityEvent OnPickUp = new UnityEvent();
    [HideInInspector] public UnityEvent OnDrop = new UnityEvent();

    private AudioSource audioSource;
    private Dictionary<int, Coroutine> waitDictionary = new Dictionary<int, Coroutine>();
    private ISelectable selected;
    private bool actionAnimationDone = true;

    public AudioClip pickUpSound;
    public AudioClip dropSound;
    public float autoPickUpRange = 3.0f;
    public float dropForce = 5.0f;
    public float pickUpDistance = 50.0f;
    public float maxDropRaycastDistance = 20.0f;
    public LayerMask ignoreMask;

    public string Shot_Bool = "Shot";
    public string PickUp_Trigger = "PickUp";
    public string Select_Bool = "Select";
    public string Drop_Trigger = "Drop";
    public string EmptyArm_Bool = "EmptyArm";
    public int PickUp_Action = "PickUp_Action".GetHashCode();
    public int UseWeapon_Action = "UseWeapon_Action".GetHashCode();
    private void Awake()
    {
        Animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        proceduralAnimator = GetComponent<ProceduralArmAnimator>();
    }
    private void FixedUpdate()
    {
        //if(currentWeapon == null)
        //{
        //    AdvanceWeapon wp = weapons.FirstOrDefault(x => x.transform.parent == null && Vector3.Distance(transform.position, x.transform.position) <= autoPickUpRange);
        //    if(wp != null)
        //    {
        //        PickUpAction(wp);
        //    }
        //}
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, pickUpDistance, ~ignoreMask))
        {
            if(hit.transform.TryGetComponent(out ISelectable selectable))
            {
                if (selected != null)
                {
                    selected.UnSelect();
                }
                Animator.SetBool(Select_Bool, true);
                selected = selectable;
                selected.Select();
            }
            else
            {
                if (selected != null)
                {
                    Animator.SetBool(Select_Bool, false);
                    selected.UnSelect();
                    selected = null;
                }
            }
        }
        else
        {
            if(selected != null)
            {
                Animator.SetBool(Select_Bool, false);
                selected.UnSelect();
                selected = null;
            }
        }
    }
    private void Start()
    {
        //weapons = FindObjectsOfType<AdvanceWeapon>();
        inputs.fireAction.started += context =>
        {
            UseWeapon(currentWeapon);
        };
        inputs.pickUpAction.started += contex =>
        {
            PickUp();
        };
        inputs.dropAction.started += contex =>
        {
            DropWeapon(); 
        };
    }
    public void UseWeapon(AdvanceWeapon weapon) 
    {
        ActionCoroutineCheck(UseWeapon_Action);
        waitDictionary.Add(UseWeapon_Action, StartCoroutine(WaitingForAction(() => weapon?.MakeShot(inputs.fireAction.IsPressed), UseWeapon_Action)));
    }
    public void PickUp()
    {
        if (selected == null || !actionAnimationDone || selected is not AdvanceWeapon weapon) return;
        DropWeapon();
        ActionCoroutineCheck(PickUp_Action);
        waitDictionary.Add(PickUp_Action, StartCoroutine(WaitingForAction(() => PickUpAction(weapon), PickUp_Action)));
        selected.UnSelect();
    }
    public void PickUpAction(AdvanceWeapon weapon)
    {
            Animator.SetBool(Shot_Bool, false);
            Animator.SetBool(EmptyArm_Bool, false);
            Animator.SetTrigger(PickUp_Trigger);
            proceduralAnimator.StateWeightChange("FingerLayer", 1.0f);
            SoundPlay(pickUpSound);
            SetupWeapon(weapon); 
    }
    public void SetupWeapon(AdvanceWeapon weapon)
    {
        currentWeapon = weapon;
        currentWeapon.ToggleColleders(false);
        currentWeapon.SetRigidBodyParametrs(true, RigidbodyInterpolation.None, CollisionDetectionMode.Discrete);
        currentWeapon.SetParentTransform(weaponAnchor);
        currentWeapon.SetLayer(gameObject.layer);
        currentWeapon.Vision = playerCamera;
        currentWeapon.SetWeaponBullet(currentWeapon.WeaponData.Bullet);
        currentWeapon.OnEquip?.Invoke();
        OnPickUp?.Invoke();
        currentWeapon.ShotStart.AddListener(OnShotStart);
        currentWeapon.ShotProcess.AddListener(OnShotProcess);
        currentWeapon.ShotEnd.AddListener(OnShotEnd);
        currentWeapon.transform.localScale = Vector3.one;
        transform.localPosition = currentWeapon.WeaponData.DefaultPosition;
    }
    public void DropWeapon()
    {
        if (currentWeapon == null || !actionAnimationDone) return;
        currentWeapon.ShotStart.RemoveListener(OnShotStart);
        currentWeapon.ShotProcess.RemoveListener(OnShotProcess);
        currentWeapon.ShotEnd.RemoveListener(OnShotEnd);
        transform.localPosition = defaultPosition;
        proceduralAnimator.StateWeightChange("FingerLayer", 0.0f);
        Animator.SetBool(EmptyArm_Bool, true);
        Animator.SetBool(Shot_Bool, false);
        Animator.SetTrigger(Drop_Trigger);
        SoundPlay(dropSound);
        Vector3 dropDirection;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out RaycastHit hit, maxDropRaycastDistance, ~ignoreMask))
        {
            dropDirection = (hit.point - weaponAnchor.position).normalized;
        }
        else
        {
            dropDirection = playerCamera.forward;
        }

        Vector3 torquepDirection = new Vector3(UnityEngine.Random.Range(0.15f, 1.0f), UnityEngine.Random.Range(0.15f, 1.0f), UnityEngine.Random.Range(0.15f, 1.0f));
        Vector3 currectVelocity = player.Orientation.forward * player.VelocityMagnitude;
        currentWeapon.UnParent();
        currentWeapon.ToggleColleders(true);
        currentWeapon.SetRigidBodyParametrs(false, RigidbodyInterpolation.Interpolate, CollisionDetectionMode.ContinuousDynamic);
        currentWeapon.SetLayer(currentWeapon.WeaponData.DefaultLayerMask);
        currentWeapon.Trow(currectVelocity, dropDirection, torquepDirection, dropForce);
        currentWeapon.OnUnEquip?.Invoke();
        OnDrop?.Invoke();
        currentWeapon.transform.localScale = Vector3.one;
        currentWeapon = null;
    }
    private void SoundPlay(AudioClip clip)
    {
        audioSource.pitch = Random.Range(1.0f, 1.1f);
        audioSource.PlayOneShot(clip);
    }
    private void OnShotStart() => Animator?.SetBool(Shot_Bool, true);
    private void OnShotProcess() => proceduralAnimator.DoArmRecoil(currentWeapon.WeaponData);
    private void OnShotEnd() => Animator?.SetBool(Shot_Bool, false);
    public void AnimationEndEvent() => actionAnimationDone = true;
    public void AnimationStartEvent() => actionAnimationDone = false;

    private void ActionCoroutineCheck(int actionHash)
    {
        if (waitDictionary.ContainsKey(actionHash))
        {
            StopCoroutine(waitDictionary[actionHash]);
            waitDictionary.Remove(actionHash);
        }
    }
    private IEnumerator WaitingForAction(Action action, int actionHash)
    {
        do
        {
            yield return null;
        }
        while (!actionAnimationDone);
        action.Invoke();
        waitDictionary.Remove(actionHash);
    }
}
