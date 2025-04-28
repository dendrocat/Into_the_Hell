using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages game settings persistence using type-safe <see cref="SettingsKeys"/> enumeration.
/// </summary>
/// <remarks>
/// Provides a buffer system for collecting settings before bulk saving through <see cref="SettingsRepository"/>,
/// and type-safe loading using enum keys.
/// </remarks>
public class SettingsManager
{

    Dictionary<string, object> toSave;

    /// <summary>
    /// Initializes a new settings buffer.
    /// </summary>
    public SettingsManager()
    {
        toSave = new Dictionary<string, object>();
    }

    /// <summary>
    /// Adds a setting to the save queue.
    /// </summary>
    /// <param name="key">Setting identifier from <see cref="SettingsKeys"/></param>
    /// <param name="value">Setting value (must match types registered in <see cref="SettingsRepository.TypePrefixes"/>)</param>
    /// <returns>True if added successfully, False if type is unsupported</returns>
    /// <example>
    /// <code>
    /// manager.AddToSave(SettingsKeys.Volume, 0.8f);
    /// </code>
    /// </example>
    public bool AddToSave(SettingsKeys key, object value)
    {
        var type = value.GetType();
        if (!SettingsRepository.TypePrefixes.ContainsKey(type))
        {
            Debug.LogError($"Тип {type.Name} не зарегистрирован");
            return false;
        }
        toSave[Enum.GetName(typeof(SettingsKeys), key)] = value;
        return true;
    }

    /// <summary>
    /// Removes a setting from the save queue.
    /// </summary>
    /// <param name="key">Setting name as string</param>
    /// <returns>True if removed, False if key didn't exist</returns>
    /// <remarks>
    /// Warning: Uses string keys instead of <see cref="SettingsKeys"/> for removal flexibility
    /// </remarks>
    public bool RemoveFromSave(string key)
    {
        if (!toSave.ContainsKey(key))
        {
            Debug.LogError($"Ключ {key} не зарегистрирован");
            return false;
        }
        return toSave.Remove(key);
    }

    /// <summary>
    /// Persists all queued settings to PlayerPrefs through <see cref="SettingsRepository.Save"/>.
    /// </summary>
    /// <remarks>
    /// Clears the save queue after successful persistence.
    /// Automatically calls <see cref="SettingsRepository.Save"/>.
    /// </remarks>
    public void Save()
    {
        SettingsRepository.Save(toSave);
        toSave.Clear();
    }

    /// <summary>
    /// Loads all registered settings from PlayerPrefs via <see cref="SettingsRepository.Load"/>.
    /// </summary>
    /// <returns>Dictionary with <see cref="SettingsKeys"/> as keys and loaded values</returns>
    /// <remarks>
    /// Automatically filters and converts PlayerPrefs data to enum-keyed dictionary
    /// </remarks>
    /// <example>
    /// <code>
    /// var settings = manager.Load();
    /// float volume = (float)settings[SettingsKeys.Volume];
    /// </code>
    /// </example>
    public Dictionary<SettingsKeys, object> Load()
    {
        var loadedPrefs = SettingsRepository.Load(Enum.GetNames(typeof(SettingsKeys)));
        var prefs = new Dictionary<SettingsKeys, object>();

        foreach (var kvp in loadedPrefs)
        {
            if (Enum.TryParse(kvp.Key, out SettingsKeys key))
            {
                prefs[key] = kvp.Value;
            }
            else
            {
                Debug.LogError($"Неизвестный ключ настроек: {kvp.Key}");
            }
        }
        return prefs;
    }
}
