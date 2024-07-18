using UnityEngine;
public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float playSoundDistance;
    [SerializeField] private float volume;
    private Vector3 lastStepPosition;
    private void Start()
    {
        lastStepPosition = transform.position;
    }
    public void FootStepPlay()
    {
        Vector3 currentPosition = transform.position;
        if (Vector3.Distance(lastStepPosition, currentPosition) > playSoundDistance)
        {
            AudioSource.PlayClipAtPoint(footstepClips[Random.Range(0, footstepClips.Length)], currentPosition, volume);
            lastStepPosition = currentPosition;
        }
    }
}
