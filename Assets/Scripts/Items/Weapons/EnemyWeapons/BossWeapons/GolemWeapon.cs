using System.Collections.Generic;
using UnityEngine;

public class GolemWeapon : BossWeapon
{
    /// <summary>
    /// Радиус первой атаки.
    /// </summary>
    [Tooltip("Радиус первой атаки")]
    [SerializeField] float attack1Range = 2f;

    /// <summary>
    /// Угол первой атаки.
    /// </summary>
    [Tooltip("Угол первой атаки")]
    [SerializeField] float attack1Angle = 30f;

    /// <summary>
    /// Префаб булыжника для второй атаки.
    /// </summary>
    [Tooltip("Префаб булыжника для второй атаки")]
    [SerializeField] GameObject attack2MissilePrefab;

    /// <summary>
    /// Скорость булыжника для второй атаки.
    /// </summary>
    [Tooltip("Скорость булыжника для второй атаки")]
    [SerializeField] float attack2MissileSpeed = 8f;

    /// <summary>
    /// Радиус третьей атаки.
    /// </summary>
    [Tooltip("Радиус третьей атаки")]
    [SerializeField] float attack3Range = 4f;

    /// <summary>
    /// Угол третьей атаки.
    /// </summary>
    [Tooltip("Угол третьей атаки")]
    [SerializeField] float attack3Angle = 360f;
    
    /// <summary>
    /// Инициализация оружия.
    /// </summary>
    void Start()
    {
        FindOwner();
    }

    /// <summary>
    /// Конструктор, инициализирующий параметры оружия Голема.
    /// </summary>
    public GolemWeapon()
    {
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

    /// <inheritdoc />
    protected override void Attack()
    {
        List<IDamagable> targets = FindTargetsForAttack(attack1Range, attack1Angle);
        Debug.Log(damage);
        foreach (IDamagable target in targets)
        {
            target.TakeDamage(damage, DamageType.None);
        }
    }

    /// <inheritdoc />
    protected override void Attack2()
    {
        GameObject missile = GameObject.Instantiate(attack2MissilePrefab, owner.transform.position + owner.weaponObject.localPosition, owner.weaponObject.rotation);
        Missile missileComponent = missile.GetComponent<Missile>();
        missileComponent.SetTargetTags(targetTags);
        missileComponent.SetValues(getScaledDamage(), attack2MissileSpeed);
        missileComponent.direction = owner.weaponObject.localPosition;
    }

    /// <inheritdoc />
    protected override void Attack3()
    {
        List<IDamagable> targets = FindTargetsForAttack(attack3Range, attack3Angle);
        Debug.Log(attack3Damage);
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
