using UnityEngine;

public class LevelManager : MonoBehaviour, ILevelManager
{
    private LevelStorage _levelStorage;

    public static LevelManager Instance { get; private set; } = null;
    Level _level;

    GameObject[] rooms = null;
    GameObject[] halls = null;

    Locations _location;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
        _levelStorage = LevelStorage.Instance;
        _level = new(new GameObject("Rooms").transform);
    }


    void GenerateAdditional(IRoomTilemapController room,
                        TilesContainer container)
    {
        var free = room.GetFreePositions();
        int cnt = 0;
        float part = free.Count / 16;
        if (_location == Locations.FrozenCaves)
            part *= 2;
        while (cnt++ < part)
        {
            var item = free[Random.Range(0, free.Count)];
            room.SetAdditionalTile(container.Additional, item);
            free.Remove(item);
        }
    }

    void SetAdditionalEffect(Room room)
    {
        var additionalController = room.GetRoomController().AdditionalController;
        if (additionalController == null) return;
        var trapContainer = _levelStorage.GetTrapContainer(_location);
        if (trapContainer == null) return;
        additionalController.SetAdditionalEffect(trapContainer);
    }

    void MakeTiles(Room room, TilesContainer tiles)
    {
        var tilemap = room.GetRoomTilemapController();
        if (tiles.Additional == null)
            tilemap.RemoveAdditional();
        else
        {
            GenerateAdditional(tilemap, tiles);
            SetAdditionalEffect(room);
        }
        tilemap.SwapTiles(tiles);
    }


    public void Generate(Locations location)
    {
        _levelStorage = LevelStorage.Instance;
        if (_level.created()) return;
        _location = location;

        var roomContainer = _levelStorage.GetRoomContainer(_location);
        halls = _levelStorage.GetHalls().ToArray();
        rooms = roomContainer.Rooms.ToArray();
        var lastRoom = GameStorage.Instance.isLastLevel
                        ? roomContainer.BossRoom
                        : roomContainer.StartRoom;

        Generate(roomContainer.StartRoom, lastRoom, 15);

        var tiles = _levelStorage.GetTilesContainer(_location);
        _level.rooms.ForEach((r) =>
        {
            if (r == null) return;
            MakeTiles(r, tiles);
            Destroy(r.GetRoomTilemapController() as MonoBehaviour);
            r.GetRoomController().ActivateRoom();
        });

        _level.halls.ForEach((h) =>
            h.GetComponent<IHallTilemapController>().SwapTiles(tiles)
        );

        Destroy(LevelStorage.Instance.gameObject);
        Destroy(gameObject);
    }

    void Generate(GameObject startRoom,
                GameObject lastRoom,
                int countRooms)
    {
        Generator.New()
                .Rooms(new(rooms))
                .Halls(halls[0], halls[1])
                .FirstRoom(startRoom)
                .LastRoom(lastRoom)
                .Generate(countRooms, _level);
    }
}