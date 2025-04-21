using System;
using UnityEngine;

public class WeaponPanel : BasePanel
{
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
    }
}
