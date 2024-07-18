using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthy
{
    public float Health { get; }
    public void AddHealth(float value);
}
