using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for controlling the tilemap of doors
/// </summary>
public interface IDoorTilemapController : ITilemapController
{
    /// <summary>
    /// Gets a list of currently active door <see cref="GameObjects"/> in the room.
    /// This property provides access to all active door instances, allowing for further
    /// manipulation or tracking of door states during gameplay.
    /// </summary>
    public List<GameObject> ActiveDoors { get; }

    /// <summary>
    /// Activates a door at the specified location based on the given direction.
    /// </summary>
    /// <param name="direction">The direction indicating the location of the door to activate.</param>
    public void ActivateDoor(DoorDirection direction);
}