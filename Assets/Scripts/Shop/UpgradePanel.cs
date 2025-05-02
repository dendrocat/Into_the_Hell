using UnityEngine;
using System;

public class UpgradePanel : ShopPanel
{
    protected override void InitPanel()
    {
        foreach (var type in Enum.GetValues(typeof(WeaponType)))
        {
            SetItemCost(
                type.ToString(),
                WeaponStorage.Instance.GetUpgradeCost((WeaponType)type)
            );
        }
        SetItemCost(
            "Armor",
            _inventory.GetPlayerArmor().GetUpgradeCost()
        );
        SetItemCost(
            "Potion",
            _inventory.GetPotion().GetUpgradeCost()
        );
        CalcActiveItems();
    }

    public void UpgradeWeapon(string type)
    {
        if (!Enum.TryParse<WeaponType>(type, out var weaponType))
        {
            Debug.LogError($"WeaponType: {weaponType} doesn't exist");
            return;
        }
        var cost = WeaponStorage.Instance.GetUpgradeCost(weaponType);
        if (!CheckBuy(cost)) return;

        WeaponStorage.Instance.UpgradeWeapon(weaponType);

        _inventory.ModifyMoneyCount(-cost);

        SetItemCost(
            type,
            WeaponStorage.Instance.GetUpgradeCost(weaponType)
        );
        CalcActiveItems();
    }

    public void UpgradeArmor()
    {
        var armor = _inventory.GetPlayerArmor();
        if (!CheckBuy(armor.GetUpgradeCost()))
            return;

        _inventory.ModifyMoneyCount(-armor.GetUpgradeCost());
        armor.Upgrade(1);

        _inventory.GetComponent<Player>().RecalculateHealth();

        SetItemCost("Armor", armor.GetUpgradeCost());
        CalcActiveItems();
    }

    public void UpgradePotion()
    {
        var potion = _inventory.GetPotion();
        if (!CheckBuy(potion.GetUpgradeCost()))
            return;

        _inventory.ModifyMoneyCount(-potion.GetUpgradeCost());
        potion.Upgrade(1);

        SetItemCost("Potion", potion.GetUpgradeCost());
        CalcActiveItems();
    }
}
