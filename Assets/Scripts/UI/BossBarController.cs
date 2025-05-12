using UnityEngine;

/// <summary>
/// Контроллер панели босса, управляющий отображением здоровья, эффектов и реакцией на смерть босса.
/// </summary>
public class BossBarController : MonoBehaviour
{
     [Tooltip("Компонент отображения здоровья босса")]
    [SerializeField] BossHealthBarController _bossHealthBar;

    [Tooltip("Компонент отображения эффектов босса")]
    [SerializeField] EffectBarController _effect;

    /// <summary>
    /// Настраивает отображение эффекта босса в зависимости от оружия босса.
    /// </summary>
    /// <param name="boss"><see cref="Boss">Объект босса</see>.</param>
    void SetBossEffect(Boss boss)
    {
        EffectNames effect = boss.bossWeapon is YetiWeapon ?
                            EffectNames.AttackChain : EffectNames.MiniGolem;
        _effect.SetGetEffectCount(() =>
            boss.getEffectCount(effect)
        );
        _effect.SetEffectDuration(boss.getEffectDuration(effect));
    }

    /// <summary>
    /// Устанавливает босса для панели, подписывается на события изменения здоровья и смерти.
    /// </summary>
    /// <param name="boss"><see cref="Boss">Объект босса</see>.</param>
    public void SetBoss(Boss boss)
    {
        _bossHealthBar.SetBossName(boss.GetBossName());
        _bossHealthBar.SetHealth(boss.getHP(), boss.MaxHealth);
        boss.HealthChanged.AddListener(() =>
        {
            _bossHealthBar.SetHealthSmoothed(boss.getHP(), boss.MaxHealth);
        }
        );
        Person.Died.AddListener(OnBossDied);
        _bossHealthBar.SetPredicateColorChange(() =>
            boss.hasEffect(EffectNames.MiniGolem)
        );
        SetBossEffect(boss);
    }
    void OnDestroy()
    {
        Person.Died.RemoveListener(OnBossDied);
    }

    /// <summary>
    /// Обработчик события смерти персонажа. Если умерший - босс, скрывает панель.
    /// </summary>
    /// <param name="person">Умерший <see cref="Person">персонаж</see>.</param>
    void OnBossDied(Person person)
    {
        if (!(person is Boss)) return;
        gameObject.SetActive(false);
    }
}
