using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Базовый класс оружия, наследующий функциональность улучшаемого предмета.
/// </summary>
public class BaseWeapon : UpgradableItem
{
    /// <summary>
    /// Список спрайтов оружия.
    /// </summary>
    [Tooltip("Список спрайтов оружия.")]
    [SerializeField] protected List<SpriteRenderer> weaponSprites;

    /// <summary>
    /// Список тегов, которым оружие может наносить урон.
    /// </summary>
    [Tooltip("Список тегов, которым оружие может наносить урон.")]
    [SerializeField] protected List<string> targetTags;

    /// <summary>
    /// Владелец оружия.
    /// </summary>
    [Tooltip("Владелец оружия.")]
    [SerializeField] protected Person owner;

    /// <summary>
    /// Базовый урон, наносимый оружием.
    /// </summary>
    [Tooltip("Базовый урон, наносимый оружием.")]
    [SerializeField] protected float damage;

    /// <summary>
    /// Базовое время перезарядки оружия.
    /// </summary>
    [Tooltip("Базовое время перезарядки оружия.")]
    [SerializeField] protected float baseReloadTime;

    /// <summary>
    /// Базовое время подготовки атаки.
    /// </summary>
    [Tooltip("Базовое время подготовки атаки.")]
    [SerializeField] protected float basePrepareAttackTime;

    /// <summary>
    /// Базовое время окончания атаки.
    /// </summary>
    [Tooltip("Базовое время окончания атаки.")]
    [SerializeField] protected float baseEndAttackTime;

    /// <summary>
    /// Флаг, указывающий, находится ли оружие в процессе перезарядки.
    /// </summary>
    bool reloading = false;

    /// <summary>
    /// Компонент аниматора для управления анимациями
    /// </summary>
    protected Animator _animator;

    ///<summary>
    /// Показывает, отражено ли сейчас оружие по локальной оси Y.
    /// </summary>
    bool flippedY = false;

    /// <summary>
    /// Показывает, опущены ли спрайты оружия ниже игрока
    /// </summary>
    bool spritesDowned = false;

    /// <summary>
    /// Проверяет, находится ли оружие в процессе перезарядки.
    /// </summary>
    /// <returns><see langword="true"/>, если оружие перезаряжается, иначе <see langword="false"/>.</returns>
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
        TryGetComponent(out _animator);
    }

    /// <summary>
    /// Проверяет текущий поворот оружия и изменяет его ориентацию при необходимости.
    /// </summary>
    protected virtual void Update()
    {
        bool newFlippedY = owner.weaponDirection.x < 0;
        if (flippedY != newFlippedY)
        {
            Vector3 localScale = transform.localScale;
            transform.localScale = new Vector3(localScale.x, -localScale.y, localScale.z);
        }
        flippedY = newFlippedY;
        if (owner.weaponDirection.y > 0)
        {
            if (!spritesDowned) DownWeaponSprites();
        }
        else if (spritesDowned) UpWeaponSprites();
    }

    /// <summary>
    /// Конструктор, инициализирующий коэффициент масштабирования.
    /// </summary>
    public BaseWeapon()
    {
        scaleCoeff = 1f;
    }

    /// <summary>
    /// Возвращает урон, масштабированный согласно текущему уровню оружия.
    /// </summary>
    /// <returns><see langword="float"/> - Масштабированный урон.</returns>
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
            _animator?.Play("Attack");
        }
    }

    /// <summary>
    /// Корутина, выполняющая последовательность действий при атаке. Может быть переопределена в классах-наследниках.
    /// </summary>
    /// <returns>Корутина.</returns>
    protected virtual IEnumerator PerformAttack()
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
    /// <returns>true, если атака возможна, иначе false.</returns>
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
    protected IEnumerator ReloadWeapon(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        reloading = false;
    }

    // <summary>
    /// Возвращает список тегов целей.
    /// </summary>
    /// <returns>Список строк с тегами целей.</returns>
    public List<string> GetTargetTags()
    {
        return targetTags;
    }

    /// <summary>
    /// Опускает оружие визуально, уменьшая порядок сортировки спрайтов.
    /// </summary>
    private void DownWeaponSprites()
    {
        spritesDowned = true;
        weaponSprites.ForEach(s => s.sortingOrder -= 2);
    }

    /// <summary>
    /// Поднимает оружие визуально, увеличивая порядок сортировки спрайтов.
    /// </summary>
    private void UpWeaponSprites()
    {
        spritesDowned = false;
        weaponSprites.ForEach(s => s.sortingOrder += 2);
    }
}
