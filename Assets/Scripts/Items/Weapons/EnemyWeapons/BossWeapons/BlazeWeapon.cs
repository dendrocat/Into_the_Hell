using System.Collections;
using UnityEngine;

/**
 * <summary>
 * Класс, реализующий оружие исчадия ада.
 * </summary>
 * **/
public class BlazeWeapon : BossWeapon
{
    [SerializeField] GameObject missilePrefab;
    [SerializeField] float missileSpeed = 8f;

    [SerializeField] GameObject attack2MissilePrefab1;
    [SerializeField] GameObject attack2MissilePrefab2;
    [SerializeField] float attack2MissileSpeed = 8f;

    [SerializeField] int attack3FireballCount = 16;
    [SerializeField] GameObject attack3MissilePrefab;
    [SerializeField] float attack3MissileSpeed = 8f;

    /**
     * <summary>
     * Инициализация оружия.
     * </summary>
     * **/
    void Start()
    {
        FindOwner();
    }

    public BlazeWeapon()
    {
        level = 1;
        maxLevel = 1;

        damage = 75f;
        missileSpeed = 8f;
        baseReloadTime = 1f;
        basePrepareAttackTime = 0f;
        baseEndAttackTime = 0f;

        attack2MissileSpeed = 4f;
        attack2Damage = 100f;
        baseAttack2ReloadTime = 4f;
        baseAttack2PrepareAttackTime = 0.5f;
        baseAttack2EndAttackTime = 0.5f;

        attack3FireballCount = 16;
        attack3MissileSpeed = 8f;
        attack3Damage = 25f;
        baseAttack3ReloadTime = 8f;
        baseAttack3PrepareAttackTime = 1f;
        baseAttack3EndAttackTime = 2f;
    }

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
    }

    /**
     * <inheritdoc/>
     * **/
    protected override void Attack2()
    {
        GameObject sphere1 = GameObject.Instantiate(attack2MissilePrefab1, owner.transform.position + owner.weaponObject.localPosition, owner.weaponObject.rotation);
        Missile sphere1Component = sphere1.GetComponent<Missile>();
        sphere1Component.SetTargetTags(targetTags);
        sphere1Component.SetValues(CalcScale(attack2Damage), attack2MissileSpeed);
        sphere1Component.direction = Quaternion.Euler(0f, 0f, -15f) * owner.weaponObject.localPosition;

        GameObject sphere2 = GameObject.Instantiate(attack2MissilePrefab2, owner.transform.position + owner.weaponObject.localPosition, owner.weaponObject.rotation);
        Missile sphere2Component = sphere2.GetComponent<Missile>();
        sphere2Component.SetTargetTags(targetTags);
        sphere2Component.SetValues(CalcScale(attack2Damage), attack2MissileSpeed);
        sphere2Component.direction = Quaternion.Euler(0f, 0f, 15f) * owner.weaponObject.localPosition;

        FireSphere fireSphere1 = sphere1.GetComponent<FireSphere>(),
            fireSphere2 = sphere2.GetComponent<FireSphere>();

        fireSphere1.linkedSphere = fireSphere2;
        fireSphere2.linkedSphere = fireSphere1;
    }

    /**
     * <inheritdoc/>
     * **/
    protected override void Attack3()
    {
        StartCoroutine(SpawnFireball(attack3FireballCount - 1));
    }

    /**
     * <summary>
     * Корутина, управляющая спавном огненных шаров.
     * </summary>
     * <param name="number">Номер очередного огненного шара.</param>
     * **/
    IEnumerator SpawnFireball(int number)
    {
        Debug.Log("Start Spawning");
        while (number-- > 0)
        {
            GameObject missile = GameObject.Instantiate(attack3MissilePrefab, owner.transform.position + owner.weaponObject.localPosition, owner.weaponObject.rotation);
            Missile missileComponent = missile.GetComponent<Missile>();
            missileComponent.SetTargetTags(targetTags);
            missileComponent.SetValues(CalcScale(attack3Damage), attack3MissileSpeed);
            missileComponent.direction = owner.weaponObject.localPosition;
            yield return new WaitForSeconds(baseAttack3EndAttackTime / attack3FireballCount);
        }
        Debug.Log("End Spawning");
    }
}
