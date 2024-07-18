using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Ballistics : MonoBehaviour
{
    public Transform player;
    public GameObject bullet;
    public Transform SPAWNPOINT;
    public Transform target;
    public float angleInDegrees;
    public float g = -20;

    // Update is called once per frame
    void Update()
    {
        SPAWNPOINT.localEulerAngles = new Vector3(-angleInDegrees, 0.0f, 0.0f);
        if (Input.GetKeyDown(KeyCode.V))
        {
            Shot();
        }
    }
    void Shot()
    {
        Vector3 fromTo = target.position - transform.position;
        Vector3 fromToXZ = new Vector3 (fromTo.x, 0.0f, fromTo.z);
        transform.rotation = Quaternion.LookRotation(fromToXZ, Vector3.up);
        float x = fromToXZ.magnitude;
        float y = fromTo.y;
        float AnglesToRadian = angleInDegrees * Mathf.Deg2Rad;
        float v = Mathf.Sqrt((g * x * x) / (2 * (y - Mathf.Tan(AnglesToRadian) * x) * Mathf.Pow(Mathf.Cos(AnglesToRadian), 2)));
        Rigidbody newbullet = Instantiate(bullet, SPAWNPOINT.position, Quaternion.identity).GetComponent<Rigidbody>();
        newbullet.velocity = SPAWNPOINT.forward * v;   
    }
     public static float Velocity(Vector3 target, Vector3 current, float dropAngle)
    {
        Vector3 fromTo = target - current;
        Vector3 fromToXZ = new Vector3(fromTo.x, 0.0f, fromTo.z);
        float x = fromToXZ.magnitude;
        float y = fromTo.y;
        float AnglesToRadian = dropAngle * Mathf.Deg2Rad;
        return Mathf.Sqrt((-25* x * x) / (2 * (y - Mathf.Tan(AnglesToRadian) * x) * Mathf.Pow(Mathf.Cos(AnglesToRadian), 2)));
    }
}
