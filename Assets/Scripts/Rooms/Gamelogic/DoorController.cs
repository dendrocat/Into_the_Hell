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
        _doors = new List<GameObject>();
    }

    void LateUpdate()
    {
        if (_doors.Count == 0)
        {
            GetDoors();
        }
    }

    void GetDoors()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            var d = transform.GetChild(i);
            if (d.GetComponent<Tilemap>() != null || d.IsDestroyed())
                continue;
            _doors.Add(d.gameObject);
        }

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
