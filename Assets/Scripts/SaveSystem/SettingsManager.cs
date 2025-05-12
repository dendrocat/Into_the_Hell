using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет сохранением и загрузкой настроек игры с использованием типобезопасного перечисления <see cref="SettingsKeys"/>.
/// </summary>
/// <remarks>
/// Обеспечивает буферизацию настроек перед массовым сохранением через <see cref="SettingsRepository"/>, а также типобезопасную загрузку с использованием ключей-перечислений.
/// </remarks>
public class SettingsManager : MonoBehaviour
{
    [Header("Default settings")]

    [Tooltip("Громкость звука по умолчанию (0.0 - 1.0)")]
    [SerializeField, Range(0f, 1f)] float defaultVolume = 1.0f;

    [Tooltip("Яркость экрана по умолчанию (0.5 - 1.5)")]
    [SerializeField, Range(0.5f, 1.5f)] float defaultBrightness = 1.0f;

    [Tooltip("Уровень качества графики по умолчанию (0 - 3)")]
    [SerializeField, Range(0, 3)] int defaultQualityLevel = 1;

    [Tooltip("Включён ли полноэкранный режим по умолчанию")]
    [SerializeField] bool defaultFullScreen = true;
    int resolutionIndex;
    string rebinds = "";

    /// <summary>
    /// Singleton-экземпляр менеджера настроек.
    /// </summary>
    static public SettingsManager Instance { get; private set; } = null;

    Dictionary<SettingsKeys, object> settings;

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


    /// <summary>
    /// Возвращает значение настройки по умолчанию для указанного ключа.
    /// </summary>
    /// <param name="key">Ключ настройки.</param>
    /// <returns>Значение настройки по умолчанию.</returns>
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

    /// <summary>
    /// Выдает текущее значение настройки.
    /// </summary>
    /// <param name="key">Ключ настройки.</param>
    /// <returns>Значение настройки, либо значение по умолчанию, если настройка не установлена.</returns>
    public object GetSetting(SettingsKeys key)
    {
        var val = settings.GetValueOrDefault(key, null);
        if (val != null) return val;
        return GetDefaultSetting(key);
    }

    /// <summary>
    /// Устанавливает значение настройки.
    /// </summary>
    /// <param name="key">Ключ настройки.</param>
    /// <param name="val">Новое значение настройки.</param>
    public void SetSetting(SettingsKeys key, object val) { settings[key] = val; }

    /// <summary>
    /// Удаляет настройку из текущих значений.
    /// </summary>
    /// <param name="key">Ключ настройки.</param>
    /// <returns><see langword="true"/>, если настройка была удалена; иначе <see langword="false"/>.</returns>
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
    /// Сохраняет все текущие настройки в PlayerPrefs через <see cref="SettingsRepository.Save"/>.
    /// </summary>
    public void Save()
    {
        Dictionary<string, object> toSave = new();
        foreach (var i in settings)
            toSave.Add(Enum.GetName(typeof(SettingsKeys), i.Key), i.Value);
        SettingsRepository.Save(toSave);
    }

    /// <summary>
    /// Загружает все зарегистрированные настройки из PlayerPrefs через <see cref="SettingsRepository.Load"/>.
    /// </summary>
    /// <returns>Словарь с ключами <see cref="SettingsKeys"/> и загруженными значениями.</returns>
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
