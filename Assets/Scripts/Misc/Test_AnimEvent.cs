using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTANIMEVENT : MonoBehaviour
{
    public LayerMask mask;
    public Transform mainCamera;
    public float dropForceUp;
    public float dropForceRight;
    public Transform shellAnchor;
    public GameObject shell;

    public GameObject bullethole;
    public AudioSource shotSound;
    public GameObject muzzleFlash;
    public Transform muzzlefalshanchor;
    public void MuzzleFlashSpawn()
    {
        Transform zuzzle = Instantiate(muzzleFlash, muzzlefalshanchor).transform;
        zuzzle.localPosition = Vector3.zero;
        zuzzle.localEulerAngles = Vector3.zero;
    }
    public void Sound()
    {
        shotSound.pitch = Random.Range(1f, 1.5f);
        shotSound.PlayOneShot(shotSound.clip);
    }
    public void ShellDrop()
    {
        Rigidbody shellrb = Instantiate(shell).GetComponent<Rigidbody>();
        shellrb.transform.position = shellAnchor.position;
        shellrb.transform.eulerAngles = shellAnchor.eulerAngles;
        shellrb.AddForce(-shellrb.transform.forward * dropForceRight + shellrb.transform.up * dropForceUp, ForceMode.Impulse);
        shellrb.AddTorque(Random.Range(2, 3), Random.Range(2, 3), Random.Range(2, 3));
    }
    public void BulletHole()
    {
        RaycastHit arayhit;
        if (Physics.Raycast(mainCamera.position, mainCamera.forward, out arayhit, 50f, mask))
        {
            Transform bullet = Instantiate(bullethole).transform;
            bullet.forward = arayhit.normal;
            bullet.position = arayhit.point - bullet.forward * 0.01f;
        }
    }
}
