using System.Collections.Generic;
using UnityEngine;

public class TwoHandedSword : RangedWeapon
{
    public float altRange = 5f;
    public float altAngle = 10f;
    void Start()
    {
        damage = 15f;
        baseReloadTime = 2f;
        basePrepareAttackTime = 0.2f;
        baseEndAttackTime = 0.2f;
        altDamage = 45f;
        baseAltPrepareAttackTime = 0f;
        baseAltEndAttackTime = 1f;
        baseAltReloadTime = 6f;
        scaleCoeff = 1f;
        minValueDescending = 5f / 6f;
        range = 3f;
        angle = 90f;
    }

    protected override bool CheckAltAttackConditions()
    {
        return true;
    }

    protected override void OnPrepareAltAttackEnd()
    {
        owner.facingDirection = owner.weaponDirection;
        owner.AddEffect(EffectNames.Shift, 1);
    }

    protected override void AltAttack()
    {
        List<IDamagable> targets = FindTargetsForAttack();

        foreach(IDamagable target in targets)
        {
            target.TakeDamage(altDamage, DamageType.None);
        }
    }
}
