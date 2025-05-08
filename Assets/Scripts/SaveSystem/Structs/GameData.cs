using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main container for game progress state
/// </summary>
/// <remarks>
/// Contains map position, character state, and inventory through <see cref="PlayerData"/>.
/// Serialized via <see cref="SaveLoadRepository"/>.
/// </remarks>
[Serializable]
public struct GameData
{
    /// <summary>Current location ID</summary>
    public Location location;

    /// <summary>Current level in location</summary>
    [Range(0, 10)]
    public int level;

    /// <summary>Character-specific parameters defined in <see cref="PlayerData"/></summary>
    public PlayerData playerData;

    /// <summary>Weapon upgrade states</summary>
    /// <remarks>
    /// Each byte corresponds to <see cref="WeaponType"/> ordinal value
    /// </remarks>
    [Range(1, 5)]
    public List<byte> weaponLevels;
}
