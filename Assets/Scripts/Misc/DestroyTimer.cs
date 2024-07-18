using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [Min(0.0f)] public float time;
    private void Start()
    {
        Destroy(gameObject, time);
    }
}
