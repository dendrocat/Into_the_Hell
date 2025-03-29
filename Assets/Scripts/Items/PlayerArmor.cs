using UnityEngine;

public class PlayerArmor : UpgradableItem
{
    [SerializeField] float damageReduction = 0.05f;
    [SerializeField] float maxHealthBonus = 25f;

    void Start()
    {
        scaleCoeff = 1f;
    }

    public float getDamageReduction()
    {
        return CalcScale(damageReduction);
    }

    public float getHealthBonus()
    {
        return CalcScale(maxHealthBonus);
    }
}
