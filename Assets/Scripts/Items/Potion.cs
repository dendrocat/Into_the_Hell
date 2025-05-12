/// <summary>
/// Класс, описывающий зелье игрока.
/// </summary>
public class Potion : UpgradableItem
{
    /// <summary>
    /// Базовое количество здоровья, восстанавливаемое зельем.
    /// </summary>
    float baseHeal = 50f;

    /// <summary>
    /// Базовое время перезарядки зелья (в секундах).
    /// </summary>
    float baseReloadTime = 10f;

     /// <summary>
    /// Конструктор, инициализирующий параметры улучшения предмета.
    /// </summary>
    public Potion()
    {
        maxLevel = 3;
        scaleCoeff = 1f;
        minValueDescending = 0.8f;
    }

    /// <summary>
    /// Функция, возвращающая количество хп, восстанавливаемое зельем.
    /// </summary>
    /// <returns><see langword="float"/> - Количество хп, восстанавливаемое зельем.</returns>
    public float getTotalHeal()
    {
        return CalcScale(baseHeal);
    }

    /// <summary>
    /// Функция, возвращающая время перезарядки зелья.
    /// </summary>
    /// <returns><see langword="float"/> - Время перезарядки зелья в секундах.</returns>
    public float getReloadTime()
    {
        return CalcScaleDescending(baseReloadTime);
    }
}
