using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class NoiseSource : MonoBehaviour
{
    private static float delayBeforeNoise = 0.35f;
    private static float maxTargetPointOffset = 5.0f;
    private static float checkDistance = 35.0f;

    public LayerMask enviromentMask;
    public float noiseRadius = 0.5f;
    public float noiseDelay = 1.0f;

    private float delayTimer;
    private void Start()
    {
        delayTimer = 0.0f;
    }
    private void Update()
    {
        if (delayTimer <= 0.0f) return;
        delayTimer -= Time.deltaTime;
    }
    public void DoNoise()
    {
        if (delayTimer > 0.0f) return;
        delayTimer = noiseDelay;
        StartCoroutine(NoiseCoroutine());   
    }
    private IEnumerator NoiseCoroutine()
    {
        yield return new WaitForSeconds(delayBeforeNoise);
        if (!NavMesh.SamplePosition(transform.position, out NavMeshHit navMeshHit, maxTargetPointOffset, NavMesh.AllAreas)) yield break;
        var direction = navMeshHit.position - transform.position;
        Physics.Raycast(transform.position, direction, out RaycastHit raycastHit, checkDistance, enviromentMask);
        if (Vector3.Distance(transform.position, raycastHit.point) < direction.magnitude) yield break;
        var noiseSensitives = INoiseSensitive.noiseSensitives;
        for (int i = 0; i < noiseSensitives.Count; i++)
        {
            if (noiseSensitives[i].Item2 == null) yield break;
            if (Vector3.Distance(transform.position, noiseSensitives[i].Item2.position) <= noiseRadius)
            {
                noiseSensitives[i].Item1.NoiseReact(navMeshHit.position);
            }

        }
    }
}
