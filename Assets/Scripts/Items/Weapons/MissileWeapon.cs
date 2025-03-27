using UnityEngine;

//Класс оружия дальнего боя
public class MissileWeapon : AlternateAttackWeapon
{
    public GameObject missilePrefab;
    public float missileSpeed;

    protected override void Attack()
    {
        GameObject missile = GameObject.Instantiate(missilePrefab, owner.transform.position + owner.weaponObject.localPosition, owner.weaponObject.rotation);
        Missile missileComponent = missile.GetComponent<Missile>();
        missileComponent.speed = missileSpeed;
        missileComponent.damage = getScaledDamage();
        missileComponent.direction = owner.weaponObject.localPosition;
    }
}
