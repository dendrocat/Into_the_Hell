using UnityEngine;

/// <summary>
/// Класс оружия дальнего боя.
/// </summary>
public class MissileWeapon : AlternateAttackWeapon
{
    /// <summary>
    /// Префаб снаряда для создания при атаке.
    /// </summary>
    [Tooltip("Префаб снаряда для создания при атаке")]
    [SerializeField] protected GameObject missilePrefab;

    /// <summary>
    /// Скорость снаряда.
    /// </summary>
    [Tooltip("Скорость снаряда")]
    [SerializeField] protected float missileSpeed;

    /// <inheritdoc />
    protected override void Attack()
    {
        Debug.Log($"{gameObject} Attack");
        GameObject missile = GameObject.Instantiate(missilePrefab, owner.transform.position + owner.weaponObject.localPosition, owner.weaponObject.rotation);
        Missile missileComponent = missile.GetComponent<Missile>();
        missileComponent.SetTargetTags(targetTags);
        missileComponent.SetValues(getScaledDamage(), missileSpeed);
        missileComponent.direction = owner.weaponObject.localPosition;
    }
}
