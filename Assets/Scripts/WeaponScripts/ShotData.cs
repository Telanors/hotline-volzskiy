using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShotData
{
    public WeaponData weaponData;
    public RaycastHit[] hits;
    public ShotData( WeaponData weaponData, params RaycastHit[] hits)
    {
        this.weaponData = weaponData;
        this.hits = hits;;
    }
}
