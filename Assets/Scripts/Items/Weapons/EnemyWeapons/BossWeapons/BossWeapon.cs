using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
 * <summary>
 * Класс, описывающий оружие босса.
 * </summary>
 * **/
public class BossWeapon : BaseWeapon
{
    /// <summary>
    /// Урон второй атаки.
    /// </summary>
    [Tooltip("Урон второй атаки")]
    [SerializeField] protected float attack2Damage;

    /// <summary>
    /// Базовое время перезарядки второй атаки.
    /// </summary>
    [Tooltip("Базовое время перезарядки второй атаки")]
    [SerializeField] protected float baseAttack2ReloadTime;

    /// <summary>
    /// Базовое время подготовки второй атаки.
    /// </summary>
    [Tooltip("Базовое время подготовки второй атаки")]
    [SerializeField] protected float baseAttack2PrepareAttackTime;

    /// <summary>
    /// Базовое время завершения второй атаки.
    /// </summary>
    [Tooltip("Базовое время завершения второй атаки")]
    [SerializeField] protected float baseAttack2EndAttackTime;

    /// <summary>
    /// Флаг, указывающий, что вторая атака перезаряжается.
    /// </summary>
    bool attack2Reloading = false;

    /// <summary>
    /// Урон третьей атаки.
    /// </summary>
    [Tooltip("Урон третьей атаки")]
    [SerializeField] protected float attack3Damage;

    /// <summary>
    /// Базовое время перезарядки третьей атаки.
    /// </summary>
    [Tooltip("Базовое время перезарядки третьей атаки")]
    [SerializeField] protected float baseAttack3ReloadTime;

    /// <summary>
    /// Базовое время подготовки третьей атаки.
    /// </summary>
    [Tooltip("Базовое время подготовки третьей атаки")]
    [SerializeField] protected float baseAttack3PrepareAttackTime;

    /// <summary>
    /// Базовое время завершения третьей атаки.
    /// </summary>
    [Tooltip("Базовое время завершения третьей атаки")]
    [SerializeField] protected float baseAttack3EndAttackTime;

    /// <summary>
    /// Флаг, указывающий, что третья атака перезаряжается.
    /// </summary>
    bool attack3Reloading = false;


    /// <summary>
    /// Определяет, перезаряжается ли атака 2.
    /// </summary>
    /// <returns><see langword="bool"/> - перезаряжается ли атака 2.</returns>
    public bool Attack2IsReloading()
    {
        return attack2Reloading;
    }

    /// <summary>
    /// Определяет, можно ли сейчас использовать атаку.
    // </summary>
    /// <param name="number">Номер атаки - 1 (0, 1 или 2).</param>
    /// <returns><see langword="bool"/> - результат соответствующей функции CheckAttackConditions</returns>
    public bool CanUseAttack(int number)
    {
        switch (number)
        {
            case 0:
                return CheckAttackConditions();
            case 1:
                return CheckAttack2Conditions();
            case 2:
                return CheckAttack3Conditions();
            default:
                return false;
        }
    }


    /// <summary>
    /// Запускает атаку с заданным номером.
    /// </summary>
    /// <param name="number">Номер атаки - 1 (0, 1 или 2).</param>
    public void LaunchAttackByNumber(int number)
    {
        switch (number)
        {
            case 0:
                LaunchAttack();
                break;
            case 1:
                LaunchAttack2();
                break;
            case 2:
                LaunchAttack3();
                break;
            default:
                Debug.LogWarning(gameObject.name + ": unexpected attack number (" + number + ")");
                break;
        }
    }

    /// <inheritdoc />
    protected override void OnPrepareAttackStart()
    {
        owner.setMoving(false);
        owner.anim.SetTrigger("Attack1");
    }

    /// <inheritdoc />
    protected override void OnEndAttackEnd()
    {
        owner.setMoving(true);
    }


    /// <summary>
    /// Определяет, перезаряжается ли атака 3.
    /// </summary>
    /// <returns><see langword="bool"/> - перезаряжается ли атака 3.</returns>
    public bool Attack3IsReloading()
    {
        return attack3Reloading;
    }

    /// <summary>
    /// Запускает атаку 2, если условия позволяют.
    /// </summary>
    public void LaunchAttack2()
    {
        if (!attack2Reloading && CheckAttack2Conditions())
        {
            attack2Reloading = true;
            StartCoroutine(PerformAttack2());
        }
    }

    /// <summary>
    /// Корутина, выполняющая последовательность действий при атаке 2.
    /// </summary>
    /// <returns>Корутина.</returns>
    private IEnumerator PerformAttack2()
    {
        OnPrepareAttack2Start();
        yield return new WaitForSeconds(CalcScaleDescending(baseAttack2PrepareAttackTime));
        OnPrepareAttack2End();
        Attack2();
        OnEndAttack2Start();
        yield return new WaitForSeconds(CalcScaleDescending(baseAttack2EndAttackTime));
        OnEndAttack2End();
        StartCoroutine(ReloadAttack2(CalcScaleDescending(baseAttack2ReloadTime)));
    }

