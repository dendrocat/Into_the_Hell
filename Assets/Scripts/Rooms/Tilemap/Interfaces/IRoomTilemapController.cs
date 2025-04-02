using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Interface for objects that manage tilemaps within a room, including doors and other tile-based elements.
/// </summary>
public interface IRoomTilemapController : ITilemapController
{
    /// <summary>
    /// Gets the controller for managing the door tilemap within the room.
    /// </summary>
    /// <value>
    /// An <see cref="IDoorTilemapController"/> instance responsible for door-related tilemap operations.
    /// </value>
    public IDoorTilemapController DoorTilemap { get; }

    /// <summary>
    /// Removes additional tilemap.
    /// </summary>
    public void RemoveAdditional();

    /// <summary>
    /// Retrieves all free positions on the additional tilemap.
    /// </summary>
    /// <returns>A list of free positions as <see cref="Vector2Int"/> coordinates. Returns an empty list if the additional tilemap is not enabled.</returns>
    public List<Vector2Int> GetFreePositions();

    /// <summary>
    /// Places an additional tile at the specified position on the tilemap.
    /// </summary>
    /// <param name="tile">The tile to be placed.</param>
    /// <param name="position">The position where the tile will be placed.</param>
    public void SetAdditionalTile(TileBase tile, Vector2Int position);
}