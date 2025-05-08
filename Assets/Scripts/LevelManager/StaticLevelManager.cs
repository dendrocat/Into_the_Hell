using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StaticLevelManager : AbstractLevelManager
{
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

    void SetAdditionalEffect(IRoomController room)
    {
        var additionalController = room.AdditionalController;
        if (additionalController == null) return;
        var trapContainer = _levelStorage.GetTrapContainer(_location);
        if (trapContainer == null) return;
        additionalController.SetAdditionalEffect(trapContainer);
    }

    public override void Generate()
    {
        if (_halls.Count > 0)
        {
            for (int i = 0; i < _halls.Count; ++i)
            {
                var dt = _rooms[i + 1].gameObject.transform.position -
                        _rooms[i].gameObject.transform.position;
                if (dt.x != 0)
                {
                    _rooms[i].DoorTilemap.ActivateDoor(DoorDirection.Right);
                    _rooms[i + 1].DoorTilemap.ActivateDoor(DoorDirection.Left);
                }
                else
                {
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
                _halls[i].ExtendToLength(10);
            }
        }
        var tiles = _levelStorage.GetTilesContainer(_location);
        _rooms.ForEach(r =>
        {
            GenerateAdditional(r, tiles.Additional);
            r.SwapTiles(tiles);
            if (r.TryGetComponent(out IRoomController roomController))
            {
                SetAdditionalEffect(roomController);
                roomController.DoorController.SetDoors(r.DoorTilemap.ActiveDoors);
                roomController.ActivateRoom();
            }
            Destroy(r);
        });
        _halls.ForEach(h =>
        {
            h.SwapTiles(tiles);
            Destroy(h);
        });

    }
}
