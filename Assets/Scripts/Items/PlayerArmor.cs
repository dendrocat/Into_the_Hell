using UnityEngine;

public class PlayerArmor : UpgradableItem
{
    public float damageReduction = 0.05f;
    public float maxHealthBonus = 25f;

    public float getDamageReduction()
    {
        return CalcScale(damageReduction);
    }

    public float getHealthBonus()
    {
        return CalcScale(maxHealthBonus);
    }
}
