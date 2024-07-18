using System.Collections;
using UnityEngine;

public class WeaponEffectsController : MonoBehaviour
{
    [SerializeField] private Transform muzzleAnchor;
    [SerializeField] private Transform shellAnchor;

    public int shellDropDirection;
    public float shellDropForce;
    public float shellDropTorque;

    private AudioSource source;
    private Vector3 lastFramePosition;
    private void Start()
    {
        source = GetComponent<AudioSource>();
    }
    private void LateUpdate()
    {
        lastFramePosition = transform.position;
    }
    public void SoundPlay(AudioClip clip)
    {
        source.pitch = Random.Range(1.0f, 1.1f);
        source.PlayOneShot(clip);
    }
    public void MuzzleFlashSpawn(GameObject muzzleFlash)
    {
        var muzzleFlashGO = Instantiate(muzzleFlash, muzzleAnchor.position, muzzleAnchor.rotation, muzzleAnchor);
        var objects = muzzleFlashGO.GetComponentsInChildren<Transform>(true);
        foreach(var obj in objects)
        {
            obj.gameObject.layer = gameObject.layer;
        }
        Destroy(muzzleFlashGO, 2.5f);
    }
    public void ShellSpawn(GameObject shell)
    {
        Rigidbody rb = Instantiate(shell, shellAnchor.position, shellAnchor.rotation).GetComponent<Rigidbody>();

        Vector3 currectVelocity = (transform.position - lastFramePosition) / Time.deltaTime;
        rb.velocity = currectVelocity * 0.85f;

        rb.maxAngularVelocity = Mathf.Infinity;
        rb.AddForce((shellAnchor.right * shellDropDirection + shellAnchor.up) * shellDropForce, ForceMode.VelocityChange);
        rb.AddTorque(new Vector3(Random.Range(0.1f, 1.0f), Random.Range(0.1f, 1.0f), Random.Range(0.1f, 1.0f)) * shellDropTorque, ForceMode.VelocityChange);
    }
    //public void BulletSpawn(BulletData bullet, RaycastHit[] hits)
    //{
    //    int spawnCount = bullet.SpawnCount;
    //    for (int i = 0; i < spawnCount; i++)
    //    {
    //        StartCoroutine(TrailMove(bullet, hits[i]));
    //    }
    //}
    //private IEnumerator TrailMove(BulletData bullet, RaycastHit hit)
    //{
    //    TrailRenderer trail = Instantiate(bullet.Trail, muzzleAnchor.position, Quaternion.identity);
    //    Vector3 startPosition = trail.transform.position;
    //    Vector3 endPosition = hit.point;
    //    float distance = Vector3.Distance(startPosition, endPosition);
    //    float remainingDistance = distance;
    //    while(remainingDistance > 0)
    //    {
    //        trail.transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.Clamp01(1 - (remainingDistance / distance)));
    //        remainingDistance -= bullet.Speed * Time.deltaTime;
    //        yield return null;
    //    }
    //    trail.transform.position = endPosition;
    //    if (hit.collider != null)
    //    {
    //        Instantiate(bullet.RandomBulletHolePrefab, endPosition, Quaternion.LookRotation(hit.normal), hit.transform);
    //    }
    //    yield return null ;
    //}
}
