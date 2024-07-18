using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefab;
    public float timeToSpawn = 0;
    public bool spawn = false;
    private float timer = 0;

    private void Update()
    {
        if (!spawn) return;
        timer += Time.deltaTime;
        if(timer >= timeToSpawn)
        {
            Spawn();
            timer = 0;
        }
    }
    private void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity, transform);
    }
}
