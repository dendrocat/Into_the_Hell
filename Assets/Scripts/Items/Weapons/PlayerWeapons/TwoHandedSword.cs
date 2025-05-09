using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * Класс, описывающий оружие игрока "Двуручный меч"
 * </summary>
 * **/
public class TwoHandedSword : RangedWeapon
{
    [SerializeField] float altRange = 5f;
    [SerializeField] float altAngle = 10f;

    public TwoHandedSword()
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

    /**
     * <inheritdoc/>
     * **/
    protected override bool CheckAltAttackConditions()
    {
        return true;
    }

    /**
     * <inheritdoc/>
     * **/
    protected override void OnPrepareAltAttackEnd()
    {
        owner.facingDirection = owner.weaponDirection;
        owner.AddEffect(EffectNames.Shift, 1);
    }

    /**
     * <inheritdoc/>
     * **/
    protected override void AltAttack()
    {
        List<IDamagable> targets = FindTargetsForAttack(altRange, altAngle);

        foreach (IDamagable target in targets)
        {
            target.TakeDamage(CalcScale(altDamage), DamageType.None);
        }
    }
}
