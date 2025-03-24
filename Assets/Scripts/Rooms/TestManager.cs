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
        {
            room.SwapTiles(_container);
            if (room is RoomTilemapController r)
            {
                GenerateAdditional(r);
            }
        }
    }

    void GenerateAdditional(RoomTilemapController r)
    {
        var free = r.GetFreeAdditional();
        int cnt = 0;
        float part = free.Count / 16;
        while (cnt++ < part)
        {
            var item = free[Random.Range(0, free.Count)];
            r.SetAdditionalTile(item, _container.Additional);
            free.Remove(item);
        }
    }
}
