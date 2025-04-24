using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WeaponPanel : BasePanel
{
    public UnityEvent<WeaponType> OnWeaponChanged = new();

    [SerializeField] List<Button> _windows;

    void Start()
    {
        var weapon = _inventory.GetPlayerWeapon().name;
        _windows[(int)Enum.Parse<WeaponType>(weapon)].interactable = false;
    }

    void CalcItemState(WeaponType type)
    {
        foreach (var item in _windows)
        {
            item.interactable = true;
        }
        _windows[(int)type].interactable = false;
    }

    public void ChangeWeapon(string type)
    {
        if (!Enum.TryParse<WeaponType>(type, out var weaponType))
        {
            Debug.LogError($"WeaponType: {type} doesn't exist");
            return;
        }

        var weapon = WeaponStorage.Instance.GetWeapon(weaponType);
        weapon.level = WeaponStorage.Instance.GetWeaponLevel(weaponType);
        _inventory.SetPlayerWeapon(weapon);
        weapon.level = 1;

        CalcItemState(weaponType);
        OnWeaponChanged.Invoke(weaponType);
    }
}
