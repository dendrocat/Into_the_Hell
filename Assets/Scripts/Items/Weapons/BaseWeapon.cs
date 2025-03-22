using System.Collections;
using UnityEngine;

//Ѕазовый класс оружи€
public class BaseWeapon : UpgradableItem
{
    public Texture2D weaponTexture;
    public Person owner;
    public float damage;
    public float baseReloadTime;
    public float basePrepareAttackTime;
    public float baseEndAttackTime;
    public bool reloading = false;

    void Start()
    {
        scaleCoeff = 1f;
    }

    public float getScaledDamage()
    {
        return CalcScale(damage);
    }

    public void LaunchAttack()
    {
        if (!reloading && CheckAttackConditions())
        {
            reloading = true;
            StartCoroutine(PerformAttack());
        }
    }

    private IEnumerator PerformAttack()
    {
        yield return new WaitForSeconds(CalcScaleDescending(basePrepareAttackTime));
        Attack();
        yield return new WaitForSeconds(CalcScaleDescending(baseEndAttackTime));
        StartCoroutine(ReloadWeapon(CalcScaleDescending(baseReloadTime)));
    }

    protected virtual bool CheckAttackConditions() //метод дл€ проверки, возможна ли атака. ѕереопредел€етс€ в классах-наследниках
    {
        return true;
    }

    protected virtual void Attack() //метод дл€ атаки. ѕереопредел€етс€ в классах-наследниках
    {

    }

    private IEnumerator ReloadWeapon(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
    }
}
