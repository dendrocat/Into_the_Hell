using UnityEngine;
public class Room{ 
    public int w {get; private set; } 	
    public int h {get; private set; } 	
    private GameObject room; 
    public Vector2 coord;
    public Room(GameObject prefab, Level level){
        room = GameObject.Instantiate(prefab, level.transform);
        var col = room.GetComponent<ISizeGetable>().Size;
        w = col.x;
        h = col.y;
    }

    public int doors = 0;

    public void SetUp(){
        room.transform.position = (Vector3)coord;
        room.GetComponent<IRoomController>().DoorController.SetDoors(
            room.GetComponent<IRoomTilemapController>().DoorTilemap.ActiveDoors );
    }

    public void AddDoor(DoorDirection dir) {
        doors |= 1<<(int)dir;
        Debug.Log((room == null) + " " + (room?.GetComponent<IRoomController>() == null));
        room.GetComponent<IRoomTilemapController>().
                DoorTilemap.ActivateDoor(dir);
    }

    public static void GenerateHall(Vector2 start, Vector2 fin, GameObject prefab, Level level) {
        GameObject hall = GameObject.Instantiate(prefab, level.transform);
        hall.transform.position = (Vector3) start;
        hall.GetComponent<IHallTilemapController>().
                ExtendToLength((int)(fin-start).magnitude);
        level.halls.Add(hall);
    }
}