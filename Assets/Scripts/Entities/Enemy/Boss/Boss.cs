using System.Collections;
using UnityEngine;


/// <summary>
/// Класс, описывающий босса.
/// </summary>
public class Boss : BaseEnemy
{
    /// <summary>
    /// Имя босса.
    /// </summary>
    [Tooltip("Имя босса")]
    [SerializeField] string bossName;

    /// <summary>
    /// Оружие босса с несколькими типами атак.
    /// </summary>
    [Tooltip("Оружие босса")]
    public BossWeapon bossWeapon;

    /// <summary>
    /// Флаг, разрешающий выполнение атаки.
    /// </summary>
    bool canAttack = true;

    /// <summary>
    /// Получить имя босса.
    /// </summary>
    /// <returns><see langword="string"/> - Имя босса.</returns>
    public string GetBossName()
    {
        return bossName;
    }

    /// <summary>
    /// Инициализация босса, запуск звука начала боя и установка задержки уничтожения.
    /// </summary>
    protected override void Start()
    {
        base.Start();
        destructionDelay = 4f;
        StartCoroutine(PlayStartSound());
    }

    /// <summary>
    /// Корутина воспроизведения звука начала боя с блокировкой атак на время звука.
    /// </summary>
    IEnumerator PlayStartSound()
    {
        canAttack = false;
        setMoving(false);
        _audioPlayer.Play("Start");
        yield return new WaitForSeconds(_audioPlayer.CurrentClipLength);
        setMoving(true);
        canAttack = true;
    }

    /// <summary>
    /// Выполнение атаки босса.
    /// </summary>
    /// <remarks>
    /// Приоритет отдаётся атакам с более высоким номером (3 и 2).
    /// Проверяется готовность каждой атаки и её перезарядка.
    /// </remarks>
    new public void Attack()
    {
        if (!isAlive() || !canAttack) return;
        bool[] reloading = new bool[3];
        reloading[0] = bossWeapon.isReloading();
        reloading[1] = bossWeapon.Attack2IsReloading();
        reloading[2] = bossWeapon.Attack3IsReloading();

        int selectedWeapon = 2;

        for (; selectedWeapon >= 0; selectedWeapon--) //перебираем атаки с конца, так как приоритет выше у 3 и 2 атаки
        {
            if (bossWeapon.CanUseAttack(selectedWeapon) && !reloading[selectedWeapon])
            {
                _audioPlayer.Play($"Attack{selectedWeapon + 1}");
                bossWeapon.LaunchAttackByNumber(selectedWeapon);
                break;
            }
        }
    }
}
