using System;

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
    public int money;

    /// <summary>
    /// Potion effectiveness tier
    /// </summary>
    public byte potionLevel;
    /// <summary>
    /// Quantity of health potions
    /// </summary>
    public byte potionCount;

    /// <summary>
    /// Quantity of arrows for bows
    /// </summary>
    public byte arrowCount;

    /// <summary>
    /// Damage reduction level
    /// </summary>
    public byte armorLevel;
}