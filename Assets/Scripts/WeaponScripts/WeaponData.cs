using UnityEngine;
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon/New WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Characteristics")]
    [SerializeField] private new string name;
    [SerializeField] private string description;
    public string Name { get => name; }
    public string Description { get => description; }
    [Header("Weapon Parameters")]
    [SerializeField] private float maxRaycastDistance;
    [SerializeField, Range(0.0f, 32.0f)] private float fireRate;
    [SerializeField] private int maxMagazineSize;
    [SerializeField] private int defaultLayerMask;
    public float FireRate { get => fireRate; }
    public float MaxRaycastDistance { get => maxRaycastDistance; }
    public int MaxMagazineSize { get => maxMagazineSize; }
    public int DefaultLayerMask { get => defaultLayerMask; }
    [Header("Weapon Recoil Animation Parametrs")]
    [SerializeField] private float yPositionRecoil= 0.01f;
    [SerializeField] private float zPositionRecoil = -0.05f;
    [SerializeField] private float xRotationRecoil = -3.0f;
    [SerializeField] private float recoilPerlinNoiseMultuply = 5;
    [SerializeField, Range(0, 100)] private float recoilTimeMultiply = 2;
    public AnimationCurve YAnimationCurve;
    public AnimationCurve ZAnimationCurve;
    public float YPositionRecoil { get => yPositionRecoil; }
    public float ZPositionRecoil { get => zPositionRecoil; }
    public float XRotationRecoil { get => xRotationRecoil; }
    public float RecoilPerlinNoiseMultuply { get => recoilPerlinNoiseMultuply; }
    public float RecoilTimeMultiply { get => recoilTimeMultiply; }
    [Header("Weapon Recoil Parametrs")]
    [SerializeField] private WeaponRecoilParametrs recoilParametrs;
    public WeaponRecoilParametrs RecoilParametrs { get => recoilParametrs; }
    [Header("Weapon Effects")]
    [SerializeField] private Vector3 defaultPosition;
    [SerializeField] private GameObject shellPrefab;
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private BulletData bullet;
    [SerializeField] private BulletData AIbullet;
    public Vector3 DefaultPosition { get => defaultPosition; }    
    public GameObject ShellPrefab { get => shellPrefab; }
    public GameObject MuzzleFlashPrefab { get => muzzleFlashPrefab; }
    public BulletData Bullet { get => bullet; }
    public BulletData AIBullet { get => AIbullet; }
    [SerializeField] private AudioClip[] shotSounds;
    [SerializeField] private AudioClip shutterDelaySound;
    [SerializeField] private AudioClip triggerSound;
    [SerializeField] private AudioClip fallSound;
    public AudioClip[] ShotSounds { get => shotSounds; }
    public AudioClip RandomShotSound { get => shotSounds[Random.Range(0, shotSounds.Length)]; }
    public AudioClip ShutterDelaySound { get => shutterDelaySound; }
    public AudioClip FallSound { get => fallSound; }
    public AudioClip TriggerSound { get => triggerSound; }
}
