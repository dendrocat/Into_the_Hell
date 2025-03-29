using System.Collections;
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

    // <summary>
    /// Called when the game starts. Initiates a coroutine to check the doors.
    /// </summary>
    void Start()
    {
        StartCoroutine(CheckDoors());
    }

    /// <summary>
    /// <see cref="Coroutine"/> to verify the presence and validity of the door list.
    /// </summary>
    /// <returns><see cref="IEnumerator"/> for use in coroutines.</returns>
    IEnumerator CheckDoors()
    {
        yield return null;
        if (_doors == null)
        {
            Debug.LogError($"{GetType()}: The doors were not setted.");
        }
        else if (_doors.Count == 0)
        {
            Debug.LogError($"{GetType()}: In the room must be at least one door.");
        }
    }

    /// <inheritdoc />
    public void SetDoors(List<GameObject> doors)
    {
        _doors = doors;
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
