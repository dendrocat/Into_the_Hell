using UnityEngine;

// <summary>
/// Класс, описывающий броню игрока.
/// </summary>
public class PlayerArmor : UpgradableItem
{
    /// <summary>
    /// Базовый коэффициент снижения урона, предоставляемый бронёй.
    /// </summary>
    [Tooltip("Базовый коэффициент снижения урона, предоставляемый бронёй")]
    [SerializeField] float damageReduction = 0.05f;

    /// <summary>
    /// Бонус к максимальному здоровью, предоставляемый бронёй.
    /// </summary>
    [Tooltip("Бонус к максимальному здоровью, предоставляемый бронёй")]
    [SerializeField] float maxHealthBonus = 25f;

    /// <summary>
    /// Конструктор, инициализирующий коэффициент масштабирования.
    /// </summary>
    public PlayerArmor()
    {
        scaleCoeff = 1f;
    }

    /// <summary>
    /// Возвращает значение снижения урона с учётом улучшений.
    /// </summary>
    /// <returns><see langword="float"/> - коэффициент снижения урона.</returns>
    public float getDamageReduction()
    {
        return CalcScale(damageReduction);
    }

    /// <summary>
    /// Возвращает бонус к максимальному здоровью с учётом улучшений.
    /// </summary>
    /// <returns><see langword="float"/> - бонус к максимальному здоровью.</returns>
    public float getHealthBonus()
    {
        return CalcScale(maxHealthBonus);
    }
}
