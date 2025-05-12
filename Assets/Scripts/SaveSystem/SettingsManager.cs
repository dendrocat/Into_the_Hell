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
public class SettingsManager : MonoBehaviour
{
    [Header("Default settings")]
    [SerializeField, Range(0f, 1f)] float defaultVolume = 1.0f;
    [SerializeField, Range(0.5f, 1.5f)] float defaultBrightness = 1.0f;
    [SerializeField, Range(0, 3)] int defaultQualityLevel = 1;
    [SerializeField] bool defaultFullScreen = true;
    int resolutionIndex;
    string rebinds = "";

    static public SettingsManager Instance { get; private set; } = null;

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        resolutionIndex = Screen.resolutions.Length - 1;
        
        Application.targetFrameRate = 60;

        Instance = this;
        settings = new();
        Load();
    }

    Dictionary<SettingsKeys, object> settings;

    public object GetDefaultSetting(SettingsKeys key) {
        switch (key) {
            case SettingsKeys.Brightness:
                return defaultBrightness;
            case SettingsKeys.FullScreen:
                return defaultFullScreen;
            case SettingsKeys.Quality:
                return defaultQualityLevel;
            case SettingsKeys.Rebinds:
                return rebinds;
            case SettingsKeys.Resolution:
                return resolutionIndex;
            case SettingsKeys.Volume:
                return defaultVolume;
        }
        return null;
    }

    public object GetSetting(SettingsKeys key)
    {
        var val = settings.GetValueOrDefault(key, null);
        if (val != null) return val;
        return GetDefaultSetting(key);
    }
    public void SetSetting(SettingsKeys key, object val) { settings[key] = val; }
    public bool RemoveSetting(SettingsKeys key)
    {
        if (!settings.ContainsKey(key))
        {
            Debug.LogError($"Ключ {key} не зарегистрирован");
            return false;
        }
        return settings.Remove(key);
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
        Dictionary<string, object> toSave = new();
        foreach (var i in settings)
            toSave.Add(Enum.GetName(typeof(SettingsKeys), i.Key), i.Value);
        SettingsRepository.Save(toSave);
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
        settings = prefs;
        return prefs;
    }
}
