using UnityEngine;

/// <summary>
/// Класс, описывающий улучшаемый предмет.
/// </summary>
public class UpgradableItem : MonoBehaviour
{
    /// <summary>
    /// Коэффициент масштабирования. Задается в классах-наследниках в методе Awake.
    /// </summary>
    protected float scaleCoeff = 0.2f;

    /// <summary>
    /// Доля, которая остается при уменьшающем масштабировании на максимальном уровне.
    /// </summary>
    protected float minValueDescending = 0.5f;

    /// <summary>
    /// Текущий уровень предмета.
    /// </summary>
    public byte level;

    /// <summary>
    /// Максимальный уровень предмета.
    /// </summary>
    public byte maxLevel;

    /// <summary>
    /// Возвращает стоимость улучшения на следующий уровень. Если предмет уже максимально улучшен, возвращает -1.
    /// </summary>
    /// <returns><see langword="int"/> - Стоимость улучшения или -1.</returns>
    public int GetUpgradeCost()
    {
        if (level < maxLevel) return 100 * level;
        else return -1;
    }

    /// <summary>
    /// Возвращает значение scalable, умноженное согласно текущему уровню предмета.
    /// </summary>
    /// <param name="scalable">Значение, которое необходимо масштабировать.</param>
    /// <returns><see langword="float"/> - Масштабированное значение.</returns>
    public float CalcScale(float scalable)
    {
        //Debug.Log(gameObject.name + ": initial=" + scalable + ", calculated=" + (scalable * (1f + (level - 1) * scaleCoeff)));
        return scalable * (1f + (level - 1) * scaleCoeff);
    }

    /// <summary>
    /// Возвращает значение scalable, уменьшенное согласно текущему уровню предмета.
    /// </summary>
    /// <param name="scalable">Значение, которое необходимо уменьшить.</param>
    /// <returns><see langword="float"/> - уменьшенное значение.</returns>
    public float CalcScaleDescending(float scalable)
    {
        if (level == 1) return scalable;
        float coeff = minValueDescending / (maxLevel - 1);
        return scalable * (1f - (level - 1) * coeff);
    }

    /// <summary>
    /// Функция, отвечающая за апгрейд предмета на указанное количество уровней.
    /// </summary>
    /// <param name="diff">Количество уровней, на которое нужно улучшить предмет.</param>
    public void Upgrade(byte diff)
    {
        level = (byte)Mathf.Clamp(level + diff, 1, maxLevel);
    }
}
