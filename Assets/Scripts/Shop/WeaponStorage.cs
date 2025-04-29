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
    Dictionary<WeaponType, byte> _weaponLevels;


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
        _weaponLevels = new();
        foreach (var weapon in _weaponMapping)
        {
            _weapons[weapon.type] = weapon.prefab;
            _weaponLevels[weapon.type] = 1;
        }
    }

    public AlternateAttackWeapon GetWeapon(WeaponType type)
    {
        return _weapons[type];
    }

    public List<byte> GetWeaponLevels()
    {
        return new List<byte>(_weaponLevels.Values);
    }

    public void SetWeaponLevels(List<byte> weaponLevels)
    {
        foreach (var key in Enum.GetValues(typeof(WeaponType)))
        {
            _weaponLevels[(WeaponType)key] = weaponLevels[(int)key];
        }
    }

    public int GetUpgradeCost(WeaponType type)
    {
        _weapons[type].level = _weaponLevels[type];
        var cost = _weapons[type].GetUpgradeCost();
        _weapons[type].level = 1;
        return cost;
    }

    public void UpgradeWeapon(WeaponType type)
    {
        _weaponLevels[type]++;
    }
}
