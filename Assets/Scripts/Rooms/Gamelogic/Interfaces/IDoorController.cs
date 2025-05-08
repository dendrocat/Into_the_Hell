using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for controlling doors.
/// </summary>
public interface IDoorController
{
    /// <summary>
    /// Set all doors of this room.
    /// </summary>
    /// <param name="doors">A list of <see cref="GameObject"/> instances representing the doors to be managed.</param>
    public void SetDoors(List<GameObject> doors);

    /// <summary>
    /// Opens all doors managed by this controller.
    /// </summary>
    public void OpenDoors();

    /// <summary>
    /// Closes all doors managed by this controller.
    /// </summary>
    public void CloseDoors();
    /// <summary>
    /// Removes all doors managed by this controller.
    /// </summary>
    public void RemoveDoors();
}