using System;
using System.Collections.Generic;
using UnityEngine;

public enum Locations
{
    FrozenCaves = 0, MineShaft = 1, HellCastle = 2, Final = 3
}

public class LevelStorage : MonoBehaviour
{
    public static LevelStorage Instance { get; private set; }

    [Serializable]
    struct LocationContainer
    {
        public Locations locations;
        public TilesContainer tilesContainer;
        public RoomContainer roomContainer;

        public TrapContainer trapContainer;
    }

    [SerializeField] List<LocationContainer> _location;

    [SerializeField] List<GameObject> _halls;

    void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    public TilesContainer GetTilesContainer(Locations location)
    {
        return _location[(int)location].tilesContainer;
    }

    public RoomContainer GetRoomContainer(Locations location)
    {
        return _location[(int)location].roomContainer;
    }

    public List<GameObject> GetHalls()
    {
        return _halls;
    }

    public TrapContainer GetTrapContainer(Locations location)
    {
        return _location[(int)location].trapContainer;
    }
}
