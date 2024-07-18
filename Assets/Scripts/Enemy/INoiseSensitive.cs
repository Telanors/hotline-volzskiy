using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public interface INoiseSensitive
{
    static List<(INoiseSensitive, Transform)> noiseSensitives = new List<(INoiseSensitive, Transform)>();
    public event Action<Vector3> OnNoise;
    public void NoiseReact(Vector3 noisePoint);
}
