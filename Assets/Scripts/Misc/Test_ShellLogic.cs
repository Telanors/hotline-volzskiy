using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ShellLogic : MonoBehaviour
{
    private AudioSource source;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude >= 5.0f && !collision.transform.CompareTag("Small") && !collision.transform.CompareTag("Player"))
        {
            source.PlayOneShot(source.clip);
        }
    }
}
