using UnityEngine;

/// <summary>
/// Менеджер уровня, наследующийся от <see cref="AbstractLevelManager"/>.
/// Отвечает за инициализацию и генерацию текущего уровня.
/// </summary>
public class LevelManager : AbstractLevelManager
{
    /// <summary>
    /// Текущий уровень.
    /// </summary>
    Level _level;

    /// <summary>
    /// Инициализация уровня: создание нового экземпляра <see cref="Level"/> с корневым объектом "Rooms".
    /// </summary>
    protected override void InitLevel()
    {
        _level = new(new GameObject("Rooms").transform);
    }

    /// <summary>
    /// Генерация уровня.
    /// Если уровень уже создан, метод завершает выполнение.
    /// Иначе запускает процесс генерации комнат, холлов, активации уровня и обновления UI.
    /// </summary>
    public override void Generate()
    {
        if (_level.created()) return;

        Generator.New()
                .SetUpRooms(_levelStorage.GetRoomContainer(_location), isLastLevel)
                .Halls(_levelStorage.GetHalls())
                .GenerateRooms(15, _level)
                .ActivateLevel(_level,
                    _levelStorage.GetTilesContainer(_location),
                    _levelStorage.GetTrapContainer(_location)
                );

        UIManager.Instance.GenerateMap(_level);
    }
}