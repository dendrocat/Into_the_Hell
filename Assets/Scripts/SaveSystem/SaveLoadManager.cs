using System;
using UnityEngine;

/// <summary>
/// Manages game state persistence between scenes and sessions
/// </summary>
/// <remarks>
/// Bridges between game objects and <see cref="SaveLoadRepository"/> for loaded serialization.
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
        GameStorage.Instance.CurrentGameData = SaveLoadRepository.Load()
                                        ?? GameStorage.Instance.InitialGameData;
    }

    /// <summary>
    /// Captures current game state and persists it
    /// </summary>
    public static void Save()
    {
        SaveLoadRepository.Save(GameStorage.Instance.CurrentGameData);
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
