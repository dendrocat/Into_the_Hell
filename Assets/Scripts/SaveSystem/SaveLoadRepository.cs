using System.IO;
using UnityEngine;

/// <summary>
/// Обрабатывает сериализацию в JSON и операции с файлами для данных игры.
/// </summary>
/// <remarks>
/// Управляет созданием, удалением файлов сохранения и конвертацией JSON с использованием структуры <see cref="GameData"/>.
/// Реализует автоматическую инициализацию новой игры при отсутствии сохранения.
/// </remarks>
public class SaveLoadRepository
{
    private static readonly string _savePath = Path.Combine(
        Application.persistentDataPath, "save.json"
    );

    /// <summary>
    /// Проверяет наличие файла сохранения.
    /// </summary>
    /// <returns><see langword="true">, если файл сохранения существует по пути persistentDataPath/save.json.</returns>
    public static bool HasSave()
    {
        return File.Exists(_savePath);
    }

    /// <summary>
    /// Сериализует состояние игры в JSON и сохраняет в файл.
    /// </summary>
    /// <param name="data">Контейнер состояния игры для сериализации.</param>
    /// <remarks>
    /// Создаёт новый или перезаписывает существующий файл сохранения с форматированным JSON.
    /// </remarks>
    public static void Save(GameData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_savePath, json);
    }

    /// <summary>
    /// Загружает состояние игры из постоянного хранилища.
    /// </summary>
    /// <returns>
    /// Существующий <see cref="GameData"/>, если найден, иначе null.
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
    /// Удаляет файл сохранения.
    /// </summary>
    /// <remarks>
    /// Используется для реализации функции "Новая игра" или сброса сохранений.
    /// </remarks>
    public static void RemoveSave()
    {
        File.Delete(_savePath);
    }

}
