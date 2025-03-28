using System.Collections.Generic;
using UnityEngine;

public class TestLevelGenerator : MonoBehaviour
{
    [SerializeField] TilesContainer _container;

    [SerializeField] GameObject[] _generated;

    void Awake()
    {
        Generate();
    }

    void Generate()
    {
        var rooms = new List<IRoomTilemapController>();
        var controllers = new List<ITilemapController>();
        foreach (var g in _generated)
        {
            if (!g.activeInHierarchy) continue;
            ITilemapController a = g.GetComponent<ITilemapController>();
            controllers.Add(a);
            if (a is IRoomTilemapController room)
                rooms.Add(room);
        }

        DoDoors(rooms);
        DoTilemap(controllers);
        rooms.ForEach((r) => (r as MonoBehaviour).GetComponent<IRoomController>().DoorController.SetDoors(r.DoorTilemap.ActiveDoors));
        rooms.ForEach((r) => Destroy(r as MonoBehaviour));
    }

    void DoDoors(List<IRoomTilemapController> rooms)
    {
        foreach (var room in rooms)
        {
            room.DoorTilemap.ActivateDoor(DoorDirection.Down);
            room.DoorTilemap.ActivateDoor(DoorDirection.Right);
        }
    }

    void DoTilemap(List<ITilemapController> controllers)
    {
        foreach (var controller in controllers)
        {
            controller.SwapTiles(_container);
            if (controller is IRoomTilemapController room)
            {
                GenerateAdditional(room);
            }
        }
    }

    void GenerateAdditional(IRoomTilemapController room)
    {
        var free = room.GetFreePositions();
        int cnt = 0;
        float part = free.Count / 16;
        while (cnt++ < part)
        {
            var item = free[Random.Range(0, free.Count)];
            room.SetAdditionalTile(_container.Additional, item);
            free.Remove(item);
        }
    }
}
