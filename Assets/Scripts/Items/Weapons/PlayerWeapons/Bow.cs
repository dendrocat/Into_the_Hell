using System.Collections;
using UnityEngine;


/// <summary>
/// Класс, описывающий оружие игрока "Лук".
/// </summary>
public class Bow : MissileWeapon
{
    /// <summary>
    /// Префаб альтернативного снаряда (взрывная стрела).
    /// </summary>
    [Tooltip("Префаб альтернативного снаряда (взрывная стрела)")]
    [SerializeField] GameObject altMissilePrefab;

    /// <summary>
    /// Скорость альтернативного снаряда.
    /// </summary>
    [Tooltip("Скорость альтернативного снаряда")]
    [SerializeField] float altMissileSpeed;

    /// <summary>
    /// Конструктор, инициализирующий параметры оружия.
    /// </summary>
    public Bow()
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

    /// <inheritdoc />
    protected override bool CheckAltAttackConditions()
    {
        if (owner is Player player)
        {
            return player.inventory.GetExplosiveArrowCount() > 0;
        }
        else return false;
    }

    /// <inheritdoc />
    protected override IEnumerator PerformAttack()
    {
        OnPrepareAttackStart();
        yield return new WaitForSeconds(basePrepareAttackTime);
        OnPrepareAttackEnd();
        Attack();
        OnEndAttackStart();
        yield return new WaitForSeconds(CalcScaleDescending(baseEndAttackTime));
        OnEndAttackEnd();
        StartCoroutine(ReloadWeapon(CalcScaleDescending(baseReloadTime)));
    }

    /// <inheritdoc />
    protected override IEnumerator PerformAltAttack()
    {
        OnPrepareAltAttackStart();
        yield return new WaitForSeconds(baseAltPrepareAttackTime);
        OnPrepareAltAttackEnd();
        AltAttack();
        OnEndAltAttackStart();
        yield return new WaitForSeconds(CalcScaleDescending(baseAltEndAttackTime));
        OnEndAltAttackEnd();
        StartCoroutine(ReloadAltWeapon(CalcScaleDescending(baseAltReloadTime)));
    }

    /// <inheritdoc />
    protected override void AltAttack()
    {
        if (owner is Player player)
        {
            player.inventory.UseExplosiveArrow();
        }
        GameObject missile = GameObject.Instantiate(altMissilePrefab, owner.transform.position + owner.weaponObject.localPosition, owner.weaponObject.rotation);
        var arrowComponent = missile.GetComponent<ExplosiveArrow>();
        arrowComponent.SetTargetTags(targetTags);
        arrowComponent.SetValues(CalcScale(damage), altMissileSpeed);
        arrowComponent.SetExplosionDamage(CalcScale(altDamage));
        arrowComponent.direction = owner.weaponObject.localPosition;
    }
}