using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Хранилище данных уровней, содержащее информацию о локациях, тайлах, комнатах, ловушках и врагах.
/// Этот класс является ScriptableObject и может быть создан через Unity Editor.
/// </summary>
[CreateAssetMenu(fileName = "LevelStorage", menuName = "Storages/Level Storage")]
public class LevelStorage : ScriptableObject
{
    /// <summary>
    /// Контейнер данных для конкретной локации.
    /// </summary>
    [Serializable]
    struct LocationContainer
    {
        /// <summary>
        /// Локация.
        /// </summary>
        [Tooltip("Локация")]
        public Location location;

        /// <summary>
        /// Контейнер тайлов для локации.
        /// </summary>
        [Tooltip("Контейнер тайлов для локации")]
        public TilesContainer tilesContainer;

        /// <summary>
        /// Контейнер комнат для локации.
        /// </summary>
        [Tooltip("Контейнер комнат для локации")]
        public RoomContainer roomContainer;

        // <summary>
        /// Контейнер ловушек для локации.
        /// </summary>
        [Tooltip("Контейнер ловушек для локации")]
        public TrapContainer trapContainer;

        /// <summary>
        /// Контейнер врагов для локации.
        /// </summary>
        [Tooltip("Контейнер врагов для локации")]
        public EnemyContainer enemyContainer;
    }

    /// <summary>
    /// Список контейнеров локаций.
    /// Индексы соответствуют значениям перечисления <see cref="Location"/>.
    /// </summary>
    [Tooltip("Список контейнеров локаций")]
    [SerializeField] List<LocationContainer> _location;

    /// <summary>
    /// Список префабов коридоров (холлов).
    /// </summary>
    [Tooltip("Список префабов коридоров (холлов)")]
    [SerializeField] List<GameObject> _halls;

    /// <summary>
    /// Выдает контейнер тайлов для указанной локации.
    /// </summary>
    public TilesContainer GetTilesContainer(Location location)
    {
        return _location[(int)location].tilesContainer;
    }

    /// <summary>
    /// Выдает контейнер комнат для указанной локации.
    /// </summary>
    public RoomContainer GetRoomContainer(Location location)
    {
        return _location[(int)location].roomContainer;
    }

    /// <summary>
    /// Выдает список префабов коридоров (холлов).
    /// </summary>
    public List<GameObject> GetHalls()
    {
        return _halls;
    }

    /// <summary>
    /// Выдает контейнер ловушек для указанной локации.
    /// </summary>
    public TrapContainer GetTrapContainer(Location location)
    {
        return _location[(int)location].trapContainer;
    }

    /// <summary>
    /// Выдает контейнер врагов для указанной локации.
    /// </summary>
    public EnemyContainer GetEnemyContainer(Location location)
    {
        return _location[(int)location].enemyContainer;
    }
}
