using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class DoorBreaker : MonoBehaviour
{
    public float maxSpeedForBreak;
    
    public Rigidbody door;
    public Rigidbody pieces;
    public AudioClip clip;

    private NavMeshObstacle meshObstacle;
    private AudioSource source;

    private void Start()
    {
        meshObstacle = GetComponent<NavMeshObstacle>();
        source = GetComponent<AudioSource>();
        meshObstacle.carving = true;
        SwitchRigidBodyKinematic(true);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent(out Player player) && collision.relativeVelocity.magnitude > maxSpeedForBreak)
        {
            SwitchRigidBodyKinematic(false);
            if (TryGetComponent(out NoiseSource noiseSource))
            {
                noiseSource.DoNoise();
            }
            meshObstacle.carving = false;
            player.LeftArmController.ActivateTrigger("DoorBreak");
            source.PlayOneShot(clip);
            Vector3 direction = player.Orientation.forward * 20;
            door.AddForce(direction, ForceMode.VelocityChange);
            pieces.AddForce(direction, ForceMode.VelocityChange);
            Destroy(this);
            Destroy(source, 5.0f);
        }
    }
    private void SwitchRigidBodyKinematic(bool value)
    {
        door.isKinematic = value;
        pieces.isKinematic = value;
    }
}
