using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller for managing doors in the scene. Implements the <see cref="IDoorController"/> interface.
/// Handles door detection, opening, and closing logic.
/// </summary>
public class DoorController : MonoBehaviour, IDoorController
{
    /// <summary>
    /// List of door GameObjects found in the scene.
    /// </summary>
    List<GameObject> _doors;

    /// <inheritdoc />
    public void FindDoors()
    {
        _doors = new List<GameObject>();
        var objs = GetComponentsInChildren<Transform>();
        for (int i = 1; i < objs.Length; ++i)
        {
            if (objs[i].CompareTag("Door"))
                _doors.Add(objs[i].gameObject);
        }
    }

    /// <inheritdoc />
    public void OpenDoors()
    {
        Debug.LogError("Not implemnted open doors");
    }

    /// <inheritdoc />
    public void CloseDoors()
    {
        Debug.LogError("Not implemnted close doors");
    }
}
