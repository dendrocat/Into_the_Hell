using System.Collections;
using UnityEngine;

/**
 * <summary>
 * Класс, описывающий оружие с альтернативной атакой.
 * </summary>
 * **/
public class AlternateAttackWeapon : BaseWeapon
{
    [SerializeField] protected float altDamage;
    [SerializeField] protected float baseAltReloadTime;
    [SerializeField] protected float baseAltPrepareAttackTime;
    [SerializeField] protected float baseAltEndAttackTime;
    bool altReloading = false;

    /**
     * <summary>
     * Определяет, перезаряжается ли альтернативная атака оружия.
     * </summary>
     * <returns>bool - перезаряжается ли альтернативная атака оружия.</returns>
     * **/
    public bool isReloadingAlt()
    {
        return altReloading;
    }

    /**
     * <summary>
     * Метод, запускающий альтернативную атаку. Переопределяется в классах-наследниках.
     * </summary>
     * **/
    public virtual void LaunchAltAttack()
    {
        if (!altReloading && CheckAltAttackConditions())
        {
            altReloading = true;
            StartCoroutine(PerformAltAttack());
            _animator?.Play("AltAttack");
        }
    }

    /**
     * <summary>
     * <see cref="Coroutine">Корутина</see>, отвечающая за проведение альтернативной атаки.
     * </summary>
     * <returns><see cref="IEnumerator"/>, использующийся в корутинах.</returns>
     * **/
    protected virtual IEnumerator PerformAltAttack()
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

    /**
     * <summary>
     * Метод для проверки, возможна ли альтернативная атака. Переопределяется в классах-наследниках.
     * </summary>
     * <returns>bool - возможна ли альтернативная атака.</returns>
     * **/
    protected virtual bool CheckAltAttackConditions()
    {
        return false;
    }

    /**
     * <summary>
     * Метод, описывающий действия, входящие в альтернативную атаку. Переопределяется в классах-наследниках.
     * </summary>
     * **/
    protected virtual void AltAttack()
    {

    }

    /**
     * <summary>
     * Метод, описывающий действия, выполняемые до начала подготовки к альтернативной атаке. Переопределяется в классах-наследниках.
     * </summary>
     * **/
    protected virtual void OnPrepareAltAttackStart()
    {

    }

    /**
     * <summary>
     * Метод, описывающий действия, выполняемые после окончания подготовки к альтернативной атаке. Переопределяется в классах-наследниках.
     * </summary>
     * **/
    protected virtual void OnPrepareAltAttackEnd()
    {

    }

    /**
     * <summary>
     * Метод, описывающий действия, выполняемые до начала завершения альтернативной атаки. Переопределяется в классах-наследниках.
     * </summary>
     * **/
    protected virtual void OnEndAltAttackStart()
    {

    }

    /**
     * <summary>
     * Метод, описывающий действия, выполняемые после окончания завершения альтернативной атаки. Переопределяется в классах-наследниках.
     * </summary>
     * **/
    protected virtual void OnEndAltAttackEnd()
    {

    }

    /**
     * <summary>
     * <see cref="Coroutine">Корутина</see>, отвечающая за обработку перезарядки альтернативной атаки оружия.
     * </summary>
     * <returns><see cref="IEnumerator"/>, использующийся в корутинах.</returns>
     * **/
    protected IEnumerator ReloadAltWeapon(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        altReloading = false;
    }
}
