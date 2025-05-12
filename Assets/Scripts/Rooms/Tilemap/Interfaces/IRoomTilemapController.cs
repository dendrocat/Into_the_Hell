using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Interface for objects that manage tilemaps within a room, including doors and other tile-based elements.
/// </summary>
public interface IRoomTilemapController : ITilemapController, ISizeGetable
{
    /// <summary>
    /// Получает контроллер для управления тайлмапом дверей внутри комнаты.
    /// </summary>
    public IDoorTilemapController DoorTilemap { get; }

    /// <summary>
    /// Удаляет дополнительные тайлы из дополнительного тайлмапа.
    /// </summary>
    public void RemoveAdditional();

    /// <summary>
    /// Получает все свободные позиции на дополнительном тайлмапе.
    /// </summary>
    /// <returns>Список свободных позиций в виде координат <see cref="Vector2Int"/>. Возвращает пустой список, если дополнительный тайлмап не активен.</returns>
    public List<Vector2Int> GetFreePositions();

    /// <summary>
    /// Устанавливает дополнительный тайл в указанной позиции на тайлмапе.
    /// </summary>
    /// <param name="tile">Тайл, который будет установлен.</param>
    /// <param name="position">Позиция для установки тайла.</param>
    public void SetAdditionalTile(TileBase tile, Vector2Int position);
}