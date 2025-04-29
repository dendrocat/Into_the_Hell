using System.IO;
using UnityEngine;

/// <summary>
/// Handles JSON serialization and file operations for game data
/// </summary>
/// <remarks>
/// Manages save file creation, deletion, and JSON conversion using <see cref="GameData"/> structure.
/// Implements automatic new game initialization when no save exists.
/// </remarks>
public class SaveLoadRepository
{
    private static readonly string _savePath = Path.Combine(
        Application.persistentDataPath, "save.json"
    );

    /// <summary>
    /// Checks for existing save file
    /// </summary>
    /// <returns>True if save file exists at persistentDataPath/save.json</returns>
    public static bool HasSave()
    {
        return File.Exists(_savePath);
    }

    /// <summary>
    /// Serializes game state to JSON file
    /// </summary>
    /// <param name="data">Game state container to serialize</param>
    /// <remarks>
    /// Creates new or overwrites existing save file with pretty-printed JSON
    /// </remarks>
    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_savePath, json);
    }

    /// <summary>
    /// Loads game state from persistent storage
    /// </summary>
    /// <returns>
    /// Existing <see cref="GameData"/> if found, new game state otherwise
    /// </returns>
    public static GameData? Load()
    {
        if (!HasSave())
        {
            return null;
        }

        string json = File.ReadAllText(_savePath);
        return JsonUtility.FromJson<GameData>(json);
    }

    /// <summary>
    /// Deletes persistent save file
    /// </summary>
    /// <remarks>
    /// Use for implementing "New Game" functionality or save reset
    /// </remarks>
    public static void RemoveSave()
    {
        File.Delete(_savePath);
    }

}
