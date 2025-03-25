using UnityEngine;

//Класс оружия дальнего боя
public class MissileWeapon : AlternateAttackWeapon
{
    public GameObject missilePrefab;
    public float missileSpeed;

    protected override void Attack()
    {
        GameObject missile = GameObject.Instantiate(missilePrefab, owner.transform.position, Quaternion.Euler(owner.weaponDirection));
        Missile missileComponent = missile.GetComponent<Missile>();
        missileComponent.speed = missileSpeed;
        missileComponent.damage = getScaledDamage();
        missileComponent.direction = transform.localPosition;
    }
}
