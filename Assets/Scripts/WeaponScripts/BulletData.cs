using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Bullet", menuName = "Weapon/New BulletData")]
public class BulletData : ScriptableObject
{
    [SerializeField] private LayerMask ignoreMask;
    [SerializeField] private GameObject[] bulletHoleprefabs;
    [SerializeField] private Bullet prefab;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float impactForce;
    [SerializeField] private float spread;
    [SerializeField] private float maxDistance;
    [SerializeField] private int spawnCount;
    public LayerMask IgnoreMask { get => ignoreMask; }
    public GameObject RandomBulletHolePrefab { get => bulletHoleprefabs[Random.Range(0, bulletHoleprefabs.Length)]; }
    public Bullet Prefab { get => prefab; }
    public float MaxDistance { get => maxDistance; }
    public float Speed { get => speed; }
    public float Damage { get => damage; }
    public float ImpactForce { get => impactForce; }
    public float Spread { get => spread; }
    public int SpawnCount { get => spawnCount; }
}
