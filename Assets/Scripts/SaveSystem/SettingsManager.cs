using System;
using System.Collections.Generic;
using UnityEngine;

public enum SettingsKeys
{
    Volume,
    InvertY, Sensitivity,
    Brightness, Quality, FullScreen,
    Resolution
}
public class SettingsManager
{

    Dictionary<string, object> toSave;

    public SettingsManager()
    {
        toSave = new Dictionary<string, object>();
    }

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

    public bool RemoveFromSave(string key)
    {
        if (!toSave.ContainsKey(key))
        {
            Debug.LogError($"Ключ {key} не зарегистрирован");
            return false;
        }
        return toSave.Remove(key);
    }

    public void Save()
    {
        SettingsRepository.Save(toSave);
        toSave.Clear();
    }

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