    /// <summary>
    /// Проверяет условия, позволяющие выполнить атаку 2. Метод для переопределения в классах-наследниках.
    /// </summary>
    /// <returns><see langword="true"/>, если атака 2 возможна, иначе <see langword="false"/>.</returns>
    protected virtual bool CheckAttack2Conditions()
    {
        return true;
    }

    /// <summary>
    /// Метод для выполнения атаки 2. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void Attack2()
    {

    }

    /// <summary>
    /// Действия, выполняемые перед началом подготовки атаки 2. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void OnPrepareAttack2Start()
    {
        owner.setMoving(false);
        owner.anim.SetTrigger("Attack2");
    }

    /// <summary>
    /// Действия, выполняемые после окончания подготовки атаки 2. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void OnPrepareAttack2End()
    {

    }

    /// <summary>
    /// Действия, выполняемые перед началом окончания атаки 2. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void OnEndAttack2Start()
    {

    }

    /// <summary>
    /// Действия, выполняемые после окончания окончания атаки 2. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void OnEndAttack2End()
    {
        owner.setMoving(true);
    }

    /// <summary>
    /// Корутина, выполняющая перезарядку атаки 2 оружия.
    /// </summary>
    /// <param name="reloadTime">Время перезарядки.</param>
    /// <returns>Корутина.</returns>
    private IEnumerator ReloadAttack2(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        attack2Reloading = false;
    }

    /// <summary>
    /// Запускает атаку 3, если условия позволяют.
    /// </summary>
    public void LaunchAttack3()
    {
        if (!attack3Reloading && CheckAttack3Conditions())
        {
            attack3Reloading = true;
            StartCoroutine(PerformAttack3());
        }
    }

    /// <summary>
    /// Корутина, выполняющая последовательность действий при атаке 3.
    /// </summary>
    /// <returns>Корутина.</returns>
    private IEnumerator PerformAttack3()
    {
        OnPrepareAttack3Start();
        yield return new WaitForSeconds(CalcScaleDescending(baseAttack3PrepareAttackTime));
        OnPrepareAttack3End();
        Attack3();
        OnEndAttack3Start();
        yield return new WaitForSeconds(CalcScaleDescending(baseAttack3EndAttackTime));
        OnEndAttack3End();
        StartCoroutine(ReloadAttack3(CalcScaleDescending(baseAttack3ReloadTime)));
    }

    /// <summary>
    /// Проверяет условия, позволяющие выполнить атаку 3. Метод для переопределения в классах-наследниках.
    /// </summary>
    /// <returns><see langword="true"/>, если атака 3 возможна, иначе <see langword="false"/>.</returns>
    protected virtual bool CheckAttack3Conditions()
    {
        return true;
    }

    /// <summary>
    /// Метод для выполнения атаки 3. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void Attack3()
    {

    }

    /// <summary>
    /// Действия, выполняемые перед началом подготовки атаки 3. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void OnPrepareAttack3Start()
    {
        owner.setMoving(false);
        owner.anim.SetTrigger("Attack3");
    }

    /// <summary>
    /// Действия, выполняемые после окончания подготовки атаки 3. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void OnPrepareAttack3End()
    {

    }

    /// <summary>
    /// Действия, выполняемые перед началом окончания атаки 3. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void OnEndAttack3Start()
    {

    }

    /// <summary>
    /// Действия, выполняемые после окончания окончания атаки 3. Метод для переопределения в классах-наследниках.
    /// </summary>
    protected virtual void OnEndAttack3End()
    {
        owner.setMoving(true);
    }

    /// <summary>
    /// Корутина, выполняющая перезарядку атаки 3 оружия.
    /// </summary>
    /// <param name="reloadTime">Время перезарядки.</param>
    /// <returns>Корутина.</returns>
    private IEnumerator ReloadAttack3(float reloadTime)
    {
        yield return new WaitForSeconds(reloadTime);
        attack3Reloading = false;
    }

    /**
     * <summary>
     * Метод для поиска целей для ближних атак босса.
     * </summary>
     * <param name="angle">Угол ближней атаки.</param>
     * <param name="range">Расстояние ближней атаки.</param>
     * <returns>Список целей для атаки.</returns>
     * **/
    protected List<IDamagable> FindTargetsForAttack(float range, float angle)
    {
        List<IDamagable> targets = new List<IDamagable>(); //поиск целей для атаки
        //Person[] possibleTargets = FindObjectsByType<Person>(FindObjectsSortMode.None);

        List<Collider2D> possibleTargets = Physics2D.OverlapCircleAll(owner.transform.position, range).ToList<Collider2D>();

        foreach (Collider2D candidate in possibleTargets)
        { // перебор возможных целей
            IDamagable damagable = candidate.GetComponent<IDamagable>();
            if (!targetTags.Contains(candidate.gameObject.tag)) continue; //не реагируем на цели, не соответствующие тегу

            Vector2 candidatePosition = candidate.transform.position;
            Vector2 candidateDirection = candidatePosition - (Vector2)transform.position;

            if (candidateDirection.magnitude <= range)
            {
                float candidateAngle = Vector2.Angle(candidateDirection, owner.weaponDirection);
                if (candidateAngle <= angle / 2)
                {
                    targets.Add(damagable);
                }
            }
        }

        return targets;
    }
}
