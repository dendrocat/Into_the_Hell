using System;
using System.Collections.Generic;
using UnityEngine;

public class SettingsRepository
{
    public static readonly Dictionary<Type, string> TypePrefixes = new()
    {
        { typeof(int), "int_" },
        { typeof(float), "float_" },
        { typeof(bool), "bool_" }
    };

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
