using UnityEngine;

public abstract class ShopPanel : BasePanel
{
    [SerializeField] ItemsPanel items;

    protected abstract void InitPanel();

    void Start()
    {
        InitPanel();
    }

    protected void SetItemCost(string item, int cost)
    {
        if (cost == -1)
        {
            items.RemoveItem(item);
            return;
        }
        items.SetItemCost(item, cost);
    }

    protected bool CheckBuy(int cost)
    {
        return _inventory.GetMoney() >= cost;
    }

    protected void CalcActiveItems()
    {
        items.CalcItemsState((cost) => CheckBuy(cost));
    }
}
