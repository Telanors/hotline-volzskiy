using System.Collections;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

public abstract class AdvanceWeapon : MonoBehaviour, ISelectable
{
    [SerializeField] private Collider[] colliders;
    [SerializeField] private SkinnedMeshRenderer[] meshes;
    [SerializeField] protected TextMeshProUGUI ammoCounter;
    [SerializeField] protected Transform muzzle;
    public Transform Muzzle => muzzle;
    [SerializeField] protected WeaponData weaponData;
    public WeaponData WeaponData => weaponData;
    protected WeaponEffectsController effectsController;
    protected BulletData bullet;
    protected AudioSource audioSource;
    protected Animator animator;
    protected bool readyToShoot = true;

    [HideInInspector] public UnityEvent ShotStart = new UnityEvent();
    [HideInInspector] public UnityEvent ShotProcess = new UnityEvent();
    [HideInInspector] public UnityEvent ShotEnd = new UnityEvent();

    [HideInInspector] public IEntityVision Vision { get; set; }
    [HideInInspector] public UnityEvent OnEquip = new UnityEvent();
    [HideInInspector] public UnityEvent OnUnEquip = new UnityEvent();
    public Rigidbody RigidBody { get; protected set; }
    public bool Equipped { get; private set; }
    public int currentAmmo;
    public Material selectMaterial;
    private Material[] standartMaterials;

    public string Shot_Trigger = "Shot";
    public string Empty_Bool = "Empty";
    public string Trigger_Bool = "Trigger";
    private void Awake()
    {
        name = GetInstanceID().ToString();
        if(TryGetComponent(out NoiseSource noiseSource))
        {
            ShotProcess.AddListener(noiseSource.DoNoise);
        }
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        RigidBody = GetComponent<Rigidbody>();
        colliders = GetComponents<Collider>();
        effectsController = GetComponent<WeaponEffectsController>();
        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        OnEquip.AddListener(() => 
        { 
            Equipped = true;
            readyToShoot = true;
        });
        OnUnEquip.AddListener(() => 
        {
            Equipped = false;
            StopAllCoroutines();
        });
    }
    protected abstract IEnumerator Shot(Func<bool> input);
    public void SetWeaponBullet(BulletData bullet)
    {
        this.bullet = bullet;
    }
    public bool IsEmpty() 
    {
        if (weaponData.MaxMagazineSize <= 0) return false;
        return currentAmmo <= 0.0f; 
    }
    public void SubMagazineAmmo()
    {
        currentAmmo--;
        animator.SetBool(Empty_Bool, IsEmpty());
        AmmoCounterTextUpdate(currentAmmo);
    }
    protected void AmmoCounterTextUpdate(int currentAmmo) => ammoCounter.text = currentAmmo <= 0.0f ? "0" : currentAmmo.ToString();
    protected IEnumerator FireRateDelay()
    {
        readyToShoot = false;
        yield return new WaitForSeconds(1 / weaponData.FireRate);
        readyToShoot = true;
    }
    public void MakeShot(Func<bool> input)
    {
        if (!readyToShoot) return;
        StartCoroutine(Shot(input));
    }
    public void SetLayer(int layer)
    {
        var childrens = GetComponentsInChildren<Transform>(true);
        foreach (var children in childrens)
        {
            children.gameObject.layer = layer;
        }
    }
    public void UnParent()
    {
        transform.parent = null;
    }
    public void SetParentTransform(Transform parent)
    {
        transform.position = parent.position;
        transform.rotation = parent.rotation;
        transform.parent = parent;
    }
    public void SetRigidBodyParametrs(bool isKinematic, RigidbodyInterpolation interpolation, CollisionDetectionMode detectionMode)
    {
        if (RigidBody == null) return;
        RigidBody.isKinematic = isKinematic;
        RigidBody.interpolation = interpolation;
        RigidBody.collisionDetectionMode = detectionMode;
    }
    public void ToggleColleders(bool enable)
    {
        foreach(var collider in colliders)
        {
            collider.enabled = enable;
        }
    }
    public void Trow(Vector3 currentVelocity, Vector3 direction, Vector3 torque, float force)
    {
        RigidBody.velocity = currentVelocity;
        RigidBody.AddForce((direction + Vector3.up * 0.1f) * force, ForceMode.VelocityChange);
        RigidBody.AddTorque(torque * force, ForceMode.VelocityChange);
    }
    public void Select()
    {
        if (meshes[0].material.color == Color.red) return;
        List<Material> materials = new List<Material>();
        foreach (var mesh in meshes)
        {
            materials.Add(mesh.material);
            mesh.material = selectMaterial;
        }
        standartMaterials = materials.ToArray();
        InfoWeaponPanel.Instance?.OnPanel(transform, weaponData.Name, weaponData.Description);
    }
    public void UnSelect()
    {
        if (meshes[0].material.color == Color.white) return;
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].material = standartMaterials[i];
        }
        standartMaterials = null;
        InfoWeaponPanel.Instance?.OffPanel();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= 7.0f)
        {
            if (collision.transform.TryGetComponent(out IStunable stunable)) stunable.Stun();
            if (collision.transform.CompareTag("Small")) return;
            audioSource.pitch = UnityEngine.Random.Range(1.0f, 1.05f);
            audioSource.PlayOneShot(weaponData.FallSound);
        }
    }
}
