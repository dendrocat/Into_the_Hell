using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Contoller for rooms
/// Must handle the game logic of the room
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class RoomContoller : MonoBehaviour
{
    [SerializeField] Optional<GameObject> BossSpawn;
    [SerializeField] Optional<GameObject> EnemySpawns;

    DoorController _doorController;

    void Awake()
    {
        _doorController = GetComponentInChildren<DoorController>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        throw new NotImplementedException("Logic for entering in room not implementing");
    }
}
