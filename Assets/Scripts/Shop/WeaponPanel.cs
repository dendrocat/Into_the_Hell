using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WeaponPanel : BasePanel
{
    public UnityEvent<WeaponType> WeaponChanged = new();

    [SerializeField] List<Button> _windows;

    void Start()
    {
        var weapon = _inventory.GetPlayerWeapon().GetType().Name;
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
        _inventory.SetPlayerWeapon(
            WeaponStorage.Instance.GetWeapon(weaponType)
        );

        CalcItemState(weaponType);
        WeaponChanged.Invoke(weaponType);
    }
}
