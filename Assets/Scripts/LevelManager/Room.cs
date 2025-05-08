using UnityEngine;
public class Room
{
    public int w { get; private set; }
    public int h { get; private set; }
    private GameObject room;
    private bool _isEndRoom;
    public bool _isStartRoom{ get; private set; }
    public Vector2 coord;
    public Room(GameObject prefab, Level level, bool isEndRoom = false, bool isStartRoom = false)
    {
        room = GameObject.Instantiate(prefab, level.transform);
        var col = room.GetComponent<ISizeGetable>().Size;
        w = col.x;
        h = col.y;
        _isEndRoom = isEndRoom;
        _isStartRoom = isStartRoom;
    }

    public int doors = 0;

    public void SetUp()
    {
        room.transform.position = (Vector3)coord;
        room.GetComponent<IRoomController>().DoorController.SetDoors(
            room.GetComponent<IRoomTilemapController>().DoorTilemap.ActiveDoors);
        if (_isEndRoom)
            room.GetComponent<IRoomController>().MarkAsEndRoom();
    }

    public void AddDoor(DoorDirection dir)
    {
        doors |= 1 << (int)dir;
        room.GetComponent<IRoomTilemapController>().
                DoorTilemap.ActivateDoor(dir);
    }

    public static void GenerateHall(Vector2 start, Vector2 fin, GameObject prefab, Level level)
    {
        GameObject hall = GameObject.Instantiate(prefab, level.transform);
        hall.transform.position = (Vector3)start;
        hall.GetComponent<IHallTilemapController>().
                ExtendToLength((int)(fin - start).magnitude);
        level.halls.Add(hall);
    }

    public IRoomTilemapController GetRoomTilemapController()
    {
        return room.GetComponent<IRoomTilemapController>();
    }

    public IRoomController GetRoomController()
    {
        return room.GetComponent<IRoomController>();
    }
}