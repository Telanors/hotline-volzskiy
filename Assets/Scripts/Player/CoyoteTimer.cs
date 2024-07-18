using UnityEngine;

public class CoyoteTimer : MonoBehaviour
{
    private bool allowed = true;
    public bool Allowed { get => allowed; }

    [SerializeField] private float coyoteTime;
    public void PlayTimer()
    {
        if(allowed) 
            Invoke(nameof(Disable), coyoteTime);
    }
    public void ResetTimer() => allowed = true;
    
    public void Disable() => allowed = false;
}
