using UnityEngine;
[CreateAssetMenu(fileName = "New Recoil Parametrs", menuName = "Weapon/New Recoil")]
public class WeaponRecoilParametrs : ScriptableObject
{
    [SerializeField] private Vector2[] recoilPatterns = { new Vector2(1.0f, 0.0f) }; 
    public Vector2[] RecoilPatterns { get => recoilPatterns; }

    [SerializeField] private float duration;
    public float Duration { get => duration; }

    [SerializeField] private bool randomYPattern;
    public bool RandomYPattern { get => randomYPattern; }
}
