using System;
using UnityEngine;

public abstract class ShopPanel : BasePanel
{
    [SerializeField] ItemsPanel items;

    protected abstract void InitPanel();

    void Start()
    {
        InitPanel();
    }

    protected void SetItemCount(string item, int count)
    {
        items.SetItemCount(item, count);
    }

    protected void SetItemCost(string item, int cost)
    {
        if (cost == -1)
        {
            items.RemoveItem(item);
            return;
        }
        if (cost == -2)
        {
            items.DeactivateItem(item);
        }
        items.SetItemCost(item, cost);
    }

    protected virtual bool CheckBuy(int cost, string name = "")
    {
        return _inventory.GetMoney() >= cost;
    }

    protected void CalcActiveItems()
    {
        items.CalcItemsState(CheckBuy);
    }
}
