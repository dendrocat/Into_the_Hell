using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for controlling doors.
/// </summary>
public interface IDoorController
{
    /// <summary>
    /// Finds all doors tagged with "Door" in the children of this <see cref="GameObject"/>.
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
}