using System.Collections;
using UnityEngine;

public class AlternateAttackWeapon : BaseWeapon
{
    public float altDamage;
    public float baseAltReloadTime;
    public float baseAltPrepareAttackTime;
    public float baseAltEndAttackTime;
    public bool altReloading = false;
    public virtual void LaunchAltAttack()
    {
        if (!altReloading && CheckAltAttackConditions())
        {
            altReloading = true;
            StartCoroutine(PerformAltAttack());
        }
    }

    private IEnumerator PerformAltAttack()
    {
        OnPrepareAltAttackStart();
        yield return new WaitForSeconds(CalcScaleDescending(baseAltPrepareAttackTime));
        OnPrepareAltAttackEnd();
        AltAttack();
        OnEndAltAttackStart();
        yield return new WaitForSeconds(CalcScaleDescending(baseAltEndAttackTime));
        OnEndAltAttackEnd();
        StartCoroutine(ReloadAltWeapon(CalcScaleDescending(baseAltReloadTime)));
    }

    protected virtual bool CheckAltAttackConditions() //метод для проверки, возможна ли альтернативаная атака.
                                                      //Переопределяется в классах-наследниках
    {
        return false;
    }

    protected virtual void AltAttack() //метод для альтернативной атаки. Переопределяется в классах-наследниках
    {

    }

    protected virtual void OnPrepareAltAttackStart() //действия, выполняемые перед началом подготовки альт атаки
    {

    }

    protected virtual void OnPrepareAltAttackEnd() //действия, выполняемые после окончания подготовки альт атаки
    {

    }

    protected virtual void OnEndAltAttackStart() //действия, выполняемые перед началом окончания альт атаки
    {

    }

    protected virtual void OnEndAltAttackEnd() //действия, выполняемые после окончания окончания альт атаки
    {

    }

    private IEnumerator ReloadAltWeapon(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        altReloading = false;
    }
}
