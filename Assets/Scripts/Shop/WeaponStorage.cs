using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStorage : MonoBehaviour
{
    [Serializable]
    struct TypePrefab
    {
        public WeaponType type;
        public AlternateAttackWeapon prefab;
    }


    public static WeaponStorage Instance { get; private set; }

    Dictionary<WeaponType, AlternateAttackWeapon> _weapons;
    Dictionary<WeaponType, byte> _weaponLevels;


    [SerializeField] List<TypePrefab> _weaponList;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        _weapons = new();
        _weaponLevels = new();
        foreach (var weapon in _weaponList)
        {
            _weapons[weapon.type] = weapon.prefab;
            _weaponLevels[weapon.type] = 1;
        }
    }

    public AlternateAttackWeapon GetWeapon(WeaponType type)
    {
        return _weapons[type];
    }

    public byte GetWeaponLevel(WeaponType type)
    {
        return _weaponLevels[type];
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
