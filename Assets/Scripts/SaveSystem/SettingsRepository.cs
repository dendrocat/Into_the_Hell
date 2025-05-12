using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Обеспечивает типобезопасное хранение настроек в PlayerPrefs с автоматическим добавлением префиксов ключей.
/// </summary>
public class SettingsRepository
{
    /// <summary>
    /// Отображает поддерживаемые типы значений в префиксы ключей PlayerPrefs.
    /// </summary>
    /// <remarks> 
    /// Используется для различения типов значений при сохранении и загрузке.
    /// Поддерживаются типы:
    /// <list type="bullet">
    /// <item>int</item>
    /// <item>float</item>
    /// <item>bool</item>
    /// <item>string</item>
    /// </list>
    /// </remarks>
    public static readonly Dictionary<Type, string> TypePrefixes = new()
    {
        { typeof(int), "int_" },
        { typeof(float), "float_" },
        { typeof(bool), "bool_" },
        { typeof(string), "string_" }
    };

    /// <summary>
    /// Сохраняет коллекцию настроек в PlayerPrefs.
    /// </summary>
    /// <param name="data">Пары "ключ-значение" для сохранения, где ключ - имя настройки, значение - её значение.</param>
    /// <remarks>
    /// Автоматически:
    /// <list type="bullet">
    /// <item><description>Добавляет префиксы для предотвращения конфликтов ключей.</description></item>
    /// <item><description>Конвертирует значения в поддерживаемые типы.</description></item>
    /// <item><description>Логирует ошибки для неподдерживаемых типов.</description></item>
    /// </list>
    /// </remarks>
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
                case string v: PlayerPrefs.SetString(key, v); break;
                default:
                    Debug.LogError($"Неподдерживаемый тип: {type.Name}");
                    break;
            }
        }
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Загружает настройки из PlayerPrefs по списку ключей.
    /// </summary>
    /// <param name="keys">Коллекция имён настроек для загрузки.</param>
    /// <returns>Словарь загруженных значений с исходными ключами.</returns>
    /// <remarks>
    /// Автоматически:
    /// <list type="bullet">
    /// <item><description>Определяет тип значения по префиксу ключа.</description></item>
    /// <item><description>Конвертирует сохранённые данные в исходные типы.</description></item>
    /// <item><description>Пропускает отсутствующие ключи без ошибок.</description></item>
    /// </list>
    /// </remarks>
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
                    Type t when t == typeof(string) => PlayerPrefs.GetString(prefixedKey),
                    _ => throw new NotSupportedException()
                };

                break;
            }
        }

        return result;
    }
}
