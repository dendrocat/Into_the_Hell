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
        var data = SaveLoadRepository.Load();

        PlayerStorage.Instance.PlayerData = data.playerData;
        WeaponStorage.Instance.SetWeaponLevels(data.weaponLevels);
        GameManager.Instance.SetPlayerProgress(
            (Locations)Enum.GetValues(typeof(Locations)).GetValue(data.location),
            data.level
        );
    }

    /// <summary>
    /// Captures current game state and persists it
    /// </summary>
    public static void Save()
    {
        var data = new GameData();

        data.weaponLevels = WeaponStorage.Instance.GetWeaponLevels();
        data.playerData = PlayerStorage.Instance.PlayerData;
        (var location, var level) = GameManager.Instance.GetPlayerProgress();
        data.location = (int)location;
        data.level = level;

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
