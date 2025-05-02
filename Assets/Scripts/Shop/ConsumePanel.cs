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
        SetItemCount("Arrow", _inventory.GetExplosiveArrowCount());
        SetItemCount("Potion", _inventory.GetPotionCount());
        CalcActiveItems();
    }

    public void BuyArrow()
    {
        if (!base.CheckBuy(explosiveArrowCost)) return;
        if (!_inventory.AddExplosiveArrow()) return;
        _inventory.ModifyMoneyCount(-explosiveArrowCost);
        SetItemCount("Arrow", _inventory.GetExplosiveArrowCount());

        if (!CheckBuy(explosiveArrowCost, "Arrow"))
            SetItemCost("Arrow", -1);
        CalcActiveItems();
    }

    public void BuyPotion()
    {
        var cost = CalcPotionCost();
        if (!base.CheckBuy(cost)) return;
        if (!_inventory.AddPotion()) return;
        _inventory.ModifyMoneyCount(-cost);
        SetItemCount("Potion", _inventory.GetPotionCount());

        if (!CheckBuy(CalcPotionCost(), "Potion"))
            SetItemCost("Potion", -1);
        CalcActiveItems();
    }

    protected override bool CheckBuy(int cost, string name)
    {
        var res = base.CheckBuy(cost);
        switch (name)
        {
            case "Arrow":
                res &= _inventory.GetExplosiveArrowCount() < _inventory.MaxExplosiveArrowCount;
                break;
            case "Potion":
                res &= _inventory.GetPotionCount() < _inventory.MaxPotionsCount;
                break;
        }
        return res;
    }
}
