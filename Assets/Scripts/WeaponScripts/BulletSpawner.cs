using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletSpawner
{
    public static void HitScanShot(Vector3 position, Vector3 direction, BulletData data)
    {
        Ray ray = new Ray(position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, data.MaxDistance, ~data.IgnoreMask))
        {
            if (hit.transform.TryGetComponent(out IDamageable damageable))
            {
                damageable.Damage(data.Damage);
            }
            if (hit.transform.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.AddForceAtPosition(direction * data.ImpactForce, hit.point, ForceMode.VelocityChange);
            }
            var obj = Object.Instantiate(data.RandomBulletHolePrefab, hit.point, Quaternion.identity, hit.transform);
            obj.transform.forward = hit.normal;
        }
    }
    public static Vector3 CalculateSpread(float spread, Transform transform)
    {
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        return transform.TransformDirection(new Vector3(x, y, 0.0f));
    }
    public static Vector3 CalculateDirection(Vector3 position, Vector3 direction, Vector3 spawnPoint, LayerMask ignoreMask, float maxDistance)
    {
        Ray ray = new Ray(position, direction);
        if (!Physics.Raycast(ray, out RaycastHit targetHit, maxDistance, ~ignoreMask))
        {
            targetHit.point = ray.GetPoint(maxDistance);
        }
        if(targetHit.normal != Vector3.zero && Vector3.Dot(targetHit.normal, spawnPoint - targetHit.point) <= 0)
        {
            return Vector3.zero;
        }
        return (targetHit.point - spawnPoint).normalized;
    }
    public static void Spawn(Vector3 position, Vector3 direction, Transform spawnPoint, BulletData bulletData)
    {
        float bulletCount = bulletData.SpawnCount;
        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 moveDirection = CalculateDirection(position, direction, spawnPoint.position, bulletData.IgnoreMask, bulletData.MaxDistance);
            if (moveDirection != Vector3.zero)
            {
                Bullet bullet = Object.Instantiate(bulletData.Prefab, spawnPoint.position, spawnPoint.rotation);
                Vector3 spread = CalculateSpread(bulletData.Spread, bullet.transform);
                bullet.Setup((moveDirection + spread).normalized, bulletData);
            }
            else
            {
                HitScanShot(position, direction, bulletData);
            }

        }
    }
}
