
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    static LevelManager Instance = null;

    [SerializeField]
    Level level;
    
    public static LevelManager GetInstance(){
        return Instance;
    }
    GameObject[] rooms = null;
    GameObject[] halls = null;
    private void Start() {
        // Resources loading will be deleted;
        rooms = Resources.LoadAll<GameObject>("Rooms");
        halls = Resources.LoadAll<GameObject>("Halls");
        if(!level.created()){
            Generator.New()
                    .Rooms(new(rooms))
                    .Halls(halls[1], halls[0])
                    .FirstRoom(rooms[0])
                    .LastRoom(rooms[rooms.Length-1])
                    .Generate(10, level);
        }

        if(Instance == null)Instance = this;
    }

    LevelManager(){

    }
}