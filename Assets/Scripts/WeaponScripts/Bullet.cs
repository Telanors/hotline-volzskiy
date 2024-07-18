using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private BulletData data;
    private Vector3 startPosition;
    private Vector3 lastFramePosition;
    private Vector3 moveDirection;
    private void Start()
    {
        if (moveDirection == Vector3.zero) Destroy(gameObject);
        startPosition = transform.position;
        lastFramePosition = startPosition;
    }
    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= data.MaxDistance) Destroy(gameObject);
        transform.Translate(moveDirection * data.Speed * Time.deltaTime, Space.World);
    }
    private void LateUpdate()
    {
        float distance = Vector3.Distance(lastFramePosition, transform.position);
        if (Physics.Raycast(lastFramePosition, moveDirection, out RaycastHit hit, distance, ~data.IgnoreMask, QueryTriggerInteraction.Ignore))
        {
            var obj = Instantiate(data.RandomBulletHolePrefab, hit.point, Quaternion.identity, hit.transform);
            obj.transform.forward = hit.normal;
            transform.position = hit.point;
            Destroy(this);
            if (hit.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(data.Damage);
            }
            if (hit.transform.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddForceAtPosition(moveDirection * data.ImpactForce, hit.point, ForceMode.VelocityChange);
            }
            return;
        }
        lastFramePosition = transform.position;
    }
    public void Setup(Vector3 moveDirection, BulletData data) 
    { 
        this.moveDirection = moveDirection;
        this.data = data;
    }
}
