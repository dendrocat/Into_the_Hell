using System.Collections.Generic;
using UnityEngine;

public class Level: MonoBehaviour{
    public List<GameObject> halls = null;
    public Matrix<Room> rooms = null;
    private Level() { }
    public bool created(){
        return halls != null && rooms != null;
    }
    public void create(Matrix<Room> rooms, List<GameObject> halls){
        this.rooms = rooms;
        this.halls = halls;
    }
}