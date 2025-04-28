using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RoomContainer", menuName = "Scriptable Objects/RoomContainer")]
public class RoomContainer : ScriptableObject
{
    [SerializeField] GameObject _startRoom;

    [SerializeField] List<GameObject> _rooms;

    [SerializeField] GameObject _bossRoom;


    public GameObject StartRoom => _startRoom;

    public List<GameObject> Rooms => _rooms;

    public GameObject BossRoom => _bossRoom;
}
