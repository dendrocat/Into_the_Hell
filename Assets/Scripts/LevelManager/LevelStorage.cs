using System;
using System.Collections.Generic;
using UnityEngine;

public enum Location
{
    FrozenCaves = 0, MineShaft = 1, HellCastle = 2, Final = 3
}

[CreateAssetMenu(fileName = "LevelStorage", menuName = "Storages/Level Storage")]
public class LevelStorage : ScriptableObject
{
    [Serializable]
    struct LocationContainer
    {
        public Location location;
        public TilesContainer tilesContainer;
        public RoomContainer roomContainer;


        public TrapContainer trapContainer;
        public EnemyContainer enemyContainer;
    }

    [SerializeField] List<LocationContainer> _location;

    [SerializeField] List<GameObject> _halls;

    public TilesContainer GetTilesContainer(Location location)
    {
        return _location[(int)location].tilesContainer;
    }

    public RoomContainer GetRoomContainer(Location location)
    {
        return _location[(int)location].roomContainer;
    }

    public List<GameObject> GetHalls()
    {
        return _halls;
    }

    public TrapContainer GetTrapContainer(Location location)
    {
        return _location[(int)location].trapContainer;
    }

    public EnemyContainer GetEnemyContainer(Location location)
    {
        return _location[(int)location].enemyContainer;
    }
}
