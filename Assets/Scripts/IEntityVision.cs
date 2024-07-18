using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntityVision
{ 
    public Transform visionTransform { get; }
    public Vector3 forward { get; }
    public Vector3 right { get; }
    public Vector3 up { get; }
    public Vector3 position { get; }
}
