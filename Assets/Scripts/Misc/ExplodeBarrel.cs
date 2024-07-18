using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ExplodeBarrel : MonoBehaviour, IDamageable
{
    public float radius;
    public float force;

    public void Damage(float damage)
    {
        Detonate();
    }
    private void Detonate()
    {
        Debug.Log("Boom");
        //Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        //foreach (var collider in colliders)
        //{
        //    IDamageable damageable = collider.transform.GetComponent<IDamageable>();
        //    if (damageable != null)
        //    {
        //        damageable.Damage();
        //    }
        //    //Rigidbody rigidBody = collider.attachedRigidbody;
        //    //if (rigidBody != null)
        //    //{
        //    //    rigidBody.AddExplosionForce(force, transform.position, radius);
        //    //}
        //}
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
