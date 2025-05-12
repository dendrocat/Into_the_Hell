using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Контейнер для префабов комнат, используемых в генерации уровней.
/// Этот класс является ScriptableObject и может быть создан через Unity Editor.
/// </summary>
[CreateAssetMenu(fileName = "RoomContainer", menuName = "Containers/RoomContainer")]
public class RoomContainer : ScriptableObject
{
    /// <summary>
    /// Префаб стартовой комнаты.
    /// </summary>
    [Tooltip("Префаб стартовой комнаты")]
    [SerializeField] GameObject _startRoom;

    /// <summary>
    /// Список префабов обычных комнат.
    /// </summary>
    [Tooltip("Список префабов обычных комнат")]
    [SerializeField] List<GameObject> _rooms;

    /// <summary>
    /// Префаб комнаты босса.
    /// </summary>
    [Tooltip("Префаб комнаты босса")]
    [SerializeField] GameObject _bossRoom;

    /// <summary>
    /// Выдает префаб стартовой комнаты.
    /// </summary>
    public GameObject StartRoom => _startRoom;

    /// <summary>
    /// Выдает список префабов обычных комнат.
    /// </summary>
    public List<GameObject> Rooms => _rooms;

    /// <summary>
    /// Выдает префаб комнаты босса.
    /// </summary>
    public GameObject BossRoom => _bossRoom;
}
