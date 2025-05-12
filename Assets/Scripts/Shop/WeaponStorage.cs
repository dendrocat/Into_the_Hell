using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Хранилище оружия, обеспечивающее доступ к префабам оружия и управление их уровнями.
/// </summary>
public class WeaponStorage : MonoBehaviour
{
    [Serializable]
    struct WeaponMapping
    {
        public WeaponType type;
        public AlternateAttackWeapon prefab;
    }

    /// <summary>
    /// Singleton-экземпляр хранилища оружия.
    /// </summary>
    public static WeaponStorage Instance { get; private set; }

    Dictionary<WeaponType, AlternateAttackWeapon> _weapons;

    [Tooltip("Сопоставление типов оружия с их префабами")]
    [SerializeField] List<WeaponMapping> _weaponMapping;

    /// <summary>
    /// Инициализация словаря для быстрого доступа
    /// </summary>
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

    /// <summary>
    /// Выдает префаб оружия по типу.
    /// </summary>
    /// <param name="type">Тип оружия.</param>
    /// <returns>Префаб оружия.</returns>
    public AlternateAttackWeapon GetWeapon(WeaponType type)
    {
        return _weapons[type];
    }

    /// <summary>
    /// Выдает список уровней улучшений всех оружий.
    /// </summary>
    /// <returns>Список уровней оружия в порядке перечисления <see cref="WeaponType"/>.</returns>
    public List<byte> GetWeaponLevels()
    {
        List<byte> weaponLevels = new();
        foreach (var item in _weapons)
        {
            weaponLevels.Add(item.Value.level);
        }
        return weaponLevels;
    }

    /// <summary>
    /// Устанавливает уровни улучшений оружия из списка.
    /// </summary>
    /// <param name="weaponLevels">Список уровней, индекс соответствует <see cref="WeaponType"/>.</param>
    public void SetWeaponLevels(List<byte> weaponLevels)
    {
        foreach (var key in Enum.GetValues(typeof(WeaponType)))
        {
            _weapons[(WeaponType)key].level = weaponLevels[(int)key];
        }
    }

    /// <summary>
    /// Выдает стоимость улучшения оружия указанного типа.
    /// </summary>
    /// <param name="type">Тип оружия.</param>
    /// <returns><see langword="int"/> - cтоимость улучшения.</returns>
    public int GetUpgradeCost(WeaponType type)
    {
        return _weapons[type].GetUpgradeCost();
    }

    /// <summary>
    /// Улучшает оружие указанного типа на 1 уровень.
    /// </summary>
    /// <param name="type">Тип оружия.</param>
    public void UpgradeWeapon(WeaponType type)
    {
        _weapons[type].Upgrade(1);
    }
}
