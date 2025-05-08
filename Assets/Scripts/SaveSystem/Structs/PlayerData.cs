using System;
using UnityEngine;

/// <summary>
/// Contains player-specific progress data for serialization
/// </summary>
/// <remarks>
/// Tracks combat equipment, consumables, and economy state. 
/// Used with <see cref="GameData"/> through <see cref="SaveLoadRepository"/>.
/// </remarks>
[Serializable]
public struct PlayerData
{
    /// <summary>
    /// Currently equipped weapon type
    /// </summary>
    /// <seealso cref="WeaponType"/>
    public WeaponType weapon;

    /// <summary>
    /// Player's currency amount
    /// </summary>
    [Range(0, 9999)]
    public int money;

    /// <summary>
    /// Potion effectiveness tier
    /// </summary>
    [Range(1, 3)]
    public byte potionLevel;
    /// <summary>
    /// Quantity of health potions
    /// </summary>
    [Range(0, 12)]
    public byte potionCount;

    /// <summary>
    /// Quantity of arrows for bows
    /// </summary>
    [Range(0, 99)]
    public byte arrowCount;

    /// <summary>
    /// Damage reduction level
    /// </summary>
    [Range(1, 5)]
    public byte armorLevel;
}