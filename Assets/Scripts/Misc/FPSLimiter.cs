using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSLimiter : MonoBehaviour
{
    [Min(1)]
    public int MaxFPS;
    private void Start()
    {
        Application.targetFrameRate = MaxFPS;
    }
    private void OnValidate()
    {
        Application.targetFrameRate = MaxFPS;
    }
}
