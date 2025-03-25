using UnityEngine;

public class Bow : MissileWeapon
{
    public GameObject altMissilePrefab;
    public float altMissileSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scaleCoeff = 1f;
        minValueDescending = 0.6f;
        damage = 10f;
        baseReloadTime = 1f;
        basePrepareAttackTime = 0f;
        baseEndAttackTime = 0f;
        altDamage = 30f;
        baseAltPrepareAttackTime = 0f;
        baseAltEndAttackTime = 0f;
        baseAltReloadTime = 5f;
    }

    
}
