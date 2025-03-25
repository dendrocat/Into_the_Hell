using UnityEngine;

public class Potion : UpgradableItem
{
    float baseHeal = 50f;

    private void Start()
    {
        maxLevel = 3;
        scaleCoeff = 1f;
    }

    public float getTotalHeal()
    {
        return CalcScale(baseHeal);
    }
}
