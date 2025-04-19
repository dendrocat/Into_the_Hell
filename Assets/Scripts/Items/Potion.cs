using UnityEngine;

/**
 * <summary>
 * Класс, описывающий зелье игрока.
 * </summary>
 * **/
public class Potion : UpgradableItem
{
    float baseHeal = 50f;
    float baseReloadTime = 10f;

    public Potion()
    {
        maxLevel = 3;
        scaleCoeff = 1f;
        minValueDescending = 0.8f;
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

    /**
     * <summary>
     * Функция, возвращающая время перезарядки зелья.
     * </summary>
     * <returns>Время перезарядки зелья.</returns>
     * **/
    public float getReloadTime()
    {
        return CalcScaleDescending(baseReloadTime);
    }
}
