using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TestWeaponStorage : MonoBehaviour
{
    [Serializable]
    private struct WeaponMapping
    {
        public WeaponType type;
        public GameObject prefab;
    }

    public static TestWeaponStorage Instance { get; private set; }

    [SerializeField] List<WeaponMapping> weaponMappings;

    private Dictionary<WeaponType, GameObject> _weaponCache;
    Dictionary<WeaponType, byte> _weaponLevels;

    void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"Instance {GetType()} уже существует");
            return;
        }
        Instance = this;

        _weaponCache = new();
        foreach (var mapping in weaponMappings)
        {
            _weaponCache[mapping.type] = mapping.prefab;
        }
        _weaponLevels = new();
    }

    public AlternateAttackWeapon GetWeapon(WeaponType type)
    {
        return _weaponCache.TryGetValue(type, out GameObject prefab)
            ? prefab.GetComponent<AlternateAttackWeapon>()
            : null;
    }

    public List<byte> GetWeaponLevels()
    {
        return new List<byte>(_weaponLevels.Values);
    }

    public void SetWeaponLevels(List<byte> weaponLevels)
    {
        if (weaponLevels == null)
        {
            foreach (var key in Enum.GetValues(typeof(WeaponType)))
            {
                _weaponLevels[(WeaponType)key] = 1;
            }
            return;
        }
        foreach (var key in Enum.GetValues(typeof(WeaponType)))
        {
            var level = weaponLevels[(int)key];
            _weaponLevels[(WeaponType)key] = level == 0 ? (byte)1 : level;
        }
    }
}
