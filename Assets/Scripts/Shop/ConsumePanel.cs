using UnityEngine;

/// <summary>
/// Панель покупки расходуемых предметов, наследует <see cref="ShopPanel"/>.
/// Управляет покупкой стрел и зелий с динамическим расчётом стоимости и ограничениями по количеству.
/// </summary>
public class ConsumePanel : ShopPanel
{
    [Header("Стоимость расходников")]

    [Tooltip("Стоимость взрывной стрелы")]
    [SerializeField] int explosiveArrowCost;

    [Tooltip("Базовая стоимость зелий")]
    [SerializeField] int basePotionCost;

    /// <summary>
    /// Рассчитывает текущую стоимость зелья с учётом уровня.
    /// </summary>
    /// <returns><see langword="int"> - cтоимость зелья.</returns>
    int CalcPotionCost()
    {
        var potion = _inventory.GetPotion();
        return basePotionCost + (potion.level - 1) * 10;
    }

    /// <summary>
    /// Инициализирует панель, устанавливая стоимость и количество стрел и зелий.
    /// </summary>
    protected override void InitPanel()
    {
        SetItemCost("Potion", CalcPotionCost());
        SetItemCost("Arrow", explosiveArrowCost);
        SetItemCount("Arrow", _inventory.GetExplosiveArrowCount());
        SetItemCount("Potion", _inventory.GetPotionCount());
        CalcActiveItems();
    }

    /// <summary>
    /// Покупает взрывную стрелу, если хватает денег и не превышен максимум.
    /// </summary>
    public void BuyArrow()
    {
        if (!base.CheckBuy(explosiveArrowCost)) return;
        if (!_inventory.AddExplosiveArrow()) return;
        _inventory.ModifyMoneyCount(-explosiveArrowCost);
        SetItemCount("Arrow", _inventory.GetExplosiveArrowCount());

        if (base.CheckBuy(explosiveArrowCost) && !CheckBuy(explosiveArrowCost, "Arrow"))
            SetItemCost("Arrow", -2);
        CalcActiveItems();
    }

    /// <summary>
    /// Покупает зелье, если хватает денег и не превышен максимум.
    /// </summary>
    public void BuyPotion()
    {
        var cost = CalcPotionCost();
        if (!base.CheckBuy(cost)) return;
        if (!_inventory.AddPotion()) return;
        _inventory.ModifyMoneyCount(-cost);
        SetItemCount("Potion", _inventory.GetPotionCount());

        if (base.CheckBuy(cost) && !CheckBuy(cost, "Potion"))
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
