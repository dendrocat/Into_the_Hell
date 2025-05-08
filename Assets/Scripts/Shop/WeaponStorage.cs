using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStorage : MonoBehaviour
{
    [Serializable]
    struct WeaponMapping
    {
        public WeaponType type;
        public AlternateAttackWeapon prefab;
    }


    public static WeaponStorage Instance { get; private set; }

    Dictionary<WeaponType, AlternateAttackWeapon> _weapons;


    [SerializeField] List<WeaponMapping> _weaponMapping;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _weapons = new();
        foreach (var weapon in _weaponMapping)
        {
            _weapons[weapon.type] = weapon.prefab;
        }
    }

    public AlternateAttackWeapon GetWeapon(WeaponType type)
    {
        return _weapons[type];
    }

    public List<byte> GetWeaponLevels()
    {
        List<byte> weaponLevels = new();
        foreach (var item in _weapons)
        {
            weaponLevels.Add(item.Value.level);
        }
        return weaponLevels;
    }

    public void SetWeaponLevels(List<byte> weaponLevels)
    {
        foreach (var key in Enum.GetValues(typeof(WeaponType)))
        {
            _weapons[(WeaponType)key].level = weaponLevels[(int)key];
        }
    }

    public int GetUpgradeCost(WeaponType type)
    {
        var cost = _weapons[type].GetUpgradeCost();
        return cost;
    }

    public void UpgradeWeapon(WeaponType type)
    {
        _weapons[type].Upgrade(1);
    }
}
