using UnityEngine;

/**
 * <summary>
 * Класс оружия дальнего боя.
 * </summary>
 * **/
public class MissileWeapon : AlternateAttackWeapon
{
    [SerializeField] protected GameObject missilePrefab;
    [SerializeField] protected float missileSpeed;

    /**
     * <inheritdoc/>
     * **/
    protected override void Attack()
    {
        GameObject missile = GameObject.Instantiate(missilePrefab, owner.transform.position + owner.weaponObject.localPosition, owner.weaponObject.rotation);
        Missile missileComponent = missile.GetComponent<Missile>();
        missileComponent.SetTargetTags(targetTags);
        missileComponent.SetValues(getScaledDamage(), missileSpeed);
        missileComponent.direction = owner.weaponObject.localPosition;

        Debug.Log(owner.gameObject.name + ": launched attack");
    }
}
