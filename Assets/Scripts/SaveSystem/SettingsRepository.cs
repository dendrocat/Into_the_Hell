using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides type-safe PlayerPrefs storage with automatic key prefixing
/// </summary>
public class SettingsRepository
{
    /// <summary>
    /// Maps supported value types to their PlayerPrefs key prefixes.
    /// </summary>
    /// /// <remarks>
    /// Used to differentiate value types in storage.
    /// Contains mappings for: int, float, and bool.
    /// </remarks>
    public static readonly Dictionary<Type, string> TypePrefixes = new()
    {
        { typeof(int), "int_" },
        { typeof(float), "float_" },
        { typeof(bool), "bool_" }
    };

    /// <summary>
    /// Saves a collection of settings to PlayerPrefs.
    /// </summary>
    /// <param name="data">Key-value pairs to store where key is the setting name and value is the setting value</param>
    /// <remarks>
    /// Automatically handles:
    /// - Type prefixing to prevent key collisions
    /// - Type conversion for supported types
    /// - Error logging for unsupported types
    /// </remarks>
    /// <example>
    /// <code>
    /// var settings = new Dictionary<string, object> {
    ///     {"Volume", 0.8f},
    ///     {"Fullscreen", true}
    /// };
    /// SettingsRepository.Save(settings);
    /// </code>
    /// </example>
    public static void Save(IDictionary<string, object> data)
    {
        foreach (var p in data)
        {
            Type type = p.Value.GetType();

            if (!TypePrefixes.TryGetValue(type, out string prefix))
            {
                Debug.LogError($"Тип {type.Name} не зарегистрирован");
                continue;
            }

            string key = prefix + p.Key;

            switch (p.Value)
            {
                case int v: PlayerPrefs.SetInt(key, v); break;
                case float v: PlayerPrefs.SetFloat(key, v); break;
                case bool v: PlayerPrefs.SetInt(key, v ? 1 : 0); break;
                default:
                    Debug.LogError($"Неподдерживаемый тип: {type.Name}");
                    break;
            }
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads settings from PlayerPrefs.
    /// </summary>
    /// <param name="keys">Collection of setting names to load</param>
    /// <returns>Dictionary of loaded values with original keys</returns>
    /// <remarks>
    /// Handles:
    /// - Automatic type detection through prefixes
    /// - Conversion of stored values to original types
    /// - Silent skipping of non-existent keys
    /// </remarks>
    /// <example>
    /// <code>
    /// var loaded = SettingsRepository.Load(new[] {"Volume", "Fullscreen"});
    /// float volume = (float)loaded["Volume"];
    /// bool fullscreen = (bool)loaded["Fullscreen"];
    /// </code>
    /// </example>
    public static Dictionary<string, object> Load(IEnumerable<string> keys)
    {
        var result = new Dictionary<string, object>();

        foreach (string key in keys)
        {
            foreach (var mapping in TypePrefixes)
            {
                string prefixedKey = mapping.Value + key;

                if (!PlayerPrefs.HasKey(prefixedKey)) continue;

                result[key] = mapping.Key switch
                {
                    Type t when t == typeof(int) => PlayerPrefs.GetInt(prefixedKey),
                    Type t when t == typeof(float) => PlayerPrefs.GetFloat(prefixedKey),
                    Type t when t == typeof(bool) => PlayerPrefs.GetInt(prefixedKey) == 1,
                    _ => throw new NotSupportedException()
                };

                break;
            }
        }

        return result;
    }
}
