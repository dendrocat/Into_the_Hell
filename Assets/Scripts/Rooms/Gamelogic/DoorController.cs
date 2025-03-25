using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Controller for doors.
/// Should handle game logic of doors
/// </summary>
public class DoorController : MonoBehaviour
{
    List<GameObject> _doors;

    void Start()
    {
        GetComponent<DoorTilemapController>().OnDeletedUnusedDoors.AddListener(GetDoors);
    }

    void GetDoors(List<GameObject> doors)
    {
        _doors = doors;
    }

    public void OpenDoors()
    {
        throw new NotImplementedException("Not implemnted open doors");
    }
    public void CloseDoors()
    {
        throw new NotImplementedException("Not implemnted close doors");
    }
}
