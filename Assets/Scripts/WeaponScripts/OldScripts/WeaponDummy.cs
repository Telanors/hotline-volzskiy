using UnityEngine;
using TMPro;
using System.Collections.Generic;
public class WeaponDummy : MonoBehaviour
{
    [SerializeField]
    private Material selectedMaterial;
    [SerializeField]
    private SkinnedMeshRenderer[] meshes;
    [SerializeField]
    private TextMeshProUGUI ammoCounter;
    [SerializeField]
    private Weapon weaponPrefab;
    public Weapon WeaponPrefab { get => weaponPrefab; }

    public int currentAmmo;
    public AudioClip fallSound;

    private Material[] normalMaterials;
    private Rigidbody rigidBody;
    private Animator animator;
    private AudioSource audioSource;
    private float torqueForce = 2.0f;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        if(audioSource != null)
            audioSource.clip = fallSound;
    }
    public void SetParametrs(int ammo)
    {
        currentAmmo = ammo;
        animator.SetBool("Empty", currentAmmo == 0);
        if (ammoCounter == null) return;
        ammoCounter.text = ammo.ToString();
    }
    public void Trow(Vector3 currentVelocity, Vector3 direction, Vector3 torque, float force)
    {
        rigidBody.velocity = currentVelocity;
        rigidBody.AddForce((direction + Vector3.up * 0.1f) * force, ForceMode.VelocityChange);
        rigidBody.AddTorque(torque * torqueForce, ForceMode.VelocityChange);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.transform.CompareTag("Small") && collision.relativeVelocity.magnitude >= 5.0f)
        {
            if (audioSource.isPlaying) return;
            if(collision.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(1000.0f);
            }
            audioSource.pitch = Random.Range(1.0f, 1.05f);
            audioSource.Play();
        }
    }
    public void SetPickUpColor()
    {
        if (meshes[0].material.color == Color.red) return;
        List<Material> materials = new List<Material>();
        foreach (var mesh in meshes)
        {
            materials.Add(mesh.material);
            mesh.material = selectedMaterial;
        }
        normalMaterials = materials.ToArray();
    }
    public void SetNormalColor()
    {
        if (meshes[0].material.color == Color.white) return;
        for (int i = 0; i < meshes.Length; i++)
        {
            meshes[i].material = normalMaterials[i];
        }
        normalMaterials = null;
    }
}
