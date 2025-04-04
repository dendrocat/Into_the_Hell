using System.Collections.Generic;
using UnityEngine;

/**
 * <summary>
 * Класс, описывающий оружие Морозного йети.
 * </summary>
 * **/
public class YetiWeapon : BossWeapon
{
    [SerializeField] float attack1Range = 2f;
    [SerializeField] float attack1Angle = 30f;

    [SerializeField] GameObject attack2MissilePrefab;
    [SerializeField] float attack2MissileSpeed = 8f;

    [SerializeField] float attack3Range = 4f;
    [SerializeField] float attack3Angle = 360f;
    /**
     * <summary>
     * Инициализация оружия.
     * </summary>
     * **/
    void Start()
    {
        FindOwner();
        level = 1;
        maxLevel = 1;

        damage = 50f;
        baseReloadTime = 2f;
        basePrepareAttackTime = 0.2f;
        baseEndAttackTime = 0f;

        attack2MissileSpeed = 8f;
        attack2Damage = 60f;
        baseAttack2ReloadTime = 5f;
        baseAttack2PrepareAttackTime = 0.5f;
        baseAttack2EndAttackTime = 0.5f;

        attack3Damage = 90f;
        baseAttack3ReloadTime = 8f;
        baseAttack3PrepareAttackTime = 1f;
        baseAttack3EndAttackTime = 1f;
    }

    /**
     * <inheritdoc/>
     * **/
    protected override void Attack()
    {
        List<IDamagable> targets = FindTargetsForAttack(attack1Range, attack1Angle);

        // определение уровня эффекта "Серия атак" у владельца оружия
        int ChainLevel = owner.getEffectCount(EffectNames.AttackChain);

        bool hasTargets = false;

        foreach(IDamagable target in targets)
        {
            hasTargets = true;
            target.TakeDamage(damage * (1f + 0.1f * ChainLevel), DamageType.None);
        }

        // обновление эффекта серии атак у владельца
        if (hasTargets) owner.AddEffect(EffectNames.AttackChain, 1);
        else owner.SetEffect(EffectNames.AttackChain, 0);
    }

    /**
     * <inheritdoc/>
     * **/
    protected override void Attack2()
    {
        GameObject missile = GameObject.Instantiate(attack2MissilePrefab, owner.transform.position + owner.weaponObject.localPosition, owner.weaponObject.rotation);
        Missile missileComponent = missile.GetComponent<Missile>();
        missileComponent.SetValues(getScaledDamage(), attack2MissileSpeed);
        missileComponent.direction = owner.weaponObject.localPosition;
    }

    /**
     * <inheritdoc/>
     * **/
    protected override void Attack3()
    {
        List<IDamagable> targets = FindTargetsForAttack(attack3Range, attack3Angle);

        // определение уровня эффекта "Серия атак" у владельца оружия
        int ChainLevel = owner.getEffectCount(EffectNames.AttackChain);

        bool hasTargets = false;

        foreach (IDamagable target in targets)
        {
            hasTargets = true;

            MonoBehaviour monoTarget = target as MonoBehaviour;
            if (monoTarget == null)
            {
                Debug.LogError("Target does not have a Transform component.");
                continue;
            }

            // определение расстояния до босса и расчет снижения урона
            float distance = Vector2.Distance(owner.transform.position, monoTarget.transform.position);
            float damageReduction = -Mathf.Pow(distance / attack3Range, 2f) + 1f;

            target.TakeDamage(attack3Damage * (1f + 0.1f * ChainLevel) * damageReduction, DamageType.None);
        }

        // обновление эффекта серии атак у владельца
        if (hasTargets) owner.AddEffect(EffectNames.AttackChain, 1);
        else owner.SetEffect(EffectNames.AttackChain, 0);
    }
}
