using System.Collections.Generic;
using UnityEngine;

public class GolemWeapon : BossWeapon
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

        damage = 75f;
        baseReloadTime = 2f;
        basePrepareAttackTime = 0.2f;
        baseEndAttackTime = 0f;

        attack2MissileSpeed = 8f;
        attack2Damage = 80f;
        baseAttack2ReloadTime = 5f;
        baseAttack2PrepareAttackTime = 0.5f;
        baseAttack2EndAttackTime = 0.5f;

        attack3Damage = 120f;
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

        foreach (IDamagable target in targets)
        {
            target.TakeDamage(damage, DamageType.None);
        }
    }

    /**
     * <inheritdoc/>
     * **/
    protected override void Attack2()
    {
        GameObject missile = GameObject.Instantiate(attack2MissilePrefab, owner.transform.position + owner.weaponObject.localPosition, owner.weaponObject.rotation);
        Missile missileComponent = missile.GetComponent<Missile>();
        missileComponent.SetTargetTags(targetTags);
        missileComponent.SetValues(getScaledDamage(), attack2MissileSpeed);
        missileComponent.direction = owner.weaponObject.localPosition;
    }

    /**
     * <inheritdoc/>
     * **/
    protected override void Attack3()
    {
        List<IDamagable> targets = FindTargetsForAttack(attack3Range, attack3Angle);

        foreach (IDamagable target in targets)
        {
            MonoBehaviour monoTarget = target as MonoBehaviour;
            if (monoTarget == null)
            {
                Debug.LogError("Target does not have a Transform component.");
                continue;
            }

            // определение расстояния до босса и расчет снижения урона
            float distance = Vector2.Distance(owner.transform.position, monoTarget.transform.position);
            float damageReduction = Mathf.Clamp(-Mathf.Pow(distance / attack3Range, 2f) + 1f, 0f, 1f);

            target.TakeDamage(attack3Damage * damageReduction, DamageType.None);
        }
    }
}
