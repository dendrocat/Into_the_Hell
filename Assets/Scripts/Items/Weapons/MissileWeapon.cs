using UnityEngine;

//Класс оружия дальнего боя
public class MissileWeapon : BaseWeapon
{
    public GameObject missilePrefab;
    public float missileSpeed;

    protected override void Attack()
    {
        GameObject missile = GameObject.Instantiate(missilePrefab, owner.transform.position, Quaternion.Euler(owner.currentDirection));
        Missile missileComponent = missile.GetComponent<Missile>();
        missileComponent.speed = missileSpeed;
        missileComponent.damage = getScaledDamage();
        missileComponent.direction = transform.localPosition;
    }
}
