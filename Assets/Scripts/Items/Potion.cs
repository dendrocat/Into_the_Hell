using UnityEngine;

/**
 * <summary>
 * Класс, описывающий зелье игрока.
 * </summary>
 * **/
public class Potion : UpgradableItem
{
    float baseHeal = 50f;

    private void Start()
    {
        maxLevel = 3;
        scaleCoeff = 1f;
    }

    /**
     * <summary>
     * Функция, возвращающая количество хп, восстанавливаемое зельем.
     * </summary>
     * <returns>Количество хп, восстанавливаемое зельем.</returns>
     * **/
    public float getTotalHeal()
    {
        return CalcScale(baseHeal);
    }
}
