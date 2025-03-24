using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    [SerializeField] TilesContainer _container;

    [SerializeField] TilemapController[] _rooms;

    void Start()
    {
        _rooms = FindObjectsByType<TilemapController>(FindObjectsSortMode.InstanceID);

        foreach (var room in _rooms)
            room.SwapTiles(_container);
    }
}
