using UnityEngine;
using System;

/// <summary>
/// Панель улучшений, наследует <see cref="ShopPanel"/>.
/// Управляет улучшением оружия, брони и зелий.
/// </summary>
public class UpgradePanel : ShopPanel
{
    /// <summary>
    /// Инициализирует панель, устанавливая стоимость улучшений для всех типов оружия, брони и зелий.
    /// </summary>
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

    /// <summary>
    /// Улучшает оружие указанного типа, если хватает денег.
    /// </summary>
    /// <param name="type">Имя типа оружия.</param>
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
        if (_inventory.GetPlayerWeapon().GetType().Name.Equals(type))
            _inventory.GetPlayerWeapon().Upgrade(1);

        _inventory.ModifyMoneyCount(-cost);

        SetItemCost(
            type,
            WeaponStorage.Instance.GetUpgradeCost(weaponType)
        );
        CalcActiveItems();
    }

    /// <summary>
    /// Улучшает броню игрока, если хватает денег.
    /// </summary>
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

    /// <summary>
    /// Улучшает зелье игрока, если хватает денег.
    /// </summary>
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
