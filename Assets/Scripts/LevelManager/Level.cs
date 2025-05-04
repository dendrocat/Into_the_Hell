using System;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Level
{
    public Transform transform;
    public List<GameObject> halls = null;
    public Matrix<Room> rooms = null;
    public Level(Transform transform)
    {
        this.transform = transform;
    }
    public bool created()
    {
        return halls != null && rooms != null;
    }
    public void create(Matrix<Room> rooms, List<GameObject> halls)
    {
        this.rooms = rooms;
        this.halls = halls;

        MiniMapUi.SetUpLevel(this);
    }
}