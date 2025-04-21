using UnityEngine;
public class ConsumePanel : ShopPanel
{
    [Header("The cost of consumables")]
    [SerializeField] int explosiveArrowCost;

    [SerializeField] int basePotionCost;

    int CalcPotionCost()
    {
        var potion = _inventory.GetPotion();
        return basePotionCost + (potion.level - 1) * 10;
    }

    protected override void InitPanel()
    {
        SetItemCost("Potion", CalcPotionCost());
        SetItemCost("Arrow", explosiveArrowCost);
        CalcActiveItems();
    }

    public void BuyArrow()
    {
        if (!CheckBuy(explosiveArrowCost)) return;
        if (!_inventory.AddExplosiveArrow()) return;
        _inventory.ModifyMoneyCount(-explosiveArrowCost);

        CalcActiveItems();
    }

    public void BuyPotion()
    {
        var cost = CalcPotionCost();
        if (!CheckBuy(cost)) return;
        if (!_inventory.AddPotion()) return;
        _inventory.ModifyMoneyCount(-cost);

        CalcActiveItems();
    }
}
