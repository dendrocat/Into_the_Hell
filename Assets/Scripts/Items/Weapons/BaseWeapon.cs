using System.Collections;
using UnityEngine;

//Базовый класс оружия
public class BaseWeapon : UpgradableItem
{
    [SerializeField] protected Texture2D weaponTexture;
    public Person owner;
    [SerializeField] protected float damage;
    [SerializeField] protected float baseReloadTime;
    [SerializeField] protected float basePrepareAttackTime;
    [SerializeField] protected float baseEndAttackTime;
    bool reloading = false;

    public bool isReloading()
    {
        return reloading;
    }

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
        OnPrepareAttackStart();
        yield return new WaitForSeconds(CalcScaleDescending(basePrepareAttackTime));
        OnPrepareAttackEnd();
        Attack();
        OnEndAttackStart();
        yield return new WaitForSeconds(CalcScaleDescending(baseEndAttackTime));
        OnEndAttackEnd();
        StartCoroutine(ReloadWeapon(CalcScaleDescending(baseReloadTime)));
    }

    protected virtual bool CheckAttackConditions() //метод для проверки, возможна ли атака. Переопределяется в классах-наследниках
    {
        return true;
    }

    protected virtual void Attack() //метод для атаки. Переопределяется в классах-наследниках
    {

    }

    protected virtual void OnPrepareAttackStart() //действия, выполняемые перед началом подготовки атаки
    {

    }

    protected virtual void OnPrepareAttackEnd() //действия, выполняемые после окончания подготовки атаки
    {

    }

    protected virtual void OnEndAttackStart() //действия, выполняемые перед началом окончания атаки
    {

    }

    protected virtual void OnEndAttackEnd() //действия, выполняемые после окончания окончания атаки
    {

    }

    private IEnumerator ReloadWeapon(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
    }
}
