using System;
using UnityEngine;

/// <summary>
/// Содержит данные прогресса игрока для сериализации.
/// </summary>
/// <remarks>
/// Отслеживает экипировку, расходуемые предметы и состояние экономики.
/// Используется совместно с <see cref="GameData"/> через <see cref="SaveLoadRepository"/>.
/// </remarks>
[Serializable]
public struct PlayerData
{
    /// <summary>
    /// Текущий <see cref="WeaponType">тип</see> экипированного оружия.
    /// </summary>
    [Tooltip("Текущий тип экипированного оружия.")]
    public WeaponType weapon;

    /// <summary>
    /// Количество валюты у игрока.
    /// </summary>
    [Tooltip("Количество валюты у игрока.")]
    [Range(0, 9999)] public int money;

    /// <summary>
    /// Уровень зелий.
    /// </summary>
    [Tooltip("Уровень зелий.")]
    [Range(1, 3)] public byte potionLevel;

    /// <summary>
    /// Количество зелий.
    /// </summary>
    [Tooltip("Количество зелий.")]
    [Range(0, 12)] public byte potionCount;

    /// <summary>
    /// Количество стрел для лука.
    /// </summary>
    [Tooltip("Количество стрел для лука.")]
    [Range(0, 99)] public byte arrowCount;

    /// <summary>
    /// Уровень брони.
    /// </summary>
    [Tooltip("Уровень брони.")]
    [Range(1, 5)] public byte armorLevel;
}