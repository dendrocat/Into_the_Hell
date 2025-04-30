using System;
using UnityEngine;

/// <summary>
/// Manages game state persistence between scenes and sessions
/// </summary>
/// <remarks>
/// Bridges between game objects and <see cref="SaveLoadRepository"/> for data serialization.
/// </remarks>
public class SaveLoadManager
{
    public static bool HasSave()
    {
        return SaveLoadRepository.HasSave();
    }

    /// <summary>
    /// Restores game state from persistent storage
    /// </summary>
    public static void Load()
    {
        GameData data = SaveLoadRepository.Load() ??
                        GameStorage.Instance.InitialGameData;

        WeaponStorage.Instance.SetWeaponLevels(data.weaponLevels);
        GameStorage.Instance.location = (Locations)Enum.GetValues(typeof(Locations)).GetValue(data.location);
        GameStorage.Instance.level = data.level;
        GameStorage.Instance.PlayerData = data.playerData;
    }

    /// <summary>
    /// Captures current game state and persists it
    /// </summary>
    public static void Save()
    {
        var data = new GameData();

        data.weaponLevels = WeaponStorage.Instance.GetWeaponLevels();
        data.playerData = GameStorage.Instance.PlayerData;
        data.location = (int)GameStorage.Instance.location;
        data.level = GameStorage.Instance.level;

        SaveLoadRepository.Save(data);
    }

    /// <summary>
    /// Deletes persistent save file
    /// </summary>
    /// <remarks>
    /// Use for implementing "New Game" functionality or save reset
    /// </remarks>
    public static void RemoveSave()
    {
        SaveLoadRepository.RemoveSave();
    }
}
