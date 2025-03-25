using System.Collections.Generic;
using UnityEngine;

public class TestLevelManager : MonoBehaviour
{
    [SerializeField] TilesContainer _container;

    [SerializeField] GameObject[] _generated;

    void Start()
    {
        var rooms = new List<RoomTilemapController>();
        var controllers = new List<TilemapController>();
        foreach (var g in _generated)
        {
            if (!g.activeInHierarchy) continue;
            TilemapController a = g.GetComponent<TilemapController>();
            controllers.Add(a);
            if (a is RoomTilemapController room)
                rooms.Add(room);
        }

        DoDoors(rooms);
        DoTilemap(controllers);
        rooms.ForEach((el) => el.DestroyUnused());
    }

    void DoDoors(List<RoomTilemapController> rooms)
    {
        foreach (var room in rooms)
        {
            room.ActivateDoor(DoorDirection.Down);
            room.ActivateDoor(DoorDirection.Right);
        }
    }

    void DoTilemap(List<TilemapController> controllers)
    {
        foreach (var controller in controllers)
        {
            controller.SwapTiles(_container);
            if (controller is RoomTilemapController room)
            {
                GenerateAdditional(room);
            }
        }
    }

    void GenerateAdditional(RoomTilemapController room)
    {
        var free = room.GetFreeAdditional();
        int cnt = 0;
        float part = free.Count / 16;
        while (cnt++ < part)
        {
            var item = free[UnityEngine.Random.Range(0, free.Count)];
            room.SetAdditionalTile(item, _container.Additional);
            free.Remove(item);
        }
    }
}
