using UnityEngine;
using System.Collections;
public class WeaponRecoilController : MonoBehaviour
{
    [HideInInspector] public PlayerCameraManager playerCamera;
    [HideInInspector] public WeaponRecoilParametrs recoilParametrs;

    private AdvanceWeapon weapon;
    private float xRecoil;
    private float yRecoil;
    private float recoilTimer;
    private int patternIndex;
    private void Start()
    {
        weapon = GetComponent<AdvanceWeapon>();
        recoilParametrs = weapon.WeaponData.RecoilParametrs;
        weapon.OnEquip.AddListener(OnEquip);
        weapon.ShotStart.AddListener(ResetPattern);
        weapon.ShotProcess.AddListener(DoRecoil);
    }
    private void Update()
    {
        if (playerCamera == null) return;
        if (recoilTimer > 0)
        {
            playerCamera.XRotation += (xRecoil * Time.deltaTime) / recoilParametrs.Duration;
            playerCamera.YRotation += (yRecoil * Time.deltaTime) / recoilParametrs.Duration;
            recoilTimer -= Time.deltaTime;
        }
    }
    private void OnEquip()
    {
        playerCamera = weapon.Vision as PlayerCameraManager;
    }
    public void DoRecoil()
    {
        if (playerCamera == null) return;
        recoilTimer = recoilParametrs.Duration;
        xRecoil = recoilParametrs.RecoilPatterns[patternIndex].x;
        if (recoilParametrs.RandomYPattern)
        {
            float maxY = recoilParametrs.RecoilPatterns[patternIndex].y;
            yRecoil = Random.Range(-maxY, maxY);
        }
        else
        {
            yRecoil = recoilParametrs.RecoilPatterns[patternIndex].y;
        }
        patternIndex = NextPatternIndex(patternIndex);
    }
    public void ResetPattern()
    {
        patternIndex = 0;
    }
    private int NextPatternIndex(int index) => (index + 1) % recoilParametrs.RecoilPatterns.Length;
}
