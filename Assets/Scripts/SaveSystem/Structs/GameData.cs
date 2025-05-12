using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Основной контейнер состояния прогресса игры.
/// </summary>
/// <remarks>
/// Содержит позицию на карте, состояние персонажа и инвентарь через <see cref="PlayerData"/>.
/// Сериализуется с помощью <see cref="SaveLoadRepository"/>.
/// </remarks>
[Serializable]
public struct GameData
{
    /// <summary>
    /// Идентификатор локации.
    /// </summary>
    [Tooltip("Идентификатор локации")]
    public Location location;

    /// <summary>
    /// Текущий уровень внутри локации.
    /// </summary>
    [Tooltip("Текущий уровень внутри локации")]
    [Range(0, 10)] public int level;

    /// <summary>
    /// Параметры персонажа, определённые в <see cref="PlayerData"/>.
    /// </summary>
    public PlayerData playerData;

    /// <summary>
    /// Состояния улучшений оружия.
    /// </summary>
    /// <remarks>
    /// Каждый элемент соответствует порядковому значению <see cref="WeaponType"/>.
    /// </remarks>
    [Tooltip("Состояния улучшений оружия (индекс соответствует типу оружия)")]
    [Range(1, 5)] public List<byte> weaponLevels;
}
