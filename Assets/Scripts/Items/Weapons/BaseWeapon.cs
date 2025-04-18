using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый класс оружия, наследующий функциональность улучшаемого предмета.
/// </summary>
public class BaseWeapon : UpgradableItem
{
    /// <summary>
    /// Текстура оружия.
    /// </summary>
    [SerializeField] protected Texture2D weaponTexture;

    /// <summary>
    /// Список тегов, которым оружие может наносить урон.
    /// </summary>
    [SerializeField] protected List<string> targetTags;

    /// <summary>
    /// Владелец оружия.
    /// </summary>
    protected Person owner;

    /// <summary>
    /// Базовый урон, наносимый оружием.
    /// </summary>
    [SerializeField] protected float damage;

    /// <summary>
    /// Базовое время перезарядки оружия.
    /// </summary>
    [SerializeField] protected float baseReloadTime;

    /// <summary>
    /// Базовое время подготовки атаки.
    /// </summary>
    [SerializeField] protected float basePrepareAttackTime;

    /// <summary>
    /// Базовое время окончания атаки.
    /// </summary>
    [SerializeField] protected float baseEndAttackTime;

    /// <summary>
    /// Флаг, указывающий, находится ли оружие в процессе перезарядки.
    /// </summary>
    bool reloading = false;

    /// <summary>
    /// Проверяет, находится ли оружие в процессе перезарядки.
    /// </summary>
    /// <returns>True, если оружие перезаряжается, иначе false.</returns>
    public bool isReloading()
    {
        return reloading;
    }

    /// <summary>
    /// Заполняет поле владельца.
    /// </summary>
    protected void FindOwner()
    {
        owner = gameObject.GetComponentInParent<Person>();
    }

    /// <summary>
    /// Инициализирует коэффициент масштабирования при старте.
    /// </summary>
    void Start()
    {
        FindOwner();
    }

    public BaseWeapon()
    {
        scaleCoeff = 1f;
    }

    /// <summary>
    /// Возвращает урон, масштабированный согласно текущему уровню оружия.
    /// </summary>
    /// <returns>Масштабированный урон.</returns>
    public float getScaledDamage()
    {
        return CalcScale(damage);
    }

    /// <summary>
    /// Запускает атаку, если условия позволяют.
    /// </summary>
    public void LaunchAttack()
    {
        if (!reloading && CheckAttackConditions())
        {
            reloading = true;
            StartCoroutine(PerformAttack());
        }
    }

    /// <summary>
    /// Корутина, выполняющая последовательность действий при атаке.
    /// </summary>
    /// <returns>Корутина.</returns>
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

    /// <summary>
    /// Проверяет условия, позволяющие выполнить атаку. Метод для переопределения в классах-наследниках.
    /// </summary>
    /// <returns>True, если атака возможна, иначе false.</returns>
    protected virtual bool CheckAttackConditions()
    {
        return true;
    }

    /// <summary>
    /// Метод для выполнения атаки. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void Attack()
    {

    }

    /// <summary>
    /// Действия, выполняемые перед началом подготовки атаки. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void OnPrepareAttackStart()
    {

    }

    /// <summary>
    /// Действия, выполняемые после окончания подготовки атаки. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void OnPrepareAttackEnd()
    {

    }

    /// <summary>
    /// Действия, выполняемые перед началом окончания атаки. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void OnEndAttackStart()
    {

    }

    /// <summary>
    /// Действия, выполняемые после окончания окончания атаки. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void OnEndAttackEnd()
    {

    }

    /// <summary>
    /// Корутина, выполняющая перезарядку оружия.
    /// </summary>
    /// <param name="reloadTime">Время перезарядки.</param>
    /// <returns>Корутина.</returns>
    private IEnumerator ReloadWeapon(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
    }

    public List<string> GetTargetTags()
    {
        return targetTags;
    }
}
