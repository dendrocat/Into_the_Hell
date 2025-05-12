/// <summary>
/// Управляет сохранением и загрузкой состояния игры между сценами и сессиями.
/// </summary>
/// <remarks>
/// Является связующим звеном между игровыми объектами и <see cref="SaveLoadRepository"/> для сериализации и десериализации данных.
/// </remarks>
public class SaveLoadManager
{
    /// <summary>
    /// Проверяет наличие файла сохранения.
    /// </summary>
    /// <returns><see langword="true">, если файл сохранения существует.</returns>
    public static bool HasSave()
    {
        return SaveLoadRepository.HasSave();
    }

    /// <summary>
    /// Восстанавливает состояние игры из постоянного хранилища.
    /// </summary>
    public static void Load()
    {
        GameStorage.Instance.CurrentGameData = SaveLoadRepository.Load()
                                        ?? GameStorage.Instance.InitialGameData;
    }

    /// <summary>
    /// Сохраняет текущее состояние игры в постоянное хранилище.
    /// </summary>
    public static void Save()
    {
        SaveLoadRepository.Save(GameStorage.Instance.CurrentGameData);
    }

    /// <summary>
    /// Удаляет файл сохранения.
    /// </summary>
    /// <remarks>
    /// Используется для реализации функции "Новая игра" или сброса сохранений.
    /// </remarks>
    public static void RemoveSave()
    {
        SaveLoadRepository.RemoveSave();
    }
}
