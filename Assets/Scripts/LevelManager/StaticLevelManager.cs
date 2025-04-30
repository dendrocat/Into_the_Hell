using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StaticLevelManager : MonoBehaviour, ILevelManager
{
    public static StaticLevelManager Instance { get; private set; } = null;
    [SerializeField] List<RoomTilemapController> _rooms;

    [SerializeField] List<HallTilemapController> _halls;

    void GenerateAdditional(
        IRoomTilemapController room,
        TileBase tile
    )
    {
        var free = room.GetFreePositions();
        int cnt = 0;
        float part = free.Count / 8;
        while (cnt++ < part)
        {
            var item = free[Random.Range(0, free.Count)];
            room.SetAdditionalTile(tile, item);
            free.Remove(item);
        }
    }

    public void Generate(Locations location)
    {
        var tiles = LevelStorage.Instance.GetTilesContainer(location);
        if (_halls.Count > 0)
        {
            for (int i = 0; i < _halls.Count; ++i)
            {
                var dt = _rooms[i + 1].gameObject.transform.position -
                        _rooms[i].gameObject.transform.position;
                var size_i = _rooms[i].GetComponent<ISizeGetable>().Size;
                var size_i1 = _rooms[i + 1].GetComponent<ISizeGetable>().Size;
                var ds = (size_i + size_i1) / 2;
                Debug.Log($"{size_i} {size_i1}");
                if (dt.x != 0)
                {
                    _rooms[i].DoorTilemap.ActivateDoor(DoorDirection.Right);
                    _rooms[i + 1].DoorTilemap.ActivateDoor(DoorDirection.Left);
                    _halls[i].ExtendToLength(Mathf.RoundToInt(dt.magnitude - ds.x));
                    Debug.Log($"Горизонтальный коридор {i} {_halls[i].Size}");
                }
                else
                {
                    _halls[i].ExtendToLength(Mathf.RoundToInt(dt.magnitude - ds.y + 1));
                    if (dt.y > 0)
                    {
                        _rooms[i].DoorTilemap.ActivateDoor(DoorDirection.Up);
                        _rooms[i + 1].DoorTilemap.ActivateDoor(DoorDirection.Down);
                    }
                    else
                    {
                        _rooms[i].DoorTilemap.ActivateDoor(DoorDirection.Down);
                        _rooms[i + 1].DoorTilemap.ActivateDoor(DoorDirection.Up);
                    }
                }
            }
        }
        _rooms.ForEach(r =>
        {
            GenerateAdditional(r, tiles.Additional);
            r.GetComponent<IRoomController>()?.DoorController.SetDoors(r?.DoorTilemap.ActiveDoors);
            r.SwapTiles(tiles);
            r.GetComponent<IRoomController>()?.ActivateRoom();
            Destroy(r);
        });
        _halls.ForEach(h =>
        {
            h.SwapTiles(tiles);
            Destroy(h);
        });

        Destroy(LevelStorage.Instance.gameObject);
        Destroy(gameObject);
    }

    void Awake()
    {
        Instance = this;
    }
}
